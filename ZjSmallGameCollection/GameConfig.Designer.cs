namespace ZjSmallGameCollection
{
    partial class GameConfig
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
            if(disposing && (components != null))
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
            this.tab = new System.Windows.Forms.TabControl();
            this.tabPage_GameRiver = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.player2_name = new System.Windows.Forms.TextBox();
            this.player2_ip = new System.Windows.Forms.TextBox();
            this.player2_local = new System.Windows.Forms.RadioButton();
            this.player2_port = new System.Windows.Forms.TextBox();
            this.player2_auto = new System.Windows.Forms.RadioButton();
            this.player2_port_label = new System.Windows.Forms.Label();
            this.player2_name_label = new System.Windows.Forms.Label();
            this.player2_server = new System.Windows.Forms.CheckBox();
            this.player2_AIdt_label = new System.Windows.Forms.Label();
            this.player2_network = new System.Windows.Forms.RadioButton();
            this.player2_AIdt = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.player_first = new System.Windows.Forms.ComboBox();
            this.player1_ip = new System.Windows.Forms.TextBox();
            this.player1_port = new System.Windows.Forms.TextBox();
            this.player1_port_label = new System.Windows.Forms.Label();
            this.player1_server = new System.Windows.Forms.CheckBox();
            this.player1_network = new System.Windows.Forms.RadioButton();
            this.player1_AIdt = new System.Windows.Forms.ComboBox();
            this.player1_AIdt_label = new System.Windows.Forms.Label();
            this.player1_name = new System.Windows.Forms.TextBox();
            this.player1_name_label = new System.Windows.Forms.Label();
            this.player1_auto = new System.Windows.Forms.RadioButton();
            this.player1_local = new System.Windows.Forms.RadioButton();
            this.tabPage_GameBlackWhite = new System.Windows.Forms.TabPage();
            this.okbtn = new System.Windows.Forms.Button();
            this.usebtn = new System.Windows.Forms.Button();
            this.cancelbtn = new System.Windows.Forms.Button();
            this.tab.SuspendLayout();
            this.tabPage_GameRiver.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab
            // 
            this.tab.Controls.Add(this.tabPage_GameRiver);
            this.tab.Controls.Add(this.tabPage_GameBlackWhite);
            this.tab.Location = new System.Drawing.Point(13, 13);
            this.tab.Name = "tab";
            this.tab.SelectedIndex = 0;
            this.tab.Size = new System.Drawing.Size(259, 285);
            this.tab.TabIndex = 0;
            // 
            // tabPage_GameRiver
            // 
            this.tabPage_GameRiver.Controls.Add(this.groupBox2);
            this.tabPage_GameRiver.Controls.Add(this.groupBox1);
            this.tabPage_GameRiver.Location = new System.Drawing.Point(4, 22);
            this.tabPage_GameRiver.Name = "tabPage_GameRiver";
            this.tabPage_GameRiver.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_GameRiver.Size = new System.Drawing.Size(251, 259);
            this.tabPage_GameRiver.TabIndex = 0;
            this.tabPage_GameRiver.Text = "过河棋";
            this.tabPage_GameRiver.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.player2_name);
            this.groupBox2.Controls.Add(this.player2_ip);
            this.groupBox2.Controls.Add(this.player2_local);
            this.groupBox2.Controls.Add(this.player2_port);
            this.groupBox2.Controls.Add(this.player2_auto);
            this.groupBox2.Controls.Add(this.player2_port_label);
            this.groupBox2.Controls.Add(this.player2_name_label);
            this.groupBox2.Controls.Add(this.player2_server);
            this.groupBox2.Controls.Add(this.player2_AIdt_label);
            this.groupBox2.Controls.Add(this.player2_network);
            this.groupBox2.Controls.Add(this.player2_AIdt);
            this.groupBox2.Location = new System.Drawing.Point(7, 134);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(238, 119);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "执白玩家";
            // 
            // player2_name
            // 
            this.player2_name.Enabled = false;
            this.player2_name.Location = new System.Drawing.Point(127, 20);
            this.player2_name.Name = "player2_name";
            this.player2_name.Size = new System.Drawing.Size(105, 21);
            this.player2_name.TabIndex = 15;
            // 
            // player2_ip
            // 
            this.player2_ip.Enabled = false;
            this.player2_ip.Location = new System.Drawing.Point(21, 94);
            this.player2_ip.Name = "player2_ip";
            this.player2_ip.Size = new System.Drawing.Size(117, 21);
            this.player2_ip.TabIndex = 22;
            this.player2_ip.Text = "255.255.255.255";
            // 
            // player2_local
            // 
            this.player2_local.AutoSize = true;
            this.player2_local.Location = new System.Drawing.Point(7, 25);
            this.player2_local.Name = "player2_local";
            this.player2_local.Size = new System.Drawing.Size(71, 16);
            this.player2_local.TabIndex = 12;
            this.player2_local.TabStop = true;
            this.player2_local.Text = "本机玩家";
            this.player2_local.UseVisualStyleBackColor = true;
            this.player2_local.CheckedChanged += new System.EventHandler(this.Radiobtn2_CheckedChanged);
            // 
            // player2_port
            // 
            this.player2_port.Enabled = false;
            this.player2_port.Location = new System.Drawing.Point(185, 69);
            this.player2_port.Name = "player2_port";
            this.player2_port.Size = new System.Drawing.Size(47, 21);
            this.player2_port.TabIndex = 21;
            // 
            // player2_auto
            // 
            this.player2_auto.AutoSize = true;
            this.player2_auto.Location = new System.Drawing.Point(7, 48);
            this.player2_auto.Name = "player2_auto";
            this.player2_auto.Size = new System.Drawing.Size(71, 16);
            this.player2_auto.TabIndex = 13;
            this.player2_auto.TabStop = true;
            this.player2_auto.Text = "电脑玩家";
            this.player2_auto.UseVisualStyleBackColor = true;
            this.player2_auto.CheckedChanged += new System.EventHandler(this.Radiobtn2_CheckedChanged);
            // 
            // player2_port_label
            // 
            this.player2_port_label.AutoSize = true;
            this.player2_port_label.Enabled = false;
            this.player2_port_label.Location = new System.Drawing.Point(144, 76);
            this.player2_port_label.Name = "player2_port_label";
            this.player2_port_label.Size = new System.Drawing.Size(35, 12);
            this.player2_port_label.TabIndex = 20;
            this.player2_port_label.Text = "端口:";
            // 
            // player2_name_label
            // 
            this.player2_name_label.AutoSize = true;
            this.player2_name_label.Enabled = false;
            this.player2_name_label.Location = new System.Drawing.Point(86, 27);
            this.player2_name_label.Name = "player2_name_label";
            this.player2_name_label.Size = new System.Drawing.Size(35, 12);
            this.player2_name_label.TabIndex = 14;
            this.player2_name_label.Text = "昵称:";
            // 
            // player2_server
            // 
            this.player2_server.AutoSize = true;
            this.player2_server.Enabled = false;
            this.player2_server.Location = new System.Drawing.Point(78, 75);
            this.player2_server.Name = "player2_server";
            this.player2_server.Size = new System.Drawing.Size(60, 16);
            this.player2_server.TabIndex = 19;
            this.player2_server.Text = "服务器";
            this.player2_server.UseVisualStyleBackColor = true;
            this.player2_server.CheckStateChanged += new System.EventHandler(this.server_CheckStateChanged);
            // 
            // player2_AIdt_label
            // 
            this.player2_AIdt_label.AutoSize = true;
            this.player2_AIdt_label.Enabled = false;
            this.player2_AIdt_label.Location = new System.Drawing.Point(86, 50);
            this.player2_AIdt_label.Name = "player2_AIdt_label";
            this.player2_AIdt_label.Size = new System.Drawing.Size(95, 12);
            this.player2_AIdt_label.TabIndex = 16;
            this.player2_AIdt_label.Text = "响应延迟(毫秒):";
            // 
            // player2_network
            // 
            this.player2_network.AutoSize = true;
            this.player2_network.Location = new System.Drawing.Point(7, 74);
            this.player2_network.Name = "player2_network";
            this.player2_network.Size = new System.Drawing.Size(47, 16);
            this.player2_network.TabIndex = 18;
            this.player2_network.TabStop = true;
            this.player2_network.Text = "网络";
            this.player2_network.UseVisualStyleBackColor = true;
            this.player2_network.CheckedChanged += new System.EventHandler(this.Radiobtn2_CheckedChanged);
            // 
            // player2_AIdt
            // 
            this.player2_AIdt.Enabled = false;
            this.player2_AIdt.FormattingEnabled = true;
            this.player2_AIdt.Items.AddRange(new object[] {
            "500",
            "1000",
            "2000",
            "3000"});
            this.player2_AIdt.Location = new System.Drawing.Point(187, 44);
            this.player2_AIdt.MaxLength = 4;
            this.player2_AIdt.Name = "player2_AIdt";
            this.player2_AIdt.Size = new System.Drawing.Size(45, 20);
            this.player2_AIdt.TabIndex = 17;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.player_first);
            this.groupBox1.Controls.Add(this.player1_ip);
            this.groupBox1.Controls.Add(this.player1_port);
            this.groupBox1.Controls.Add(this.player1_port_label);
            this.groupBox1.Controls.Add(this.player1_server);
            this.groupBox1.Controls.Add(this.player1_network);
            this.groupBox1.Controls.Add(this.player1_AIdt);
            this.groupBox1.Controls.Add(this.player1_AIdt_label);
            this.groupBox1.Controls.Add(this.player1_name);
            this.groupBox1.Controls.Add(this.player1_name_label);
            this.groupBox1.Controls.Add(this.player1_auto);
            this.groupBox1.Controls.Add(this.player1_local);
            this.groupBox1.Location = new System.Drawing.Point(7, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(238, 120);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "执黑玩家";
            // 
            // player_first
            // 
            this.player_first.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.player_first.FormattingEnabled = true;
            this.player_first.Items.AddRange(new object[] {
            "执黑先手",
            "执白先手",
            "随机"});
            this.player_first.Location = new System.Drawing.Point(146, 93);
            this.player_first.Name = "player_first";
            this.player_first.Size = new System.Drawing.Size(86, 20);
            this.player_first.TabIndex = 11;
            // 
            // player1_ip
            // 
            this.player1_ip.Enabled = false;
            this.player1_ip.Location = new System.Drawing.Point(21, 93);
            this.player1_ip.Name = "player1_ip";
            this.player1_ip.Size = new System.Drawing.Size(117, 21);
            this.player1_ip.TabIndex = 10;
            this.player1_ip.Text = "255.255.255.255";
            // 
            // player1_port
            // 
            this.player1_port.Enabled = false;
            this.player1_port.Location = new System.Drawing.Point(185, 66);
            this.player1_port.Name = "player1_port";
            this.player1_port.Size = new System.Drawing.Size(47, 21);
            this.player1_port.TabIndex = 9;
            // 
            // player1_port_label
            // 
            this.player1_port_label.AutoSize = true;
            this.player1_port_label.Enabled = false;
            this.player1_port_label.Location = new System.Drawing.Point(144, 72);
            this.player1_port_label.Name = "player1_port_label";
            this.player1_port_label.Size = new System.Drawing.Size(35, 12);
            this.player1_port_label.TabIndex = 8;
            this.player1_port_label.Text = "端口:";
            // 
            // player1_server
            // 
            this.player1_server.AutoSize = true;
            this.player1_server.Enabled = false;
            this.player1_server.Location = new System.Drawing.Point(78, 71);
            this.player1_server.Name = "player1_server";
            this.player1_server.Size = new System.Drawing.Size(60, 16);
            this.player1_server.TabIndex = 7;
            this.player1_server.Text = "服务器";
            this.player1_server.UseVisualStyleBackColor = true;
            this.player1_server.CheckStateChanged += new System.EventHandler(this.server_CheckStateChanged);
            // 
            // player1_network
            // 
            this.player1_network.AutoSize = true;
            this.player1_network.Location = new System.Drawing.Point(7, 70);
            this.player1_network.Name = "player1_network";
            this.player1_network.Size = new System.Drawing.Size(47, 16);
            this.player1_network.TabIndex = 6;
            this.player1_network.TabStop = true;
            this.player1_network.Text = "网络";
            this.player1_network.UseVisualStyleBackColor = true;
            this.player1_network.CheckedChanged += new System.EventHandler(this.Radiobtn1_CheckedChanged);
            // 
            // player1_AIdt
            // 
            this.player1_AIdt.Enabled = false;
            this.player1_AIdt.FormattingEnabled = true;
            this.player1_AIdt.Items.AddRange(new object[] {
            "500",
            "1000",
            "2000",
            "3000"});
            this.player1_AIdt.Location = new System.Drawing.Point(187, 40);
            this.player1_AIdt.MaxLength = 4;
            this.player1_AIdt.Name = "player1_AIdt";
            this.player1_AIdt.Size = new System.Drawing.Size(45, 20);
            this.player1_AIdt.TabIndex = 5;
            // 
            // player1_AIdt_label
            // 
            this.player1_AIdt_label.AutoSize = true;
            this.player1_AIdt_label.Enabled = false;
            this.player1_AIdt_label.Location = new System.Drawing.Point(86, 46);
            this.player1_AIdt_label.Name = "player1_AIdt_label";
            this.player1_AIdt_label.Size = new System.Drawing.Size(95, 12);
            this.player1_AIdt_label.TabIndex = 4;
            this.player1_AIdt_label.Text = "响应延迟(毫秒):";
            // 
            // player1_name
            // 
            this.player1_name.Enabled = false;
            this.player1_name.Location = new System.Drawing.Point(127, 16);
            this.player1_name.Name = "player1_name";
            this.player1_name.Size = new System.Drawing.Size(105, 21);
            this.player1_name.TabIndex = 3;
            // 
            // player1_name_label
            // 
            this.player1_name_label.AutoSize = true;
            this.player1_name_label.Enabled = false;
            this.player1_name_label.Location = new System.Drawing.Point(86, 23);
            this.player1_name_label.Name = "player1_name_label";
            this.player1_name_label.Size = new System.Drawing.Size(35, 12);
            this.player1_name_label.TabIndex = 2;
            this.player1_name_label.Text = "昵称:";
            // 
            // player1_auto
            // 
            this.player1_auto.AutoSize = true;
            this.player1_auto.Location = new System.Drawing.Point(7, 44);
            this.player1_auto.Name = "player1_auto";
            this.player1_auto.Size = new System.Drawing.Size(71, 16);
            this.player1_auto.TabIndex = 1;
            this.player1_auto.TabStop = true;
            this.player1_auto.Text = "电脑玩家";
            this.player1_auto.UseVisualStyleBackColor = true;
            this.player1_auto.CheckedChanged += new System.EventHandler(this.Radiobtn1_CheckedChanged);
            // 
            // player1_local
            // 
            this.player1_local.AutoSize = true;
            this.player1_local.Location = new System.Drawing.Point(7, 21);
            this.player1_local.Name = "player1_local";
            this.player1_local.Size = new System.Drawing.Size(71, 16);
            this.player1_local.TabIndex = 0;
            this.player1_local.Text = "本机玩家";
            this.player1_local.UseVisualStyleBackColor = true;
            this.player1_local.CheckedChanged += new System.EventHandler(this.Radiobtn1_CheckedChanged);
            // 
            // tabPage_GameBlackWhite
            // 
            this.tabPage_GameBlackWhite.Location = new System.Drawing.Point(4, 22);
            this.tabPage_GameBlackWhite.Name = "tabPage_GameBlackWhite";
            this.tabPage_GameBlackWhite.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_GameBlackWhite.Size = new System.Drawing.Size(251, 259);
            this.tabPage_GameBlackWhite.TabIndex = 1;
            this.tabPage_GameBlackWhite.Text = "黑白棋";
            this.tabPage_GameBlackWhite.UseVisualStyleBackColor = true;
            // 
            // okbtn
            // 
            this.okbtn.Location = new System.Drawing.Point(12, 304);
            this.okbtn.Name = "okbtn";
            this.okbtn.Size = new System.Drawing.Size(75, 23);
            this.okbtn.TabIndex = 1;
            this.okbtn.Text = "确定(&O)";
            this.okbtn.UseVisualStyleBackColor = true;
            this.okbtn.Click += new System.EventHandler(this.okbtn_Click);
            // 
            // usebtn
            // 
            this.usebtn.Location = new System.Drawing.Point(102, 304);
            this.usebtn.Name = "usebtn";
            this.usebtn.Size = new System.Drawing.Size(75, 23);
            this.usebtn.TabIndex = 2;
            this.usebtn.Text = "应用(&U)";
            this.usebtn.UseVisualStyleBackColor = true;
            this.usebtn.Click += new System.EventHandler(this.usebtn_Click);
            // 
            // cancelbtn
            // 
            this.cancelbtn.Location = new System.Drawing.Point(193, 304);
            this.cancelbtn.Name = "cancelbtn";
            this.cancelbtn.Size = new System.Drawing.Size(75, 23);
            this.cancelbtn.TabIndex = 3;
            this.cancelbtn.Text = "取消(&C)";
            this.cancelbtn.UseVisualStyleBackColor = true;
            this.cancelbtn.Click += new System.EventHandler(this.cancelbtn_Click);
            // 
            // GameConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 339);
            this.Controls.Add(this.cancelbtn);
            this.Controls.Add(this.usebtn);
            this.Controls.Add(this.okbtn);
            this.Controls.Add(this.tab);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GameConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "游戏设置";
            this.Load += new System.EventHandler(this.GameConfig_Load);
            this.tab.ResumeLayout(false);
            this.tabPage_GameRiver.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tab;
        private System.Windows.Forms.TabPage tabPage_GameRiver;
        private System.Windows.Forms.TabPage tabPage_GameBlackWhite;
        private System.Windows.Forms.Button okbtn;
        private System.Windows.Forms.Button usebtn;
        private System.Windows.Forms.Button cancelbtn;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox player1_AIdt;
        private System.Windows.Forms.Label player1_AIdt_label;
        private System.Windows.Forms.TextBox player1_name;
        private System.Windows.Forms.Label player1_name_label;
        private System.Windows.Forms.RadioButton player1_auto;
        private System.Windows.Forms.RadioButton player1_local;
        private System.Windows.Forms.RadioButton player1_network;
        private System.Windows.Forms.ComboBox player_first;
        private System.Windows.Forms.TextBox player1_ip;
        private System.Windows.Forms.TextBox player1_port;
        private System.Windows.Forms.Label player1_port_label;
        private System.Windows.Forms.CheckBox player1_server;
        private System.Windows.Forms.TextBox player2_name;
        private System.Windows.Forms.TextBox player2_ip;
        private System.Windows.Forms.RadioButton player2_local;
        private System.Windows.Forms.TextBox player2_port;
        private System.Windows.Forms.RadioButton player2_auto;
        private System.Windows.Forms.Label player2_port_label;
        private System.Windows.Forms.Label player2_name_label;
        private System.Windows.Forms.CheckBox player2_server;
        private System.Windows.Forms.Label player2_AIdt_label;
        private System.Windows.Forms.RadioButton player2_network;
        private System.Windows.Forms.ComboBox player2_AIdt;
    }
}