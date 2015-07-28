using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZjSmallGameCollection
{
    public partial class WelcomeForm : Form
    {
        public WelcomeForm()
        {
            InitializeComponent();
        }

        private void WelcomeForm_Load(object sender, EventArgs e)
        {

        }

        private void btn_Click(object sender, EventArgs e)
        {
            Button snd = (Button)sender;
            switch(snd.Name)
            {
                case "GameRiver":   //过河棋
                    ZjSmallGameCollection.GameRiver.ShowDialog(this);
                    break;
                default:
                    MessageBox.Show("尚未开放，敬请期待！");
                    break;
            }
        }
    }
}
