using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZjSmallGameCollection
{
    public partial class GameRiver : Form
    {
        IGameRiverPlayer player;
        public GameRiver()
        {
            InitializeComponent();
        }

        private void GameRiver_Load(object sender, EventArgs e)
        {
            player = new GameRiver_Local(this);
            this.DialogResult = DialogResult.OK;
        }

        public static DialogResult ShowDialog(Form parent)
        {
            parent.Hide();
            DialogResult res = new GameRiver().ShowDialog((IWin32Window)parent);
            parent.Show();
            return res;
        }

        private void gameMenuItem_ChildClick(object sender, EventArgs e)
        {
            ToolStripMenuItem sdr = (ToolStripMenuItem)sender;
            switch(sdr.Name)
            {
                case "startNewItem":

                    break;
                case "exitItem":
                    this.Close();
                    break;
            }
        }
    }

    public interface IGameRiverPlayer : IPlayer
    {
    }

    sealed public class GameRiver_Local : IGameRiverPlayer
    {
        public GameRiver_Local(GameRiver parent)
        {
            this.parent = parent;
            statue = PlayerStatue.Unknown;
        }
        public bool StartPlay()
        {
            mainThread = new Thread(ThreadFunc);
            mainThread.Start();
            return true;
        }
        public void StepOn()
        {
        }
        public PlayerType Type
        {
            get
            {
                return PlayerType.Local;
            }
        }
        public PlayerStatue Statue
        {
            get
            {
                return statue;
            }
        }
        public event VoidFunc OnEndStep;

        private Thread mainThread;
        private GameRiver parent;
        private PlayerStatue statue;
        private void ThreadFunc()
        {

        }
    }
}
