using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Collections;

namespace mysqlCSharp
{
    public partial class dbConnector : Form
    {
        ArrayList selectedUsers = new ArrayList();
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string pwd;
        public string tableName;
        public string person_oid;

        public dbConnector()
        {
            InitializeComponent();

        }
        public void Initialize()
        {
            server = srvName.Text;
            database = dbName.Text;
            uid = usrName.Text;
            pwd = pass.Text;
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + pwd + ";";
            logger.Items.Add(connectionString);
            connection = new MySqlConnection(connectionString);

        }
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                MessageBox.Show("Database Connected !");
                return true;
            }
            catch (MySqlException ex)
            {

                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        MessageBox.Show("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public void listTables()
        {
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SHOW TABLES";
            logger.Items.Add(cmd.CommandText);
            try
            {
                Initialize();
                connection.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string row = "";
                    for (int i = 0; i < reader.FieldCount; i++)
                        row += reader.GetValue(i).ToString();

                    listTable.Items.Add(row);
                }
                reader.Close();


            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Number.ToString());
                MessageBox.Show(ex.Message);
            }

        }
        private void submit_Click(object sender, EventArgs e)
        {
            Initialize();
            OpenConnection();
            getTbls.Enabled = true;
        }
        private void getData_Click(object sender, EventArgs e)
        {

            usersSelected.Enabled = true;
            selectUsers.Enabled = true;
            string tableName = listTable.Text;
            MySqlCommand command = connection.CreateCommand();
            string sql = "SELECT person_oid FROM " + tableName + " GROUP BY person_oid";
            command.CommandText = sql;
            logger.Items.Add(command.CommandText);
            MySqlDataReader Reader = command.ExecuteReader();

            while (Reader.Read())
            {
                string thisrow = "";
                for (int i = 0; i < Reader.FieldCount; i++)
                    thisrow += Reader.GetValue(i).ToString();

                personOidList.Items.Add(thisrow);

            }
            Reader.Close();
        }
        private void getTbls_Click(object sender, EventArgs e)
        {
            listTables();
            personOidList.Enabled = true;
            getData.Enabled = true;


        }
        private void button1_Click(object sender, System.EventArgs e)
        {
            testButton.Enabled = true;
            String strItem;
            foreach (Object selecteditem in personOidList.SelectedItems)
            {

                strItem = selecteditem as String;
                usersSelected.Items.Add(strItem);
                selectedUsers.Add(strItem);

            }
        }



        private void label5_Click(object sender, EventArgs e)
        {

        }
        private void label1_Click(object sender, EventArgs e)
        {

        }



        private void testButton_Click(object sender, EventArgs e)
        {

            for (int j = 0; j < usersSelected.Items.Count; j++)
            {
                string item = usersSelected.Items[j].ToString();
                string tableName = listTable.Text;
                MySqlCommand command = connection.CreateCommand();
                string testTable = "CREATE TABLE " + tableName + item + " LIKE " + tableName + "; INSERT INTO " + tableName + item + " SELECT * FROM " + tableName + " WHERE person_oid=" + item + " ORDER BY starttime;";
                command.CommandText = testTable;
                logger.Items.Add(command.CommandText);
                command.ExecuteNonQuery();
            }

        }

        private void ctID1_Click(object sender, EventArgs e)
        {
            //CREATE table call5101Y select * from callspan((Yüzde 75 olan) where person_oid = 51 order by (RAND) LIMIT 270
            //240 defa dönecek for loopu limit 18 diğerinde
            // Test % 25 olacak limit
            // select person_oid,count(*) from callspan;
            // 25lik kısmı 170 kullanıcı altında olan varsa callspan için kod yazılacak
            //select a.starttime,count(*) from cellspan as b, callspan as a where b.starttime<a.starttime and a.starttime<b.endtime and a.person_oid= 7 and b.person_oid=7 group by a.starttime
           
            MySqlCommand command = connection.CreateCommand();
            string tableName = listTable.Text;
            string createTable = "CREATE TABLE " + tableName+"16 LIKE " + tableName + ";";
            command.CommandText = createTable;
            logger.Items.Add(command.CommandText);
            command.ExecuteNonQuery();


            for (int j = 0; j < usersSelected.Items.Count; j++)
            {

                MySqlCommand command2 = connection.CreateCommand();
                string tempUser = selectedUsers[j].ToString();
                string InsertData = "INSERT INTO " + tableName+"16 SELECT * FROM " + tableName + " WHERE person_oid=" +tempUser+ " ORDER BY starttime;";
                string item = usersSelected.Items[j].ToString();
                command2.CommandText = InsertData;
                logger.Items.Add(command2.CommandText);
                command2.ExecuteNonQuery();
                
               // MessageBox.Show("This is "+ selectedUsers[j]);

            }

        }

        private void fillCTID_Click(object sender, EventArgs e)
        {

           for (int j = 0; j < usersSelected.Items.Count; j++)
            {

                MySqlCommand command2 = connection.CreateCommand();
                string tempUser = selectedUsers[j].ToString();
                string InsertData = "update callspan16 as a set ctid = if((select celltower_oid from cellspan16 where starttime<a.starttime and a.starttime<endtime and person_oid=a.person_oid)=null ,Null, (select celltower_oid from cellspan16 where starttime<a.starttime and a.starttime<endtime and person_oid=a.person_oid)) where person_oid="+tempUser;
                string item = usersSelected.Items[j].ToString();
                command2.CommandText = InsertData;
                logger.Items.Add(command2.CommandText);
                command2.ExecuteNonQuery();

                // MessageBox.Show("This is "+ selectedUsers[j]);

            }

        }

        private void dividerSmall_Click(object sender, EventArgs e)
        {
            
            string tblName1 = "callspan16";
            string tblName2 = "cellspan16";
            string tblName3 = "callspan";
            string tblName4 = "cellspan";
            string tblName5 = "activityspan";
            string tblName6 = "devicespan";

            for (int j = 0; j < usersSelected.Items.Count; j++)
            {
                string tempUser = selectedUsers[j].ToString();
                MySqlCommand command3 = connection.CreateCommand();
                string createTable;
                createTable = "CREATE TABLE " + tblName3 + tempUser + " LIKE " + tblName3 + ";";
                command3.CommandText = createTable;
                logger.Items.Add(command3.CommandText);
                command3.ExecuteNonQuery();
                createTable = "CREATE TABLE " + tblName4 + tempUser + " LIKE " + tblName4 + ";";
                command3.CommandText = createTable;
                logger.Items.Add(command3.CommandText);
                command3.ExecuteNonQuery();
                createTable = "CREATE TABLE " + tblName5 + tempUser + " LIKE " + tblName5 + ";";
                command3.CommandText = createTable;
                logger.Items.Add(command3.CommandText);
                command3.ExecuteNonQuery();
                createTable = "CREATE TABLE " + tblName6 + tempUser + " LIKE " + tblName6 + ";";
                command3.CommandText = createTable;
                logger.Items.Add(command3.CommandText);
                command3.ExecuteNonQuery();
            }

            for (int j = 0; j < usersSelected.Items.Count; j++)
            {


                MySqlCommand command = connection.CreateCommand();
                string tempUser = selectedUsers[j].ToString();
                string InsertData;
                InsertData = "INSERT INTO " + tblName3 + tempUser + " SELECT * FROM " + tblName1 + " WHERE person_oid=" + tempUser + " ORDER BY starttime;";
                command.CommandText = InsertData;
                logger.Items.Add(command.CommandText);
                command.ExecuteNonQuery();
                InsertData = "INSERT INTO " + tblName4 + tempUser + " SELECT * FROM " + tblName4 + " WHERE person_oid=" + tempUser + " ORDER BY starttime;";
                command.CommandText = InsertData;
                logger.Items.Add(command.CommandText);
                command.ExecuteNonQuery();
                InsertData = "INSERT INTO " + tblName5 + tempUser + " SELECT * FROM " + tblName5 + " WHERE person_oid=" + tempUser + " ORDER BY starttime;";
                command.CommandText = InsertData;
                logger.Items.Add(command.CommandText);
                command.ExecuteNonQuery();
                InsertData = "INSERT INTO " + tblName6 + tempUser + " SELECT * FROM " + tblName6 + " WHERE person_oid=" + tempUser + " ORDER BY starttime;";
                command.CommandText = InsertData;
                logger.Items.Add(command.CommandText);
                command.ExecuteNonQuery();



            }

        }


    }
}




