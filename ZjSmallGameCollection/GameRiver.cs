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
    public partial class GameRiver : Form
    {
        public GameRiver()
        {
            InitializeComponent();
        }

        public static DialogResult ShowDialog(Form parent)
        {
            parent.Hide();
            DialogResult res = new GameRiver().ShowDialog();
            parent.Show();
            return res;
        }

        private void GameRiver_Load(object sender, EventArgs e)
        {

        }
    }
}
