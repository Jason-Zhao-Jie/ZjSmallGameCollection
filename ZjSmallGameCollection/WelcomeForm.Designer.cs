namespace ZjSmallGameCollection
{
    partial class WelcomeForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.GameRiver = new System.Windows.Forms.Button();
            this.GameBlackWhite = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // GameRiver
            // 
            this.GameRiver.Location = new System.Drawing.Point(12, 12);
            this.GameRiver.Name = "GameRiver";
            this.GameRiver.Size = new System.Drawing.Size(75, 23);
            this.GameRiver.TabIndex = 0;
            this.GameRiver.Text = "过河棋";
            this.GameRiver.UseVisualStyleBackColor = true;
            this.GameRiver.Click += new System.EventHandler(this.btn_Click);
            // 
            // GameBlackWhite
            // 
            this.GameBlackWhite.Location = new System.Drawing.Point(93, 12);
            this.GameBlackWhite.Name = "GameBlackWhite";
            this.GameBlackWhite.Size = new System.Drawing.Size(75, 23);
            this.GameBlackWhite.TabIndex = 1;
            this.GameBlackWhite.Text = "黑白棋";
            this.GameBlackWhite.UseVisualStyleBackColor = true;
            this.GameBlackWhite.Click += new System.EventHandler(this.btn_Click);
            // 
            // WelcomeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.GameBlackWhite);
            this.Controls.Add(this.GameRiver);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WelcomeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "小游戏合辑";
            this.Load += new System.EventHandler(this.WelcomeForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button GameRiver;
        private System.Windows.Forms.Button GameBlackWhite;
    }
}

