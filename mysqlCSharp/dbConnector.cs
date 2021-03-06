﻿using System;
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
using System.IO;
using System.Diagnostics;

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
        public void Initialize()  // Database Connector
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
        } //Database Bağlantı Başlat
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
        } //Database Bağlantı kapat
        public void listTables()  // Tabloları Listelemek için fonksiyon
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
        private void submit_Click(object sender, EventArgs e)  // Girilen Değerlere göre DB Bağlantısı sağlamak için
        {
            Initialize();
            OpenConnection();
            getTbls.Enabled = true;
        }
        private void getData_Click(object sender, EventArgs e)  // Seçili olan tablodaki person_oid leri çekmek için fonksiyon
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
        private void getTbls_Click(object sender, EventArgs e) // Tabloları çeken fonksiyon
        {
            listTables();
            personOidList.Enabled = true;
            getData.Enabled = true;


        }
        private void button1_Click(object sender, System.EventArgs e) // Seçili olan person_oidleri Arraye atar
        {

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
        private void ctID1_Click(object sender, EventArgs e) // Seçili tablo ismi için seçili 16 kullanıcıyı toplar Callspan16 CellSpan16 gibi
        {
            //CREATE table call5101Y select * from callspan((Yüzde 75 olan) where person_oid = 51 order by (RAND) LIMIT 270
            //240 defa dönecek for loopu LIMIT 18 diğerinde
            // Test % 25 olacak LIMIT
            // select person_oid,count(*) from callspan;
            // 25lik kısmı 170 kullanıcı altında olan varsa callspan için kod yazılacak
            //select a.starttime,count(*) from cellspan as b, callspan as a where b.starttime<a.starttime and a.starttime<b.endtime and a.person_oid= 7 and b.person_oid=7 group by a.starttime

            MySqlCommand command = connection.CreateCommand();
            string tableName = listTable.Text;
            string createTable = "CREATE TABLE " + tableName + "16 LIKE " + tableName + ";";
            command.CommandText = createTable;
            logger.Items.Add(command.CommandText);
            command.ExecuteNonQuery();


            for (int j = 0; j < usersSelected.Items.Count; j++)
            {

                MySqlCommand command2 = connection.CreateCommand();
                string tempUser = selectedUsers[j].ToString();
                string InsertData = "INSERT INTO " + tableName + "16 SELECT * FROM " + tableName + " WHERE person_oid=" + tempUser + " ORDER BY starttime;";
                string item = usersSelected.Items[j].ToString();
                command2.CommandText = InsertData;
                logger.Items.Add(command2.CommandText);
                command2.ExecuteNonQuery();

                // MessageBox.Show("This is "+ selectedUsers[j]);

            }

        }
        private void fillCTID_Click(object sender, EventArgs e) // CTID Zor yolla doldurma fonksiyonu
        {
            for (int j = 0; j < usersSelected.Items.Count; j++)
            {

                MySqlCommand command2 = connection.CreateCommand();
                string tempUser = selectedUsers[j].ToString();
                string InsertData = "update callspan16 as a set ctid = if((select celltower_oid from cellspan16 where starttime<a.starttime and a.starttime<endtime and person_oid=a.person_oid)=null ,Null, (select celltower_oid from cellspan16 where starttime<a.starttime and a.starttime<endtime and person_oid=a.person_oid)) where person_oid=" + tempUser;
                string item = usersSelected.Items[j].ToString();
                command2.CommandText = InsertData;
                logger.Items.Add(command2.CommandText);
                command2.ExecuteNonQuery();

                // MessageBox.Show("This is "+ selectedUsers[j]);

            }

        }
        private void dividerSmall_Click(object sender, EventArgs e)  // Her Tablo için her Kullanıcının datalarını yazıyor Callspan4 Cellspan4 gibi...
        {


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
                InsertData = "INSERT INTO " + tblName3 + tempUser + " SELECT * FROM " + tblName3 + " WHERE person_oid=" + tempUser + " ORDER BY starttime;";
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
        private void tAndS_Click(object sender, EventArgs e)
        {


            string tblName3 = "callspan";
            string tblName4 = "cellspan";
            string tblName5 = "activityspan";
            string tblName6 = "devicespan";

            // SQL Döngü Countları
            double rowsCall = 0;
            double rowsCell = 0;
            double rowsActivity = 0;
            double rowsDevice = 0;
            // T Countları İçin
            double rowsCallT = 0;
            double rowsCellT = 0;
            double rowsActivityT = 0;
            double rowsDeviceT = 0;
            // S Countları İçin
            double rowsCallS = 0;
            double rowsCellS = 0;
            double rowsActivityS = 0;
            double rowsDeviceS = 0;


            for (int j = 0; j < usersSelected.Items.Count; j++)  // Her tablo için x kullancısının countunu al
            {

                string createTable;
                string InsertData;
                string alterData;
                string sql;
                string tempUser = selectedUsers[j].ToString();

                MySqlCommand command = connection.CreateCommand();

                // Callspan
                sql = "SELECT COUNT(*) FROM " + tblName3 + " where person_oid =" + tempUser + ";";
                command.CommandText = sql;
                rowsCall = Convert.ToDouble(command.ExecuteScalar());
                logger.Items.Add(command.CommandText);
                // Cellspan
                sql = "SELECT COUNT(*) FROM " + tblName4 + " where person_oid =" + tempUser + ";";
                command.CommandText = sql;
                rowsCell = Convert.ToDouble(command.ExecuteScalar());
                logger.Items.Add(command.CommandText);
                // Activityspan
                sql = "SELECT COUNT(*) FROM " + tblName5 + " where person_oid =" + tempUser + ";";
                command.CommandText = sql;
                rowsActivity = Convert.ToDouble(command.ExecuteScalar());
                logger.Items.Add(command.CommandText);
                // Devicespan
                sql = "SELECT COUNT(*) FROM " + tblName6 + " where person_oid =" + tempUser + ";";
                command.CommandText = sql;
                rowsDevice = Convert.ToDouble(command.ExecuteScalar());
                logger.Items.Add(command.CommandText);


                // TABLO OLUŞTURMA T VE S OLARAK

                MySqlCommand command3 = connection.CreateCommand();

                // % 75 Tabloları

                createTable = "CREATE TABLE " + tblName3 + tempUser + "T LIKE " + tblName3 + ";";
                command3.CommandText = createTable;
                logger.Items.Add(command3.CommandText);
                command3.ExecuteNonQuery();
                createTable = "CREATE TABLE " + tblName4 + tempUser + "T LIKE " + tblName4 + ";";
                command3.CommandText = createTable;
                logger.Items.Add(command3.CommandText);
                command3.ExecuteNonQuery();
                createTable = "CREATE TABLE " + tblName5 + tempUser + "T LIKE " + tblName5 + ";";
                command3.CommandText = createTable;
                logger.Items.Add(command3.CommandText);
                command3.ExecuteNonQuery();
                createTable = "CREATE TABLE " + tblName6 + tempUser + "T LIKE " + tblName6 + ";";
                command3.CommandText = createTable;
                logger.Items.Add(command3.CommandText);
                command3.ExecuteNonQuery();

                // % 25 Tabloları

                createTable = "CREATE TABLE " + tblName3 + tempUser + "S LIKE " + tblName3 + ";";
                command3.CommandText = createTable;
                logger.Items.Add(command3.CommandText);
                command3.ExecuteNonQuery();
                createTable = "CREATE TABLE " + tblName4 + tempUser + "S LIKE " + tblName4 + ";";
                command3.CommandText = createTable;
                logger.Items.Add(command3.CommandText);
                command3.ExecuteNonQuery();
                createTable = "CREATE TABLE " + tblName5 + tempUser + "S LIKE " + tblName5 + ";";
                command3.CommandText = createTable;
                logger.Items.Add(command3.CommandText);
                command3.ExecuteNonQuery();
                createTable = "CREATE TABLE " + tblName6 + tempUser + "S LIKE " + tblName6 + ";";
                command3.CommandText = createTable;
                logger.Items.Add(command3.CommandText);
                command3.ExecuteNonQuery();


                // T VE S YE LIMIT KADAR KULLANICILARI ATMAK IÇIN


                rowsCallT = Math.Floor(rowsCall * 0.75);
                rowsCallS = Math.Ceiling(rowsCall * 0.25);

                rowsCellT = Math.Floor(rowsCell * 0.75);
                rowsCellS = Math.Ceiling(rowsCell * 0.25);

                rowsActivityT = Math.Floor(rowsActivity * 0.75);
                rowsActivityS = Math.Ceiling(rowsActivity * 0.25);

                rowsDeviceT = Math.Floor(rowsDevice * 0.75);
                rowsDeviceS = Math.Ceiling(rowsDevice * 0.25);

                //User 4 1011 Count
                //INSERT into call4t SELECT * from callspan where person_oid = 4 ORDER by starttime LIMIT 758;
                //INSERT into call4t SELECT * from callspan where person_oid = 4 ORDER by starttime DESC LIMIT 253;
                //ALTER Table call4t order by starttime;

                MySqlCommand command4 = connection.CreateCommand();


                // T lerinin yapılması
                InsertData = "INSERT INTO " + tblName3 + tempUser + "T SELECT * FROM " + tblName3 + " WHERE person_oid=" + tempUser + " ORDER BY starttime LIMIT " + rowsCallT + ";";
                command4.CommandText = InsertData;
                logger.Items.Add(command4.CommandText);
                command4.ExecuteNonQuery();

                InsertData = "INSERT INTO " + tblName4 + tempUser + "T SELECT * FROM " + tblName4 + " WHERE person_oid=" + tempUser + " ORDER BY starttime LIMIT " + rowsCellT + ";";
                command4.CommandText = InsertData;
                logger.Items.Add(command4.CommandText);
                command4.ExecuteNonQuery();

                InsertData = "INSERT INTO " + tblName5 + tempUser + "T SELECT * FROM " + tblName5 + " WHERE person_oid=" + tempUser + " ORDER BY starttime LIMIT " + rowsActivityT + ";";
                command4.CommandText = InsertData;
                logger.Items.Add(command4.CommandText);
                command4.ExecuteNonQuery();

                InsertData = "INSERT INTO " + tblName6 + tempUser + "T SELECT * FROM " + tblName6 + " WHERE person_oid=" + tempUser + " ORDER BY starttime LIMIT " + rowsDeviceT + ";";
                command4.CommandText = InsertData;
                logger.Items.Add(command4.CommandText);
                command4.ExecuteNonQuery();

                // S lerinin yapılması
                InsertData = "INSERT INTO " + tblName3 + tempUser + "S SELECT * FROM " + tblName3 + " WHERE person_oid=" + tempUser + " ORDER BY starttime DESC LIMIT " + rowsCallS + ";";
                command4.CommandText = InsertData;
                logger.Items.Add(command4.CommandText);
                command4.ExecuteNonQuery();

                InsertData = "INSERT INTO " + tblName4 + tempUser + "S SELECT * FROM " + tblName4 + " WHERE person_oid=" + tempUser + " ORDER BY starttime DESC LIMIT " + rowsCellS + ";";
                command4.CommandText = InsertData;
                logger.Items.Add(command4.CommandText);
                command4.ExecuteNonQuery();

                InsertData = "INSERT INTO " + tblName5 + tempUser + "S SELECT * FROM " + tblName5 + " WHERE person_oid=" + tempUser + " ORDER BY starttime DESC LIMIT " + rowsActivityS + ";";
                command4.CommandText = InsertData;
                logger.Items.Add(command4.CommandText);
                command4.ExecuteNonQuery();

                InsertData = "INSERT INTO " + tblName6 + tempUser + "S SELECT * FROM " + tblName6 + " WHERE person_oid=" + tempUser + " ORDER BY starttime DESC LIMIT " + rowsDeviceS + ";";
                command4.CommandText = InsertData;
                logger.Items.Add(command4.CommandText);
                command4.ExecuteNonQuery();

                //DESC LIMIT sondan aldığı için tekrar Order by Starttime yapmak gerekiyor
                //ALTER Table call4t order by starttime;

                alterData = "ALTER Table " + tblName3 + tempUser + "S ORDER BY starttime;";
                command4.CommandText = alterData;
                logger.Items.Add(command4.CommandText);
                command4.ExecuteNonQuery();

                alterData = "ALTER Table " + tblName4 + tempUser + "S ORDER BY starttime;";
                command4.CommandText = alterData;
                logger.Items.Add(command4.CommandText);
                command4.ExecuteNonQuery();

                alterData = "ALTER Table " + tblName5 + tempUser + "S ORDER BY starttime;";
                command4.CommandText = alterData;
                logger.Items.Add(command4.CommandText);
                command4.ExecuteNonQuery();

                alterData = "ALTER Table " + tblName6 + tempUser + "S ORDER BY starttime;";
                command4.CommandText = alterData;
                logger.Items.Add(command4.CommandText);
                command4.ExecuteNonQuery();


            }

        }// Fonksiyon %75 ve %25 bölüyor
        private void cDataset_Click(object sender, EventArgs e)  // yeni tabloları oluşturdu , 270 18 gibi girilen sayılara göre rand ayırdı . PIDlerini doldurdu
        {
            int CallYNo = 270;
            int CallNNo = 18;
            int CellYNo = 6000;
            int CellNNo = 400;
            int ActvYNo = 540;
            int ActvNNo = 36;
            int DevcYNo = 360;
            int DevcNNo = 24;
            string createTable;
            string updatePID;
            string dataSet;
            string InsertInto;

            MySqlCommand commandD = connection.CreateCommand();
            MySqlCommand commandY = connection.CreateCommand();
            MySqlCommand commandDS = connection.CreateCommand();
            MySqlCommand commandDX = connection.CreateCommand();
            //Create table call5101Y select * from callspan%75 where person-oid=51 order by RAND() LIMIT 270

            for (int j = 0; j < usersSelected.Items.Count; j++)  // Her tablo için x kullancısının countunu al
            {
                string tempUser = selectedUsers[j].ToString();

                for (int i = 1; i <= 15; i++)
                {

                    if (i < 10)
                    {

                        //CallSpan Y
                        createTable = "CREATE TABLE CALL" + tempUser + "0" + i + "Y SELECT * from callspan" + tempUser + "t WHERE person_oid=" + tempUser + " ORDER BY RAND() LIMIT " + CallYNo + ";";
                        updatePID = "UPDATE CALL" + tempUser + "0" + i + "Y SET PID = \"YES\" ;";
                        commandD.CommandText = createTable;
                        commandY.CommandText = updatePID;
                        logger.Items.Add(commandD.CommandText);
                        logger.Items.Add(commandY.CommandText);
                        commandD.ExecuteNonQuery();
                        commandY.ExecuteNonQuery();

                        //CallSpan N
                        createTable = "CREATE TABLE CALL" + tempUser + "0" + i + "N SELECT * from callspan" + tempUser + "t WHERE person_oid=" + tempUser + " ORDER BY RAND() LIMIT " + CallNNo + ";";
                        updatePID = "UPDATE CALL" + tempUser + "0" + i + "N SET PID = \"NO\";";
                        commandD.CommandText = createTable;
                        commandY.CommandText = updatePID;
                        logger.Items.Add(commandD.CommandText);
                        logger.Items.Add(commandY.CommandText);
                        commandD.ExecuteNonQuery();
                        commandY.ExecuteNonQuery();


                        //CellSpan Y
                        createTable = "CREATE TABLE CELL" + tempUser + "0" + i + "Y SELECT * from cellspan" + tempUser + "t WHERE person_oid=" + tempUser + " ORDER BY RAND() LIMIT " + CellYNo + ";";
                        updatePID = "UPDATE CELL" + tempUser + "0" + i + "Y SET PID = \"YES\";";
                        commandD.CommandText = createTable;
                        commandY.CommandText = updatePID;
                        logger.Items.Add(commandD.CommandText);
                        logger.Items.Add(commandY.CommandText);
                        commandD.ExecuteNonQuery();
                        commandY.ExecuteNonQuery();

                        //CellSpan N
                        createTable = "CREATE TABLE CELL" + tempUser + "0" + i + "N SELECT * from cellspan" + tempUser + "t WHERE person_oid=" + tempUser + " ORDER BY RAND() LIMIT " + CellNNo + ";";
                        updatePID = "UPDATE CELL" + tempUser + "0" + i + "N SET PID = \"NO\";";
                        commandD.CommandText = createTable;
                        commandY.CommandText = updatePID;
                        logger.Items.Add(commandD.CommandText);
                        logger.Items.Add(commandY.CommandText);
                        commandD.ExecuteNonQuery();
                        commandY.ExecuteNonQuery();


                        //ActiviySpan Y
                        createTable = "CREATE TABLE ACTIVITY" + tempUser + "0" + i + "Y SELECT * from activityspan" + tempUser + "t WHERE person_oid=" + tempUser + " ORDER BY RAND() LIMIT " + ActvYNo + ";";
                        updatePID = "UPDATE ACTIVITY" + tempUser + "0" + i + "Y SET PID = \"YES\";";
                        commandD.CommandText = createTable;
                        commandY.CommandText = updatePID;
                        logger.Items.Add(commandD.CommandText);
                        logger.Items.Add(commandY.CommandText);
                        commandD.ExecuteNonQuery();
                        commandY.ExecuteNonQuery();

                        //ActivitySpan N
                        createTable = "CREATE TABLE ACTIVITY" + tempUser + "0" + i + "N SELECT * from activityspan" + tempUser + "t WHERE person_oid=" + tempUser + " ORDER BY RAND() LIMIT " + ActvNNo + ";";
                        updatePID = "UPDATE ACTIVITY" + tempUser + "0" + i + "N SET PID = \"NO\";";
                        commandD.CommandText = createTable;
                        commandY.CommandText = updatePID;
                        logger.Items.Add(commandD.CommandText);
                        logger.Items.Add(commandY.CommandText);
                        commandD.ExecuteNonQuery();
                        commandY.ExecuteNonQuery();

                        //DeviceSpan Y
                        createTable = "CREATE TABLE DEVICE" + tempUser + "0" + i + "Y SELECT * from devicespan" + tempUser + "t WHERE person_oid=" + tempUser + " ORDER BY RAND() LIMIT " + DevcYNo + ";";
                        updatePID = "UPDATE DEVICE" + tempUser + "0" + i + "Y SET PID = \"YES\";";
                        commandD.CommandText = createTable;
                        commandY.CommandText = updatePID;
                        logger.Items.Add(commandD.CommandText);
                        logger.Items.Add(commandY.CommandText);
                        commandD.ExecuteNonQuery();
                        commandY.ExecuteNonQuery();

                        //DeviceSpan N
                        createTable = "CREATE TABLE DEVICE" + tempUser + "0" + i + "N SELECT * from devicespan" + tempUser + "t WHERE person_oid=" + tempUser + " ORDER BY RAND() LIMIT " + DevcNNo + ";";
                        updatePID = "UPDATE DEVICE" + tempUser + "0" + i + "N SET PID = \"NO\";";
                        commandD.CommandText = createTable;
                        commandY.CommandText = updatePID;
                        logger.Items.Add(commandD.CommandText);
                        logger.Items.Add(commandY.CommandText);
                        commandD.ExecuteNonQuery();
                        commandY.ExecuteNonQuery();

                    }

                    if (i >= 10)
                    {

                        //CallSpan Y
                        createTable = "CREATE TABLE CALL" + tempUser + i + "Y SELECT * from callspan" + tempUser + "t WHERE person_oid=" + tempUser + " ORDER BY RAND() LIMIT " + CallYNo + ";";
                        updatePID = "UPDATE CALL" + tempUser + i + "Y SET PID = \"YES\";";
                        commandD.CommandText = createTable;
                        commandY.CommandText = updatePID;
                        logger.Items.Add(commandD.CommandText);
                        logger.Items.Add(commandY.CommandText);
                        commandD.ExecuteNonQuery();
                        commandY.ExecuteNonQuery();

                        //CallSpan N
                        createTable = "CREATE TABLE CALL" + tempUser + i + "N SELECT * from callspan" + tempUser + "t WHERE person_oid=" + tempUser + " ORDER BY RAND() LIMIT " + CallNNo + ";";
                        updatePID = "UPDATE CALL" + tempUser + i + "N SET PID = \"NO\";";
                        commandD.CommandText = createTable;
                        commandY.CommandText = updatePID;
                        logger.Items.Add(commandD.CommandText);
                        logger.Items.Add(commandY.CommandText);
                        commandD.ExecuteNonQuery();
                        commandY.ExecuteNonQuery();


                        //CellSpan Y
                        createTable = "CREATE TABLE CELL" + tempUser + i + "Y SELECT * from cellspan" + tempUser + "t WHERE person_oid=" + tempUser + " ORDER BY RAND() LIMIT " + CellYNo + ";";
                        updatePID = "UPDATE CELL" + tempUser + i + "Y SET PID = \"YES\";";
                        commandD.CommandText = createTable;
                        commandY.CommandText = updatePID;
                        logger.Items.Add(commandD.CommandText);
                        logger.Items.Add(commandY.CommandText);
                        commandD.ExecuteNonQuery();
                        commandY.ExecuteNonQuery();

                        //CellSpan N
                        createTable = "CREATE TABLE CELL" + tempUser + i + "N SELECT * from cellspan" + tempUser + "t WHERE person_oid=" + tempUser + " ORDER BY RAND() LIMIT " + CellNNo + ";";
                        updatePID = "UPDATE CELL" + tempUser + i + "N SET PID = \"NO\";";
                        commandD.CommandText = createTable;
                        commandY.CommandText = updatePID;
                        logger.Items.Add(commandD.CommandText);
                        logger.Items.Add(commandY.CommandText);
                        commandD.ExecuteNonQuery();
                        commandY.ExecuteNonQuery();


                        //ActiviySpan Y
                        createTable = "CREATE TABLE ACTIVITY" + tempUser + i + "Y SELECT * from activityspan" + tempUser + "t WHERE person_oid=" + tempUser + " ORDER BY RAND() LIMIT " + ActvYNo + ";";
                        updatePID = "UPDATE ACTIVITY" + tempUser + i + "Y SET PID = \"YES\";";
                        commandD.CommandText = createTable;
                        commandY.CommandText = updatePID;
                        logger.Items.Add(commandD.CommandText);
                        logger.Items.Add(commandY.CommandText);
                        commandD.ExecuteNonQuery();
                        commandY.ExecuteNonQuery();

                        //ActivitySpan N
                        createTable = "CREATE TABLE ACTIVITY" + tempUser + i + "N SELECT * from activityspan" + tempUser + "t WHERE person_oid=" + tempUser + " ORDER BY RAND() LIMIT " + ActvNNo + ";";
                        updatePID = "UPDATE ACTIVITY" + tempUser + i + "N SET PID = \"NO\";";
                        commandD.CommandText = createTable;
                        commandY.CommandText = updatePID;
                        logger.Items.Add(commandD.CommandText);
                        logger.Items.Add(commandY.CommandText);
                        commandD.ExecuteNonQuery();
                        commandY.ExecuteNonQuery();

                        //DeviceSpan Y
                        createTable = "CREATE TABLE DEVICE" + tempUser + i + "Y SELECT * from devicespan" + tempUser + "t WHERE person_oid=" + tempUser + " ORDER BY RAND() LIMIT " + DevcYNo + ";";
                        updatePID = "UPDATE DEVICE" + tempUser + i + "Y SET PID = \"YES\";";
                        commandD.CommandText = createTable;
                        commandY.CommandText = updatePID;
                        logger.Items.Add(commandD.CommandText);
                        logger.Items.Add(commandY.CommandText);
                        commandD.ExecuteNonQuery();
                        commandY.ExecuteNonQuery();

                        //DeviceSpan N
                        createTable = "CREATE TABLE DEVICE" + tempUser + i + "N SELECT * from devicespan" + tempUser + "t WHERE person_oid=" + tempUser + " ORDER BY RAND() LIMIT " + DevcNNo + ";";
                        updatePID = "UPDATE DEVICE" + tempUser + i + "N SET PID = \"NO\";";
                        commandD.CommandText = createTable;
                        commandY.CommandText = updatePID;
                        logger.Items.Add(commandD.CommandText);
                        logger.Items.Add(commandY.CommandText);
                        commandD.ExecuteNonQuery();
                        commandY.ExecuteNonQuery();

                    }

                }


            }

            //Message Box
            MessageBox.Show("CALL, CELL, ACTIVITY, DEVICE Spans were divided and filled to Y and N for 15 Times \n PID filled Yes and No respectively");

            for (int j = 0; j < usersSelected.Items.Count; j++)  // Her tablo için x kullancısının countunu al
            {
                string tempUser = selectedUsers[j].ToString();

                for (int i = 1; i <= 15; i++)
                {

                    if (i < 10)
                    {

                        // DATASETCALL için
                        dataSet = "CREATE TABLE DataSetCALL" + tempUser + "_0" + i + " SELECT * from call" + tempUser + "0" + i + "Y WHERE person_oid=" + tempUser + " ORDER BY RAND() LIMIT " + CallYNo + ";";
                        commandDS.CommandText = dataSet;
                        logger.Items.Add(commandDS.CommandText);
                        commandDS.ExecuteNonQuery();

                        for (int curr = 0; curr < usersSelected.Items.Count; curr++)
                        {
                            string currUser = selectedUsers[curr].ToString();

                            if (tempUser.Equals(currUser))
                            {
                                continue;
                            }
                            else
                            {
                                InsertInto = "INSERT INTO DataSetCALL" + tempUser + "_0" + i + " SELECT * from call" + currUser + "0" + i + "N WHERE person_oid=" + currUser + " ORDER BY RAND() LIMIT " + CallNNo + ";";
                                commandDX.CommandText = InsertInto;
                                logger.Items.Add(commandDX.CommandText);
                                commandDX.ExecuteNonQuery();
                            }

                        }
                        // DATASETCELL için
                        dataSet = "CREATE TABLE DataSetCELL" + tempUser + "_0" + i + " SELECT * from cell" + tempUser + "0" + i + "Y WHERE person_oid=" + tempUser + " ORDER BY RAND() LIMIT " + CellYNo + ";";
                        commandDS.CommandText = dataSet;
                        logger.Items.Add(commandDS.CommandText);
                        commandDS.ExecuteNonQuery();

                        for (int curr = 0; curr < usersSelected.Items.Count; curr++)
                        {
                            string currUser = selectedUsers[curr].ToString();

                            if (tempUser.Equals(currUser))
                            {
                                continue;
                            }
                            else
                            {
                                InsertInto = "INSERT INTO DataSetCELL" + tempUser + "_0" + i + " SELECT * from cell" + currUser + "0" + i + "N WHERE person_oid=" + currUser + " ORDER BY RAND() LIMIT " + CellNNo + ";";
                                commandDX.CommandText = InsertInto;
                                logger.Items.Add(commandDX.CommandText);
                                commandDX.ExecuteNonQuery();
                            }

                        }
                        // DATASETACTIVITY için
                        dataSet = "CREATE TABLE DataSetACTIVITY" + tempUser + "_0" + i + " SELECT * from activity" + tempUser + "0" + i + "Y WHERE person_oid=" + tempUser + " ORDER BY RAND() LIMIT " + ActvYNo + ";";
                        commandDS.CommandText = dataSet;
                        logger.Items.Add(commandDS.CommandText);
                        commandDS.ExecuteNonQuery();

                        for (int curr = 0; curr < usersSelected.Items.Count; curr++)
                        {
                            string currUser = selectedUsers[curr].ToString();

                            if (tempUser.Equals(currUser))
                            {
                                continue;
                            }
                            else
                            {
                                InsertInto = "INSERT INTO DataSetACTIVITY" + tempUser + "_0" + i + " SELECT * from activity" + currUser + "0" + i + "N WHERE person_oid=" + currUser + " ORDER BY RAND() LIMIT " + ActvNNo + ";";
                                commandDX.CommandText = InsertInto;
                                logger.Items.Add(commandDX.CommandText);
                                commandDX.ExecuteNonQuery();
                            }

                        }
                        // DATASETDEVICE için
                        dataSet = "CREATE TABLE DataSetDEVICE" + tempUser + "_0" + i + " SELECT * from device" + tempUser + "0" + i + "Y WHERE person_oid=" + tempUser + " ORDER BY RAND() LIMIT " + DevcYNo + ";";
                        commandDS.CommandText = dataSet;
                        logger.Items.Add(commandDS.CommandText);
                        commandDS.ExecuteNonQuery();

                        for (int curr = 0; curr < usersSelected.Items.Count; curr++)
                        {
                            string currUser = selectedUsers[curr].ToString();

                            if (tempUser.Equals(currUser))
                            {
                                continue;
                            }
                            else
                            {
                                InsertInto = "INSERT INTO DataSetDEVICE" + tempUser + "_0" + i + " SELECT * from device" + currUser + "0" + i + "N WHERE person_oid=" + currUser + " ORDER BY RAND() LIMIT " + DevcNNo + ";";
                                commandDX.CommandText = InsertInto;
                                logger.Items.Add(commandDX.CommandText);
                                commandDX.ExecuteNonQuery();
                            }

                        }

                    }

                    if (i >= 10)
                    {

                        // DATASETCALL için
                        dataSet = "CREATE TABLE DataSetCALL" + tempUser + "_" + i + " SELECT * from call" + tempUser + i + "Y WHERE person_oid=" + tempUser + " ORDER BY RAND() LIMIT " + CallYNo + ";";
                        commandDS.CommandText = dataSet;
                        logger.Items.Add(commandDS.CommandText);
                        commandDS.ExecuteNonQuery();

                        for (int curr = 0; curr < usersSelected.Items.Count; curr++)
                        {
                            string currUser = selectedUsers[curr].ToString();

                            if (tempUser.Equals(currUser))
                            {
                                continue;
                            }
                            else
                            {
                                InsertInto = "INSERT INTO DataSetCALL" + tempUser + "_" + i + " SELECT * from call" + currUser + i + "N WHERE person_oid=" + currUser + " ORDER BY RAND() LIMIT " + CallNNo + ";";
                                commandDX.CommandText = InsertInto;
                                logger.Items.Add(commandDX.CommandText);
                                commandDX.ExecuteNonQuery();
                            }

                        }
                        // DATASETCELL için
                        dataSet = "CREATE TABLE DataSetCELL" + tempUser + "_" + i + " SELECT * from cell" + tempUser + i + "Y WHERE person_oid=" + tempUser + " ORDER BY RAND() LIMIT " + CellYNo + ";";
                        commandDS.CommandText = dataSet;
                        logger.Items.Add(commandDS.CommandText);
                        commandDS.ExecuteNonQuery();

                        for (int curr = 0; curr < usersSelected.Items.Count; curr++)
                        {
                            string currUser = selectedUsers[curr].ToString();

                            if (tempUser.Equals(currUser))
                            {
                                continue;
                            }
                            else
                            {
                                InsertInto = "INSERT INTO DataSetCELL" + tempUser + "_" + i + " SELECT * from cell" + currUser + i + "N WHERE person_oid=" + currUser + " ORDER BY RAND() LIMIT " + CellNNo + ";";
                                commandDX.CommandText = InsertInto;
                                logger.Items.Add(commandDX.CommandText);
                                commandDX.ExecuteNonQuery();
                            }

                        }
                        // DATASETACTIVITY için
                        dataSet = "CREATE TABLE DataSetACTIVITY" + tempUser + "_" + i + " SELECT * from activity" + tempUser + i + "Y WHERE person_oid=" + tempUser + " ORDER BY RAND() LIMIT " + ActvYNo + ";";
                        commandDS.CommandText = dataSet;
                        logger.Items.Add(commandDS.CommandText);
                        commandDS.ExecuteNonQuery();

                        for (int curr = 0; curr < usersSelected.Items.Count; curr++)
                        {
                            string currUser = selectedUsers[curr].ToString();

                            if (tempUser.Equals(currUser))
                            {
                                continue;
                            }
                            else
                            {
                                InsertInto = "INSERT INTO DataSetACTIVITY" + tempUser + "_" + i + " SELECT * from activity" + currUser + i + "N WHERE person_oid=" + currUser + " ORDER BY RAND() LIMIT " + ActvNNo + ";";
                                commandDX.CommandText = InsertInto;
                                logger.Items.Add(commandDX.CommandText);
                                commandDX.ExecuteNonQuery();
                            }

                        }
                        // DATASETDEVICE için
                        dataSet = "CREATE TABLE DataSetDEVICE" + tempUser + "_" + i + " SELECT * from device" + tempUser + i + "Y WHERE person_oid=" + tempUser + " ORDER BY RAND() LIMIT " + DevcYNo + ";";
                        commandDS.CommandText = dataSet;
                        logger.Items.Add(commandDS.CommandText);
                        commandDS.ExecuteNonQuery();

                        for (int curr = 0; curr < usersSelected.Items.Count; curr++)
                        {
                            string currUser = selectedUsers[curr].ToString();

                            if (tempUser.Equals(currUser))
                            {
                                continue;
                            }
                            else
                            {
                                InsertInto = "INSERT INTO DataSetDEVICE" + tempUser + "_" + i + " SELECT * from device" + currUser + i + "N WHERE person_oid=" + currUser + " ORDER BY RAND() LIMIT " + DevcNNo + ";";
                                commandDX.CommandText = InsertInto;
                                logger.Items.Add(commandDX.CommandText);
                                commandDX.ExecuteNonQuery();
                            }

                        }
                    }

                }// end of 15 for loop

            }//end of userselect For

            MessageBox.Show("DataSET prep completed.");

            // SELECT  COUNT(*) FROM calltest2_51;



        }
        private void createTest_Click(object sender, EventArgs e)
        {
            //Create table calltest2_51 select * from callspan51s where person-oid=51 order by Rand() limit 170
            //Create table calltest1_51 select * from calltest2_51 order by Rand() limit 90
            //Delete from calltest2_51 limit 90
            //Insert into calltest2_51 select * from calltest1_51 order by rand limit 80-(SELECT  COUNT(*) FROM calltest2_51)

            int CallS1 = 90;
            int CallS2 = 80;
            int CellS1 = 2400;
            int CellS2 = 2000;
            int ActvS1 = 180;
            int ActvS2 = 160;
            int DevcS1 = 120;
            int DevcS2 = 100;


            string insertInto;
            string createTable;
            string updatePID;


            MySqlCommand commandT = connection.CreateCommand();

            for (int j = 0; j < usersSelected.Items.Count; j++)  // Her tablo için x kullancısının countunu al
            {
                string tempUser = selectedUsers[j].ToString();


                //CallTESTS
                createTable = "CREATE TABLE CALLTEST2_" + tempUser + " SELECT * from callspan" + tempUser + "s WHERE person_oid=" + tempUser + " ORDER BY RAND() LIMIT " + CallS1 + ";";
                commandT.CommandText = createTable;
                logger.Items.Add(commandT.CommandText);
                commandT.ExecuteNonQuery();

                createTable = "CREATE TABLE CALLTEST1_" + tempUser + " SELECT * from callspan" + tempUser + "s WHERE person_oid=" + tempUser + " ORDER BY RAND() LIMIT " + CallS2 + ";";
                commandT.CommandText = createTable;
                logger.Items.Add(commandT.CommandText);
                commandT.ExecuteNonQuery();

                //CellTESTS
                createTable = "CREATE TABLE CELLTEST2_" + tempUser + " SELECT * from cellspan" + tempUser + "s WHERE person_oid=" + tempUser + " ORDER BY RAND() LIMIT " + CellS1 + ";";
                commandT.CommandText = createTable;
                logger.Items.Add(commandT.CommandText);
                commandT.ExecuteNonQuery();

                createTable = "CREATE TABLE CELLTEST1_" + tempUser + " SELECT * from cellspan" + tempUser + "s WHERE person_oid=" + tempUser + " ORDER BY RAND() LIMIT " + CellS2 + ";";
                commandT.CommandText = createTable;
                logger.Items.Add(commandT.CommandText);
                commandT.ExecuteNonQuery();

                //ActivityTESTS
                createTable = "CREATE TABLE ACTIVITYTEST2_" + tempUser + " SELECT * from activityspan" + tempUser + "s WHERE person_oid=" + tempUser + " ORDER BY RAND() LIMIT " + ActvS1 + ";";
                commandT.CommandText = createTable;
                logger.Items.Add(commandT.CommandText);
                commandT.ExecuteNonQuery();

                createTable = "CREATE TABLE ACTIVITYTEST1_" + tempUser + " SELECT * from activityspan" + tempUser + "s WHERE person_oid=" + tempUser + " ORDER BY RAND() LIMIT " + ActvS2 + ";";
                commandT.CommandText = createTable;
                logger.Items.Add(commandT.CommandText);
                commandT.ExecuteNonQuery();

                //DeviceTESTS
                createTable = "CREATE TABLE DEVICETEST2_" + tempUser + " SELECT * from devicespan" + tempUser + "s WHERE person_oid=" + tempUser + " ORDER BY RAND() LIMIT " + DevcS1 + ";";
                commandT.CommandText = createTable;
                logger.Items.Add(commandT.CommandText);
                commandT.ExecuteNonQuery();

                createTable = "CREATE TABLE DEVICETEST1_" + tempUser + " SELECT * from devicespan" + tempUser + "s WHERE person_oid=" + tempUser + " ORDER BY RAND() LIMIT " + DevcS2 + ";";
                commandT.CommandText = createTable;
                logger.Items.Add(commandT.CommandText);
                commandT.ExecuteNonQuery();

            }

            for (int j = 0; j < usersSelected.Items.Count; j++)  // Her tablo için x kullancısının countunu al
            {
                string tempUser = selectedUsers[j].ToString();

                ///CALL SIDE
                createTable = "CREATE TABLE DATATESTCALL" + tempUser + "TEST1 SELECT * from CALLTEST1_" + tempUser + ";";
                commandT.CommandText = createTable;
                logger.Items.Add(commandT.CommandText);
                commandT.ExecuteNonQuery();

                for (int curr = 0; curr < usersSelected.Items.Count; curr++)
                {
                    string currUser = selectedUsers[curr].ToString();

                    if (tempUser.Equals(currUser))
                    {
                        continue;
                    }
                    else
                    {
                        insertInto = "INSERT INTO DATATESTCALL" + tempUser + "TEST1 SELECT * from CALLTEST1_" + currUser + ";";
                        commandT.CommandText = insertInto;
                        logger.Items.Add(commandT.CommandText);
                        commandT.ExecuteNonQuery();


                    }
                }

                updatePID = "UPDATE DATATESTCALL" + tempUser + "TEST1 SET PID =\"NO\";";
                commandT.CommandText = updatePID;
                logger.Items.Add(commandT.CommandText);
                commandT.ExecuteNonQuery();

                updatePID = "UPDATE DATATESTCALL" + tempUser + "TEST1 SET PID =\"YES\" LIMIT " + CallS1 + ";";
                commandT.CommandText = updatePID;
                logger.Items.Add(commandT.CommandText);
                commandT.ExecuteNonQuery();

                ///CELLSIDE
                createTable = "CREATE TABLE DATATESTCELL" + tempUser + "TEST1 SELECT * from CELLTEST1_" + tempUser + ";";
                commandT.CommandText = createTable;
                logger.Items.Add(commandT.CommandText);
                commandT.ExecuteNonQuery();

                for (int curr = 0; curr < usersSelected.Items.Count; curr++)
                {
                    string currUser = selectedUsers[curr].ToString();

                    if (tempUser.Equals(currUser))
                    {
                        continue;
                    }
                    else
                    {
                        insertInto = "INSERT INTO DATATESTCELL" + tempUser + "TEST1 SELECT * from CELLTEST1_" + currUser + ";";
                        commandT.CommandText = insertInto;
                        logger.Items.Add(commandT.CommandText);
                        commandT.ExecuteNonQuery();


                    }
                }

                updatePID = "UPDATE DATATESTCELL" + tempUser + "TEST1 SET PID = \"NO\";";
                commandT.CommandText = updatePID;
                logger.Items.Add(commandT.CommandText);
                commandT.ExecuteNonQuery();

                updatePID = "UPDATE DATATESTCELL" + tempUser + "TEST1 SET PID = \"YES\" LIMIT " + CellS1 + ";";
                commandT.CommandText = updatePID;
                logger.Items.Add(commandT.CommandText);
                commandT.ExecuteNonQuery();

                //ACTIVITY SIDE
                createTable = "CREATE TABLE DATATESTACTIVITY" + tempUser + "TEST1 SELECT * from ACTIVITYTEST1_" + tempUser + ";";
                commandT.CommandText = createTable;
                logger.Items.Add(commandT.CommandText);
                commandT.ExecuteNonQuery();

                for (int curr = 0; curr < usersSelected.Items.Count; curr++)
                {
                    string currUser = selectedUsers[curr].ToString();

                    if (tempUser.Equals(currUser))
                    {
                        continue;
                    }
                    else
                    {
                        insertInto = "INSERT INTO DATATESTACTIVITY" + tempUser + "TEST1 SELECT * from ACTIVITYTEST1_" + currUser + ";";
                        commandT.CommandText = insertInto;
                        logger.Items.Add(commandT.CommandText);
                        commandT.ExecuteNonQuery();


                    }
                }

                updatePID = "UPDATE DATATESTACTIVITY" + tempUser + "TEST1 SET PID = \"NO\";";
                commandT.CommandText = updatePID;
                logger.Items.Add(commandT.CommandText);
                commandT.ExecuteNonQuery();

                updatePID = "UPDATE DATATESTACTIVITY" + tempUser + "TEST1 SET PID = \"YES\" LIMIT " + ActvS1 + ";";
                commandT.CommandText = updatePID;
                logger.Items.Add(commandT.CommandText);
                commandT.ExecuteNonQuery();


                ///DEVICE SIDE
                createTable = "CREATE TABLE DATATESTDEVICE" + tempUser + "TEST1 SELECT * from DEVICETEST1_" + tempUser + ";";
                commandT.CommandText = createTable;
                logger.Items.Add(commandT.CommandText);
                commandT.ExecuteNonQuery();

                for (int curr = 0; curr < usersSelected.Items.Count; curr++)
                {
                    string currUser = selectedUsers[curr].ToString();

                    if (tempUser.Equals(currUser))
                    {
                        continue;
                    }
                    else
                    {
                        insertInto = "INSERT INTO DATATESTDEVICE" + tempUser + "TEST1 SELECT * from DEVICETEST1_" + currUser + ";";
                        commandT.CommandText = insertInto;
                        logger.Items.Add(commandT.CommandText);
                        commandT.ExecuteNonQuery();


                    }
                }

                updatePID = "UPDATE DATATESTDEVICE" + tempUser + "TEST1 SET PID = \"NO\";";
                commandT.CommandText = updatePID;
                logger.Items.Add(commandT.CommandText);
                commandT.ExecuteNonQuery();

                updatePID = "UPDATE DATATESTDEVICE" + tempUser + "TEST1 SET PID = \"YES\" LIMIT " + DevcS1 + ";";
                commandT.CommandText = updatePID;
                logger.Items.Add(commandT.CommandText);
                commandT.ExecuteNonQuery();



            }


            /*
             * 
             * 
             * 
             * TEST 2 İÇİN YAPILAN BÖLÜM
             * 
             * 
             */

            for (int j = 0; j < usersSelected.Items.Count; j++)  // Her tablo için x kullancısının countunu al
            {
                string tempUser = selectedUsers[j].ToString();
                //CALL SIDE
                createTable = "CREATE TABLE DATATESTCALL" + tempUser + "TEST2 SELECT * from CALLTEST2_" + tempUser + ";";
                commandT.CommandText = createTable;
                logger.Items.Add(commandT.CommandText);
                commandT.ExecuteNonQuery();

                for (int curr = 0; curr < usersSelected.Items.Count; curr++)
                {
                    string currUser = selectedUsers[curr].ToString();

                    if (tempUser.Equals(currUser))
                    {
                        continue;
                    }
                    else
                    {
                        insertInto = "INSERT INTO DATATESTCALL" + tempUser + "TEST2 SELECT * from CALLTEST2_" + currUser + ";";
                        commandT.CommandText = insertInto;
                        logger.Items.Add(commandT.CommandText);
                        commandT.ExecuteNonQuery();


                    }
                }

                updatePID = "UPDATE DATATESTCALL" + tempUser + "TEST2 SET PID = \"NO\";";
                commandT.CommandText = updatePID;
                logger.Items.Add(commandT.CommandText);
                commandT.ExecuteNonQuery();

                updatePID = "UPDATE DATATESTCALL" + tempUser + "TEST2 SET PID = \"YES\" LIMIT " + CallS2 + ";";
                commandT.CommandText = updatePID;
                logger.Items.Add(commandT.CommandText);
                commandT.ExecuteNonQuery();

                ///CELLSIDE
                createTable = "CREATE TABLE DATATESTCELL" + tempUser + "TEST2 SELECT * from CELLTEST2_" + tempUser + ";";
                commandT.CommandText = createTable;
                logger.Items.Add(commandT.CommandText);
                commandT.ExecuteNonQuery();

                for (int curr = 0; curr < usersSelected.Items.Count; curr++)
                {
                    string currUser = selectedUsers[curr].ToString();

                    if (tempUser.Equals(currUser))
                    {
                        continue;
                    }
                    else
                    {
                        insertInto = "INSERT INTO DATATESTCELL" + tempUser + "TEST2 SELECT * from CELLTEST2_" + currUser + ";";
                        commandT.CommandText = insertInto;
                        logger.Items.Add(commandT.CommandText);
                        commandT.ExecuteNonQuery();


                    }
                }

                updatePID = "UPDATE DATATESTCELL" + tempUser + "TEST2 SET PID = \"NO\";";
                commandT.CommandText = updatePID;
                logger.Items.Add(commandT.CommandText);
                commandT.ExecuteNonQuery();

                updatePID = "UPDATE DATATESTCELL" + tempUser + "TEST2 SET PID = \"YES\" LIMIT " + CellS2 + ";";
                commandT.CommandText = updatePID;
                logger.Items.Add(commandT.CommandText);
                commandT.ExecuteNonQuery();

                //ACTIVITY SIDE
                createTable = "CREATE TABLE DATATESTACTIVITY" + tempUser + "TEST2 SELECT * from ACTIVITYTEST2_" + tempUser + ";";
                commandT.CommandText = createTable;
                logger.Items.Add(commandT.CommandText);
                commandT.ExecuteNonQuery();

                for (int curr = 0; curr < usersSelected.Items.Count; curr++)
                {
                    string currUser = selectedUsers[curr].ToString();

                    if (tempUser.Equals(currUser))
                    {
                        continue;
                    }
                    else
                    {
                        insertInto = "INSERT INTO DATATESTACTIVITY" + tempUser + "TEST2 SELECT * from ACTIVITYTEST2_" + currUser + ";";
                        commandT.CommandText = insertInto;
                        logger.Items.Add(commandT.CommandText);
                        commandT.ExecuteNonQuery();


                    }
                }

                updatePID = "UPDATE DATATESTACTIVITY" + tempUser + "TEST2 SET PID = \"NO\";";
                commandT.CommandText = updatePID;
                logger.Items.Add(commandT.CommandText);
                commandT.ExecuteNonQuery();

                updatePID = "UPDATE DATATESTACTIVITY" + tempUser + "TEST2 SET PID = \"YES\" LIMIT " + ActvS2 + ";";
                commandT.CommandText = updatePID;
                logger.Items.Add(commandT.CommandText);
                commandT.ExecuteNonQuery();


                ///DEVICE SIDE
                createTable = "CREATE TABLE DATATESTDEVICE" + tempUser + "TEST2 SELECT * from DEVICETEST2_" + tempUser + ";";
                commandT.CommandText = createTable;
                logger.Items.Add(commandT.CommandText);
                commandT.ExecuteNonQuery();

                for (int curr = 0; curr < usersSelected.Items.Count; curr++)
                {
                    string currUser = selectedUsers[curr].ToString();

                    if (tempUser.Equals(currUser))
                    {
                        continue;
                    }
                    else
                    {
                        insertInto = "INSERT INTO DATATESTDEVICE" + tempUser + "TEST2 SELECT * from DEVICETEST2_" + currUser + ";";
                        commandT.CommandText = insertInto;
                        logger.Items.Add(commandT.CommandText);
                        commandT.ExecuteNonQuery();


                    }
                }

                updatePID = "UPDATE DATATESTDEVICE" + tempUser + "TEST2 SET PID = \"NO\";";
                commandT.CommandText = updatePID;
                logger.Items.Add(commandT.CommandText);
                commandT.ExecuteNonQuery();

                updatePID = "UPDATE DATATESTDEVICE" + tempUser + "TEST2 SET PID = \"YES\" LIMIT " + DevcS2 + ";";
                commandT.CommandText = updatePID;
                logger.Items.Add(commandT.CommandText);
                commandT.ExecuteNonQuery();

            }
        }

        private void logtoTxt_Click(object sender, EventArgs e)
        {
            if (logger.Items.Count > 0)
            {

                using (TextWriter TW = new StreamWriter("list.txt"))
                {
                    foreach (string itemText in logger.Items)
                    {
                        TW.WriteLine(itemText);
                    }
                }
            }

            Process.Start("list.txt");
        }

    }
}




 