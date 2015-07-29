namespace ZjSmallGameCollection
{
    partial class GameRiver
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
            this.mainMain = new System.Windows.Forms.MenuStrip();
            this.gameMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startNewItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.messageList = new System.Windows.Forms.ListBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statueBar = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.mainMain.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMain
            // 
            this.mainMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gameMenuItem});
            this.mainMain.Location = new System.Drawing.Point(0, 0);
            this.mainMain.Name = "mainMain";
            this.mainMain.Size = new System.Drawing.Size(494, 25);
            this.mainMain.TabIndex = 0;
            this.mainMain.Text = "主菜单";
            // 
            // gameMenuItem
            // 
            this.gameMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startNewItem,
            this.exitItem});
            this.gameMenuItem.Name = "gameMenuItem";
            this.gameMenuItem.Size = new System.Drawing.Size(61, 21);
            this.gameMenuItem.Text = "游戏(&G)";
            // 
            // startNewItem
            // 
            this.startNewItem.Name = "startNewItem";
            this.startNewItem.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.startNewItem.Size = new System.Drawing.Size(152, 22);
            this.startNewItem.Text = "开局";
            this.startNewItem.Click += new System.EventHandler(this.gameMenuItem_ChildClick);
            // 
            // exitItem
            // 
            this.exitItem.Name = "exitItem";
            this.exitItem.Size = new System.Drawing.Size(152, 22);
            this.exitItem.Text = "退出";
            this.exitItem.Click += new System.EventHandler(this.gameMenuItem_ChildClick);
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(147, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(180, 180);
            this.panel1.TabIndex = 1;
            // 
            // messageList
            // 
            this.messageList.FormattingEnabled = true;
            this.messageList.ItemHeight = 12;
            this.messageList.Location = new System.Drawing.Point(12, 28);
            this.messageList.Name = "messageList";
            this.messageList.Size = new System.Drawing.Size(129, 184);
            this.messageList.TabIndex = 2;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statueBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 221);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(494, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statueBar
            // 
            this.statueBar.Name = "statueBar";
            this.statueBar.Size = new System.Drawing.Size(93, 17);
            this.statueBar.Text = "请按F2开始游戏";
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(333, 30);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(149, 86);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "对方信息";
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(333, 122);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(149, 86);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "我方信息";
            // 
            // GameRiver
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 243);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.messageList);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.mainMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MainMenuStrip = this.mainMain;
            this.Name = "GameRiver";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "GameRiver";
            this.Load += new System.EventHandler(this.GameRiver_Load);
            this.mainMain.ResumeLayout(false);
            this.mainMain.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMain;
        private System.Windows.Forms.ToolStripMenuItem gameMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startNewItem;
        private System.Windows.Forms.ToolStripMenuItem exitItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListBox messageList;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statueBar;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}