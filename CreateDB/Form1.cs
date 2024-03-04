using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace CreateDB
{
    public partial class Form1 : Form
    {
        private const string ConnectionString = "Server={0};Database={1};integrated security=true;Trusted_Connection=True;MultipleActiveResultSets=true;";

        //Integrated Security=SSPI;TrustServerCertificate=True;

        private SqlConnection connection = null;
        private readonly string DatabaseName = ConfigurationManager.AppSettings["DatabaseName"];
        private readonly string SchemaFilePath = AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["Schema"];
        private readonly string DbObjectFilePath = AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["DbObjects"];
        private readonly string MasterDataFilePath = AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["MasterData"];
        private readonly BackgroundWorker bgw = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };

        public Form1()
        {
            InitializeComponent();
            BtnCreateDb.Click += BtnCreateDb_Click;
            BtnClose.Click += (s, e) => { Application.ExitThread(); };
            BtnBrows.Click += BtnBrows_Click;
            bgw.DoWork += Bgw_DoWork;
            bgw.RunWorkerCompleted += Bgw_RunWorkerCompleted;
        }

        private void BtnBrows_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            DialogResult result = folderBrowser.ShowDialog();

            if (result == DialogResult.OK)
            { TxtFilePath.Text = folderBrowser.SelectedPath; }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            LblMsg.Text = "Please wait while collecting databases...";
            DisableControls();
            bgw.RunWorkerAsync();
        }

        private void Bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            List<string> Parameters = (List<string>)e.Result;
            CmbServers.DataSource = Parameters;
            progressBar1.Visible = false;
            EnableControls();
        }

        private void Bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            DataTable table = SqlDataSourceEnumerator.Instance.GetDataSources();
            List<string> Parameters = new List<string>();

            foreach (DataRow row in table.Rows)
            {
                if (!string.IsNullOrEmpty(row["InstanceName"].ToString()))
                { Parameters.Add(row["ServerName"].ToString() + "\\" + row["InstanceName"].ToString()); }
            }

            e.Result = Parameters;
        }

        private void BtnCreateDb_Click(object sender, EventArgs e)
        {
            DisableControls();
            if (string.IsNullOrEmpty(DatabaseName))
            {
                MessageBox.Show("Database name not defined.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }

            if (string.IsNullOrEmpty(TxtFilePath.Text))
            {
                MessageBox.Show("Please enter path to create database file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            else if (!Directory.Exists(TxtFilePath.Text))
            {
                MessageBox.Show("Given path is not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }

            ShowMessage("Creating Database...");
            connection = new SqlConnection(string.Format(ConnectionString, CmbServers.Text, "master"));

            bool IsDbCreated = false;
            Invoke(new MethodInvoker(delegate { IsDbCreated = CreateDatabase(connection); }));

            if (IsDbCreated && File.Exists(SchemaFilePath))
            {
                if (!File.Exists(SchemaFilePath))
                {
                    MessageBox.Show("Schema file not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    return;
                }

                ShowMessage("Creating Schemas...");
                connection = new SqlConnection(string.Format(ConnectionString, CmbServers.Text, DatabaseName));

                bool IsSchemaCreated = false;
                Invoke(new MethodInvoker(delegate { IsSchemaCreated = CreateSchema(connection); }));

                if (IsSchemaCreated)
                {
                    if (!File.Exists(DbObjectFilePath))
                    {
                        MessageBox.Show("Database object file not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        return;
                    }

                    ShowMessage("Creating database objects...");
                    this.connection = new SqlConnection(string.Format(ConnectionString, CmbServers.Text, DatabaseName));

                    bool IsDbObjectCreated = false;
                    Invoke(new MethodInvoker(delegate { IsDbObjectCreated = CreateDbObjects(connection); }));

                    if (IsDbObjectCreated)
                    {
                        if (!File.Exists(MasterDataFilePath))
                        {
                            MessageBox.Show("Master data file not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            return;
                        }

                        ShowMessage("Inserting master records...");
                        bool IsMasterDataInserted = false;
                        Invoke(new MethodInvoker(delegate { IsMasterDataInserted = InsertMasterData(connection); }));

                        if (IsMasterDataInserted)
                        {
                            ShowMessage("Database created successfully...");
                        }
                    }
                }
            }
            else
            {
                ShowMessage("Database not created...");
            }

            EnableControls();
        }

        public bool CreateDatabase(SqlConnection connection)
        {
            string appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            GrantAccess(appPath);
            bool IsExits = CheckDatabaseExists(connection, DatabaseName);

            if (!IsExits)
            {
                string CreateDatabase = $" CREATE DATABASE ApexAMCS ON PRIMARY " +
                                        $"(NAME = '{DatabaseName}', " +
                                        $"FILENAME = '{TxtFilePath.Text.Trim()}\\{DatabaseName}.mdf', " +
                                        $"SIZE = 10, " +
                                        $"MAXSIZE = " +
                                        $"UNLIMITED, " +
                                        $"FILEGROWTH = 5) " +
                                        $"LOG ON " +
                                        $"(NAME = '{DatabaseName}_Log', " +
                                        $"FILENAME = '{TxtFilePath.Text.Trim()}\\{DatabaseName}_Log.ldf', " +
                                        $"SIZE = 10, " +
                                        $"MAXSIZE = UNLIMITED, " +
                                        $"FILEGROWTH = 5) ";

                SqlCommand command = new SqlCommand(CreateDatabase, connection);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
                return true;
            }
            else
            { return true; }
        }

        public bool CreateSchema(SqlConnection connection)
        {
            bool IsExits = CheckDatabaseExists(connection, DatabaseName); //Check database exists in sql server.

            if (IsExits)
            {
                string CreateSchema = File.ReadAllText(SchemaFilePath);

                SqlCommand command = new SqlCommand(CreateSchema.Replace("GO", string.Empty), connection);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }

                return true;
            }

            return false;
        }

        public bool CreateDbObjects(SqlConnection connection)
        {
            bool IsExits = CheckDatabaseExists(connection, DatabaseName); //Check database exists in sql server.

            if (IsExits)
            {
                string CreateSchema = File.ReadAllText(DbObjectFilePath);
                MatchCollection objects = Regex.Matches(CreateSchema, "CREATE PROCEDURE.*?(?=SET ANSI_NULLS ON)", RegexOptions.IgnoreCase | RegexOptions.Singleline);

                foreach (Match match in objects)
                {
                    SqlCommand command = new SqlCommand(Regex.Match(match.Value.Trim(), ".*?(?=GO)", RegexOptions.IgnoreCase | RegexOptions.Singleline).Value.Trim(), connection);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                    }
                    finally
                    {
                        if (connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                        }
                    }
                }

                return true;
            }

            return false;
        }

        public bool InsertMasterData(SqlConnection connection)
        {
            bool IsExits = CheckDatabaseExists(connection, DatabaseName); //Check database exists in sql server.

            if (IsExits)
            {
                string CreateDatabase = File.ReadAllText(MasterDataFilePath);
                SqlCommand command = new SqlCommand(CreateDatabase.Replace("GO", string.Empty), connection);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
                return true;
            }

            return false;
        }

        public static bool GrantAccess(string fullPath)
        {
            try
            {
                DirectoryInfo info = new DirectoryInfo(fullPath);
                WindowsIdentity self = System.Security.Principal.WindowsIdentity.GetCurrent();
                DirectorySecurity ds = info.GetAccessControl();
                ds.AddAccessRule(new FileSystemAccessRule(self.Name, FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                info.SetAccessControl(ds);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public static bool CheckDatabaseExists(SqlConnection tmpConn, string databaseName)
        {
            string sqlCreateDBQuery;
            bool result = false;

            try
            {
                sqlCreateDBQuery = string.Format("SELECT database_id FROM sys.databases WHERE Name = '{0}'", databaseName);
                using (SqlCommand sqlCmd = new SqlCommand(sqlCreateDBQuery, tmpConn))
                {
                    tmpConn.Open();
                    object resultObj = sqlCmd.ExecuteScalar();
                    int databaseID = 0;

                    if (resultObj != null)
                    { int.TryParse(resultObj.ToString(), out databaseID); }

                    tmpConn.Close();
                    result = (databaseID > 0);
                }
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        private void EnableControls()
        {
            LblMsg.Visible = false;
            progressBar1.Visible = false;
            CmbServers.Enabled = true;
            TxtFilePath.Enabled = true;
            BtnCreateDb.Enabled = true;
            CmbServers.Focus();
        }

        private void DisableControls()
        {
            LblMsg.Visible = true;
            progressBar1.Visible = true;
            CmbServers.Enabled = false;
            TxtFilePath.Enabled = false;
            BtnCreateDb.Enabled = false;
        }

        private void ShowMessage(string Message)
        {
            Invoke(new MethodInvoker(delegate
            {
                Thread.Sleep(2000);
                LblMsg.Text = Message;
            }));
        }
    }
}
