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
        private IGameRiverPlayer player;
        private PictureBox[] pics;
        private int[] chesses;
        private int[] lastChesses;
        private Thread mainUIThread;
        private bool mainUIThreadEnd;
        public GameRiver()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 选定棋子，使其反白，若已选定，则取消
        /// </summary>
        /// <param name="pos">选定的棋子位置</param>
        public void SelectChess(int pos)
        {
            if (this[pos] == 1 || this[pos] == -1)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (chesses[i] > 1)
                        chesses[i] = 1;
                    else if (chesses[i] < -1)
                        chesses[i] = -1;
                }
                chesses[pos] *= 2;
            }
            else if (this[pos] != 0)
            {
                chesses[pos] /= 2;
            }
        }
        /// <summary>
        /// 检测两个位置是否相邻
        /// </summary>
        /// <param name="self">位置1</param>
        /// <param name="target">位置2</param>
        /// <returns>若相邻则返回true</returns>
        public bool IsNearly(int self,int target)
        {
            if(self > 7 || self < 0 || target > 7 || target < 0 )
                throw new System.Exception("Wrong index number of chesses");
            if (self == target)
                return false;
            if (((self == 0 || self == 1) && target == 7) || (self == 7 && (target == 0 || target == 1)))
                return true;
            if (self - target == 1 || self - target == -1)
                return true;
            if (self / 2 == 0 && (self - target == 2 || self - target == -2))
                return true;
            return false;
        }
        /// <summary>
        /// 获取指定位置的所有相邻位置
        /// </summary>
        /// <param name="self">指定的位置</param>
        /// <param name="ignoredPos">要忽略的位置</param>
        /// <returns>返回self的所有邻近的位置</returns>
        public int[] GetAllNearIndex(int self, int ignoredPos = -1)
        {
            if (self > 7 || self < 0)
                throw new System.Exception("Wrong index number of chesses");
            var num = new int[] { -1, -1, -1, -1 };
            int index = 0;
            for(int i = 0; i < 8; i++)
            {
                if (IsNearly(self, i) && i != ignoredPos)
                    num[index++] = i;
            }
            var res = new int[index];
            for(int i = 0; i < index; i++)
            {
                res[i] = num[i];
            }
            return res;
        }
        /// <summary>
        /// 检测是否能到达目标位置
        /// </summary>
        /// <param name="self">原位置</param>
        /// <param name="target">要到达的目标位置</param>
        /// <returns>如果目标位置与原位置相邻，且目标位置为空，则允许到达，返回true</returns>
        public bool IsAbleToGo(int self, int target)
        {
            if (!IsNearly(self, target))
                return false;
            if (chesses[target] != 0)
                return false;
            return true;
        }
        /// <summary>
        /// 获取所有可到达的位置
        /// </summary>
        /// <param name="self">原位置</param>
        /// <param name="withPartner">是否将相邻的同阵营棋子算入列表</param>
        /// <param name="ignoredPos">要忽略的位置</param>
        /// <returns>返回结果</returns>
        public int[] GetAllAbleIndex(int self, bool withPartner = false, int ignoredPos = -1)
        {
            var src = GetAllNearIndex(self, ignoredPos);
            var index = src.Length;
            for (int i = 0; i < src.Length; i++)
            {
                if ((!withPartner && chesses[src[i]] != 0) || (withPartner && chesses[src[i]] * self <= 0))
                {
                    index--;
                    src[i] = -1;
                }
            }
            if (index <= 0)
                return null;

            var res = new int[index];
            for (int i = 0, j = 0; i < index; j++)
            {
                if (src[j] >= 0)
                {
                    res[i] = src[j];
                    i++;
                }
            }
            return res;
        }
        /// <summary>
        /// 按此走棋是否将会吃掉敌方棋子
        /// </summary>
        /// <param name="src">原棋子位置</param>
        /// <param name="dest">要走到的位置</param>
        /// <returns>会吃掉敌方棋子，返回true</returns>
        public bool WillCutEnemy(int src, int dest)
        {
            if (!IsAbleToGo(src, dest))
                throw new System.Exception("Cannot go to here");
            var nrly = GetAllNearIndex(dest, src);
            for(int i = 0; nrly != null && i < nrly.Length; i++)
            {
                var res = GetAllAbleIndex(nrly[i], true, src);
                if (nrly[i] != 0 && null == res)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// 游戏结束判定
        /// </summary>
        /// <param name="src">要走的下一步棋子原位置</param>
        /// <param name="dest">要走的下一步目标位置</param>
        /// <returns>即将结束，返回true</returns>
        public bool WillEndGame(int src = 0, int dest = 0)
        {
            if (Enemies == 0 || Ours == 0 || ((Enemies == 1 || Ours == 1) && WillCutEnemy(src, dest)))
                return true;
            return false;
        }
        /// <summary>
        /// 取对应索引处的棋子状态
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public int this[int index]
        {
            get
            {
                if (index > 7 || index < 0)
                    throw new System.Exception("Wrong index number of chesses");
                return chesses[index];
            }
        }
        /// <summary>
        /// 获取或设定当前选中的棋子
        /// </summary>
        public int SelectedItem
        {
            get
            {
                for (int i = 0; i < 8; i++)
                {
                    if (chesses[i] > 1 || chesses[i] < -1)
                        return i;
                }
                return -1;
            }
            set
            {
                if (value != SelectedItem)
                    SelectChess(value);
            }
        }
        /// <summary>
        /// 获取敌人（黑方）剩余的棋子总数
        /// </summary>
        public int Enemies
        {
            get
            {
                int res = 0;
                for (int i = 0; i < 8; i++)
                {
                    if (chesses[i] < 0)
                        res++;
                }
                return res;
            }
        }
        /// <summary>
        /// 获取我方（红方）剩余的棋子总数
        /// </summary>
        public int Ours
        {
            get
            {
                int res = 0;
                for (int i = 0; i < 8; i++)
                {
                    if (chesses[i] > 0)
                        res++;
                }
                return res;
            }
        }
        /// <summary>
        /// 弹出过河棋游戏对话框，并隐藏父对话框
        /// </summary>
        /// <param name="parent">父对话框</param>
        /// <returns>对话框返回值，恒为OK</returns>
        public static DialogResult ShowDialog(Form parent)
        {
            parent.Hide();
            var res = new GameRiver().ShowDialog((IWin32Window)parent);
            parent.Show();
            return res;
        }

        private void InitChesses()
        {
            chesses[0] = -1;
            chesses[1] = -1;
            chesses[2] = -1;
            chesses[3] = 0;
            chesses[4] = 0;
            chesses[5] = 1;
            chesses[6] = 1;
            chesses[7] = 1;
        }

        private void GameRiver_Load(object sender, EventArgs e)
        {
            player = new GameRiver_Local(this);
            this.DialogResult = DialogResult.OK;
            pics = new PictureBox[8];
            pics[0] = chess1;
            pics[1] = chess2;
            pics[2] = chess3;
            pics[3] = chess4;
            pics[4] = chess5;
            pics[5] = chess6;
            pics[6] = chess7;
            pics[7] = chess8;
            chesses = new int[8];
            lastChesses = new int[8];
            lastChesses[0] = 3;
            lastChesses[1] = 3;
            lastChesses[2] = 3;
            lastChesses[3] = 3;
            lastChesses[4] = 3;
            lastChesses[5] = 3;
            lastChesses[6] = 3;
            lastChesses[7] = 3;
            InitChesses();
            mainUIThread = new Thread(mainUIThreadFunc);
            mainUIThreadEnd = false;
            mainUIThread.Start();
        }

        private void gameMenuItem_ChildClick(object sender, EventArgs e)
        {
            var sdr = (ToolStripMenuItem)sender;
            switch(sdr.Name)
            {
                case "startNewItem":

                    break;
                case "exitItem":
                    this.Close();
                    break;
            }
        }

        private void mainUIThreadFunc()
        {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(GameRiver));
            while (!mainUIThreadEnd)
            {
                Thread.Sleep(1);
                for(int i = 0; i < 8; i++)
                {
                    if (chesses[i] != lastChesses[i])
                    {
                        lastChesses[i] = chesses[i];
                        switch (chesses[i])
                        {
                            case -2:
                                pics[i].Image = ((System.Drawing.Image)(resources.GetObject("chess2.Image")));
                                break;
                            case -1:
                                pics[i].Image = ((System.Drawing.Image)(resources.GetObject("chess1.Image")));
                                break;
                            case 0:
                                pics[i].Image = null;
                                break;
                            case 1:
                                pics[i].Image = ((System.Drawing.Image)(resources.GetObject("chess7.Image")));
                                break;
                            case 2:
                                pics[i].Image = ((System.Drawing.Image)(resources.GetObject("chess6.Image")));
                                break;
                        }
                    }
                }
            }
        }

        private void GameRiver_FormClosing(object sender, FormClosingEventArgs e)
        {
            mainUIThreadEnd = true;
            mainUIThread.Join();
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
