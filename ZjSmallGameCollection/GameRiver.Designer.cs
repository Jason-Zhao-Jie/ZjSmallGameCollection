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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameRiver));
            this.mainMain = new System.Windows.Forms.MenuStrip();
            this.gameMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startNewItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitItem = new System.Windows.Forms.ToolStripMenuItem();
            this.messageList = new System.Windows.Forms.ListBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statueBar = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.gameBack = new System.Windows.Forms.PictureBox();
            this.chess1 = new System.Windows.Forms.PictureBox();
            this.chess2 = new System.Windows.Forms.PictureBox();
            this.chess3 = new System.Windows.Forms.PictureBox();
            this.chess4 = new System.Windows.Forms.PictureBox();
            this.chess8 = new System.Windows.Forms.PictureBox();
            this.chess7 = new System.Windows.Forms.PictureBox();
            this.chess6 = new System.Windows.Forms.PictureBox();
            this.chess5 = new System.Windows.Forms.PictureBox();
            this.mainMain.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gameBack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chess1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chess2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chess3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chess4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chess8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chess7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chess6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chess5)).BeginInit();
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
            this.startNewItem.Size = new System.Drawing.Size(121, 22);
            this.startNewItem.Text = "开局";
            this.startNewItem.Click += new System.EventHandler(this.gameMenuItem_ChildClick);
            // 
            // exitItem
            // 
            this.exitItem.Name = "exitItem";
            this.exitItem.Size = new System.Drawing.Size(121, 22);
            this.exitItem.Text = "退出";
            this.exitItem.Click += new System.EventHandler(this.gameMenuItem_ChildClick);
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
            // gameBack
            // 
            this.gameBack.BackColor = System.Drawing.Color.White;
            this.gameBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.gameBack.Image = ((System.Drawing.Image)(resources.GetObject("gameBack.Image")));
            this.gameBack.Location = new System.Drawing.Point(147, 30);
            this.gameBack.Name = "gameBack";
            this.gameBack.Size = new System.Drawing.Size(180, 180);
            this.gameBack.TabIndex = 6;
            this.gameBack.TabStop = false;
            // 
            // chess1
            // 
            this.chess1.BackColor = System.Drawing.Color.White;
            this.chess1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chess1.BackgroundImage")));
            this.chess1.Image = ((System.Drawing.Image)(resources.GetObject("chess1.Image")));
            this.chess1.Location = new System.Drawing.Point(151, 30);
            this.chess1.Name = "chess1";
            this.chess1.Size = new System.Drawing.Size(48, 48);
            this.chess1.TabIndex = 7;
            this.chess1.TabStop = false;
            // 
            // chess2
            // 
            this.chess2.BackColor = System.Drawing.Color.White;
            this.chess2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chess2.BackgroundImage")));
            this.chess2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.chess2.Image = ((System.Drawing.Image)(resources.GetObject("chess2.Image")));
            this.chess2.Location = new System.Drawing.Point(213, 30);
            this.chess2.Name = "chess2";
            this.chess2.Size = new System.Drawing.Size(48, 48);
            this.chess2.TabIndex = 8;
            this.chess2.TabStop = false;
            // 
            // chess3
            // 
            this.chess3.BackColor = System.Drawing.Color.White;
            this.chess3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chess3.BackgroundImage")));
            this.chess3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.chess3.Location = new System.Drawing.Point(276, 30);
            this.chess3.Name = "chess3";
            this.chess3.Size = new System.Drawing.Size(48, 48);
            this.chess3.TabIndex = 9;
            this.chess3.TabStop = false;
            // 
            // chess4
            // 
            this.chess4.BackColor = System.Drawing.Color.White;
            this.chess4.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chess4.BackgroundImage")));
            this.chess4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.chess4.Location = new System.Drawing.Point(278, 95);
            this.chess4.Name = "chess4";
            this.chess4.Size = new System.Drawing.Size(48, 48);
            this.chess4.TabIndex = 10;
            this.chess4.TabStop = false;
            // 
            // chess8
            // 
            this.chess8.BackColor = System.Drawing.Color.White;
            this.chess8.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chess8.BackgroundImage")));
            this.chess8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.chess8.Location = new System.Drawing.Point(150, 95);
            this.chess8.Name = "chess8";
            this.chess8.Size = new System.Drawing.Size(48, 48);
            this.chess8.TabIndex = 11;
            this.chess8.TabStop = false;
            // 
            // chess7
            // 
            this.chess7.BackColor = System.Drawing.Color.White;
            this.chess7.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chess7.BackgroundImage")));
            this.chess7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.chess7.Location = new System.Drawing.Point(151, 166);
            this.chess7.Name = "chess7";
            this.chess7.Size = new System.Drawing.Size(48, 48);
            this.chess7.TabIndex = 12;
            this.chess7.TabStop = false;
            // 
            // chess6
            // 
            this.chess6.BackColor = System.Drawing.Color.White;
            this.chess6.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chess6.BackgroundImage")));
            this.chess6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.chess6.Location = new System.Drawing.Point(215, 166);
            this.chess6.Name = "chess6";
            this.chess6.Size = new System.Drawing.Size(48, 48);
            this.chess6.TabIndex = 13;
            this.chess6.TabStop = false;
            // 
            // chess5
            // 
            this.chess5.BackColor = System.Drawing.Color.White;
            this.chess5.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chess5.BackgroundImage")));
            this.chess5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.chess5.Location = new System.Drawing.Point(276, 166);
            this.chess5.Name = "chess5";
            this.chess5.Size = new System.Drawing.Size(48, 48);
            this.chess5.TabIndex = 14;
            this.chess5.TabStop = false;
            // 
            // GameRiver
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 243);
            this.Controls.Add(this.chess5);
            this.Controls.Add(this.chess6);
            this.Controls.Add(this.chess7);
            this.Controls.Add(this.chess8);
            this.Controls.Add(this.chess4);
            this.Controls.Add(this.chess3);
            this.Controls.Add(this.chess2);
            this.Controls.Add(this.chess1);
            this.Controls.Add(this.gameBack);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.messageList);
            this.Controls.Add(this.mainMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MainMenuStrip = this.mainMain;
            this.Name = "GameRiver";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "GameRiver";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GameRiver_FormClosing);
            this.Load += new System.EventHandler(this.GameRiver_Load);
            this.mainMain.ResumeLayout(false);
            this.mainMain.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gameBack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chess1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chess2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chess3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chess4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chess8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chess7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chess6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chess5)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMain;
        private System.Windows.Forms.ToolStripMenuItem gameMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startNewItem;
        private System.Windows.Forms.ToolStripMenuItem exitItem;
        private System.Windows.Forms.ListBox messageList;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statueBar;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.PictureBox gameBack;
        private System.Windows.Forms.PictureBox chess1;
        private System.Windows.Forms.PictureBox chess2;
        private System.Windows.Forms.PictureBox chess3;
        private System.Windows.Forms.PictureBox chess4;
        private System.Windows.Forms.PictureBox chess8;
        private System.Windows.Forms.PictureBox chess7;
        private System.Windows.Forms.PictureBox chess6;
        private System.Windows.Forms.PictureBox chess5;
    }
}