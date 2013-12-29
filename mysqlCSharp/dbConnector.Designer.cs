namespace mysqlCSharp
{
    partial class dbConnector
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.submit = new System.Windows.Forms.Button();
            this.srvName = new System.Windows.Forms.TextBox();
            this.usrName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pass = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dbName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.portBox = new System.Windows.Forms.TextBox();
            this.getTbls = new System.Windows.Forms.Button();
            this.listTable = new System.Windows.Forms.ListBox();
            this.getData = new System.Windows.Forms.Button();
            this.personOidList = new System.Windows.Forms.ListBox();
            this.selectUsers = new System.Windows.Forms.Button();
            this.usersSelected = new System.Windows.Forms.ListBox();
            this.testButton = new System.Windows.Forms.Button();
            this.logger = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.ctID1 = new System.Windows.Forms.Button();
            this.fillCTID = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // submit
            // 
            this.submit.Location = new System.Drawing.Point(83, 162);
            this.submit.Name = "submit";
            this.submit.Size = new System.Drawing.Size(70, 23);
            this.submit.TabIndex = 1;
            this.submit.Text = "Submit";
            this.submit.UseVisualStyleBackColor = true;
            this.submit.Click += new System.EventHandler(this.submit_Click);
            // 
            // srvName
            // 
            this.srvName.Location = new System.Drawing.Point(83, 16);
            this.srvName.Name = "srvName";
            this.srvName.Size = new System.Drawing.Size(95, 20);
            this.srvName.TabIndex = 2;
            this.srvName.Text = "localhost";
            // 
            // usrName
            // 
            this.usrName.Location = new System.Drawing.Point(83, 42);
            this.usrName.Name = "usrName";
            this.usrName.Size = new System.Drawing.Size(95, 20);
            this.usrName.TabIndex = 4;
            this.usrName.Text = "root";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "user";
            // 
            // pass
            // 
            this.pass.Location = new System.Drawing.Point(83, 68);
            this.pass.Name = "pass";
            this.pass.Size = new System.Drawing.Size(95, 20);
            this.pass.TabIndex = 6;
            this.pass.Text = "root";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "pass";
            // 
            // dbName
            // 
            this.dbName.Location = new System.Drawing.Point(83, 94);
            this.dbName.Name = "dbName";
            this.dbName.Size = new System.Drawing.Size(95, 20);
            this.dbName.TabIndex = 8;
            this.dbName.Text = "mit";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(25, 97);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "dbName";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 124);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(25, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "port";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // portBox
            // 
            this.portBox.Location = new System.Drawing.Point(83, 124);
            this.portBox.Name = "portBox";
            this.portBox.Size = new System.Drawing.Size(95, 20);
            this.portBox.TabIndex = 10;
            this.portBox.Text = "3306";
            // 
            // getTbls
            // 
            this.getTbls.Enabled = false;
            this.getTbls.Location = new System.Drawing.Point(246, 12);
            this.getTbls.Name = "getTbls";
            this.getTbls.Size = new System.Drawing.Size(75, 23);
            this.getTbls.TabIndex = 11;
            this.getTbls.Text = "Get Tables";
            this.getTbls.UseVisualStyleBackColor = true;
            this.getTbls.Click += new System.EventHandler(this.getTbls_Click);
            // 
            // listTable
            // 
            this.listTable.FormattingEnabled = true;
            this.listTable.Location = new System.Drawing.Point(227, 41);
            this.listTable.Name = "listTable";
            this.listTable.Size = new System.Drawing.Size(120, 212);
            this.listTable.TabIndex = 12;
            // 
            // getData
            // 
            this.getData.Enabled = false;
            this.getData.Location = new System.Drawing.Point(371, 12);
            this.getData.Name = "getData";
            this.getData.Size = new System.Drawing.Size(75, 23);
            this.getData.TabIndex = 13;
            this.getData.Text = "Get Data";
            this.getData.UseVisualStyleBackColor = true;
            this.getData.Click += new System.EventHandler(this.getData_Click);
            // 
            // personOidList
            // 
            this.personOidList.FormattingEnabled = true;
            this.personOidList.Location = new System.Drawing.Point(353, 41);
            this.personOidList.MultiColumn = true;
            this.personOidList.Name = "personOidList";
            this.personOidList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.personOidList.Size = new System.Drawing.Size(120, 212);
            this.personOidList.TabIndex = 14;
            // 
            // selectUsers
            // 
            this.selectUsers.Enabled = false;
            this.selectUsers.Location = new System.Drawing.Point(497, 12);
            this.selectUsers.Name = "selectUsers";
            this.selectUsers.Size = new System.Drawing.Size(75, 23);
            this.selectUsers.TabIndex = 15;
            this.selectUsers.Text = "Select Users";
            this.selectUsers.UseVisualStyleBackColor = true;
            this.selectUsers.Click += new System.EventHandler(this.button1_Click);
            // 
            // usersSelected
            // 
            this.usersSelected.Enabled = false;
            this.usersSelected.FormattingEnabled = true;
            this.usersSelected.Location = new System.Drawing.Point(479, 41);
            this.usersSelected.Name = "usersSelected";
            this.usersSelected.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.usersSelected.Size = new System.Drawing.Size(120, 212);
            this.usersSelected.TabIndex = 16;
            // 
            // testButton
            // 
            this.testButton.Enabled = false;
            this.testButton.Location = new System.Drawing.Point(626, 229);
            this.testButton.Name = "testButton";
            this.testButton.Size = new System.Drawing.Size(120, 23);
            this.testButton.TabIndex = 17;
            this.testButton.Text = "Create tbl User";
            this.testButton.UseVisualStyleBackColor = true;
            this.testButton.Click += new System.EventHandler(this.testButton_Click);
            // 
            // logger
            // 
            this.logger.FormattingEnabled = true;
            this.logger.Location = new System.Drawing.Point(12, 307);
            this.logger.Name = "logger";
            this.logger.Size = new System.Drawing.Size(587, 108);
            this.logger.TabIndex = 18;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.usrName);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.submit);
            this.panel1.Controls.Add(this.srvName);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.pass);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.dbName);
            this.panel1.Controls.Add(this.portBox);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Cursor = System.Windows.Forms.Cursors.Default;
            this.panel1.Location = new System.Drawing.Point(10, 41);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(201, 212);
            this.panel1.TabIndex = 19;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(244, 290);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "SQL Query Logs";
            // 
            // ctID1
            // 
            this.ctID1.Location = new System.Drawing.Point(626, 41);
            this.ctID1.Margin = new System.Windows.Forms.Padding(2);
            this.ctID1.Name = "ctID1";
            this.ctID1.Size = new System.Drawing.Size(120, 19);
            this.ctID1.TabIndex = 21;
            this.ctID1.Text = "Divide 16";
            this.ctID1.UseVisualStyleBackColor = true;
            this.ctID1.Click += new System.EventHandler(this.ctID1_Click);
            // 
            // fillCTID
            // 
            this.fillCTID.Location = new System.Drawing.Point(626, 65);
            this.fillCTID.Name = "fillCTID";
            this.fillCTID.Size = new System.Drawing.Size(120, 23);
            this.fillCTID.TabIndex = 22;
            this.fillCTID.Text = "Fill CTID";
            this.fillCTID.UseVisualStyleBackColor = true;
            this.fillCTID.Click += new System.EventHandler(this.fillCTID_Click);
            // 
            // dbConnector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGray;
            this.ClientSize = new System.Drawing.Size(787, 434);
            this.Controls.Add(this.fillCTID);
            this.Controls.Add(this.ctID1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.logger);
            this.Controls.Add(this.testButton);
            this.Controls.Add(this.usersSelected);
            this.Controls.Add(this.selectUsers);
            this.Controls.Add(this.personOidList);
            this.Controls.Add(this.getData);
            this.Controls.Add(this.listTable);
            this.Controls.Add(this.getTbls);
            this.ForeColor = System.Drawing.Color.Black;
            this.Name = "dbConnector";
            this.Text = "Fill CTID";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button submit;
        private System.Windows.Forms.TextBox srvName;
        private System.Windows.Forms.TextBox usrName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox pass;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox dbName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox portBox;
        private System.Windows.Forms.Button getTbls;
        private System.Windows.Forms.ListBox listTable;
        private System.Windows.Forms.Button getData;
        private System.Windows.Forms.ListBox personOidList;
        private System.Windows.Forms.Button selectUsers;
        private System.Windows.Forms.ListBox usersSelected;
        private System.Windows.Forms.Button testButton;
        private System.Windows.Forms.ListBox logger;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button ctID1;
        private System.Windows.Forms.Button fillCTID;
    }
}

