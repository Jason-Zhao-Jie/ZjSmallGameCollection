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
    public enum GameRiverStatue
    {
        Waiting,
        EnemyRound,
        OurRound,
        Resulting
    }
    public partial class GameRiver : Form
    {
        /// <summary>玩家1</summary>
        private IGameRiverPlayer player1;
        /// <summary>玩家2</summary>
        private IGameRiverPlayer player2;
        /// <summary>界面图片框的引用</summary>
        private PictureBox[] pics;
        /// <summary>当前棋盘状况</summary>
        private int[] chesses;
        /// <summary>上一步棋盘状况</summary>
        private int[] lastChesses;
        /// <summary>界面监测线程</summary>
        private Thread mainUIThread;
        /// <summary>界面线程退出标志</summary>
        private bool mainUIThreadEnd;
        /// <summary>游戏流程状态</summary>
        public GameRiverStatue statue;
        /// <summary>上一个游戏流程状态，用于线程记忆</summary>
        public GameRiverStatue lastStatue;
        /// <summary>是否走完棋等待胜利检验</summary>
        private bool isStepReady;

        public delegate void OnChessClick(int clicked, bool isCanceled);
        public event OnChessClick OnClickChess1;
        public event OnChessClick OnClickChess2;

        public GameRiver()
        {
            InitializeComponent();
            DialogResult = DialogResult.OK;
            chesses = new int[8];
            lastChesses = new int[8] { 3, 3, 3, 3, 3, 3, 3, 3 };
            InitChesses();
            statue = GameRiverStatue.Waiting;
            isStepReady = false;
            OnClickChess1 = null;
            OnClickChess2 = null;
        }

        /// <summary>
        /// 选定棋子，使其反白，若已选定，则取消
        /// </summary>
        /// <param name="pos">选定的棋子位置</param>
        public void SelectChess(int pos)
        {
            if(this[pos] == 1 || this[pos] == -1)
            {
                for(int i = 0; i < 8; i++)
                {
                    if(chesses[i] > 1)
                        chesses[i] = 1;
                    else if(chesses[i] < -1)
                        chesses[i] = -1;
                }
                chesses[pos] *= 2;
            }
            else if(this[pos] != 0)
            {
                chesses[pos] /= 2;
            }
        }
        /// <summary>
        /// 走棋
        /// </summary>
        /// <returns>成功返回true</returns>
        public bool StepOn(int src, int dest)
        {
            if(!IsAbleToGo(src, dest))
                return false;
            chesses[dest] = chesses[src];
            chesses[src] = 0;
            if(chesses[dest] * chesses[dest] >= 4)
                chesses[dest] /= 2;
            ShowMessage("已走棋，从" + src + "到" + dest);
            return true;
        }
        /// <summary>
        /// 检测两个位置是否相邻
        /// </summary>
        /// <param name="self">位置1</param>
        /// <param name="target">位置2</param>
        /// <returns>若相邻则返回true</returns>
        public bool IsNearly(int self, int target)
        {
            if(self > 7 || self < 0 || target > 7 || target < 0)
                throw new System.Exception("Wrong index number of chesses");
            if(self == target)
                return false;
            if(((self == 0 || self == 1) && target == 7) || (self == 7 && (target == 0 || target == 1)))
                return true;
            if(self - target == 1 || self - target == -1)
                return true;
            if(self % 2 == 1 && (self - target == 2 || self - target == -2))
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
            if(self > 7 || self < 0)
                throw new System.Exception("Wrong index number of chesses");
            var num = new int[] { -1, -1, -1, -1 };
            int index = 0;
            for(int i = 0; i < 8; i++)
            {
                if(IsNearly(self, i) && i != ignoredPos)
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
            if(!IsNearly(self, target))
                return false;
            if(chesses[target] != 0)
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
            for(int i = 0; i < src.Length; i++)
            {
                if((!withPartner && chesses[src[i]] != 0) || (withPartner && chesses[src[i]] * chesses[self] < 0))
                {
                    index--;
                    src[i] = -1;
                }
            }
            if(index <= 0)
                return null;

            var res = new int[index];
            for(int i = 0, j = 0; i < index; j++)
            {
                if(src[j] >= 0)
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
            if(!IsAbleToGo(src, dest))
                throw new System.Exception("Cannot go to here");
            var nrly = GetAllNearIndex(dest, src);
            for(int i = 0; nrly != null && i < nrly.Length; i++)
            {
                var res = GetAllAbleIndex(nrly[i], true, src);
                if(nrly[i] != 0 && null == res)
                    return true;
            }
            if(IsAtonceWin()!=0)
                return true;
            return false;
        }
        /// <summary>
        /// 是否获胜，是否对方无棋可走
        /// </summary>
        /// <param name="ignored">将要走的棋子</param>
        /// <param name="instead">将要走到的位置</param>
        /// <returns>黑方胜，返回-1，白方返回1，未决胜负返回0</returns>
        public int IsAtonceWin(int ignored = -1, int instead = -1)
        {
            bool enemyFail = true;
            int lastIgNored = 3;
            int lastInstead = 3;
            if(ignored >= 0)
            {
                lastIgNored = chesses[ignored];
                chesses[ignored] = 0;
            }
            if(instead >= 0)
            {
                lastInstead = chesses[instead];
                chesses[instead] = 1;
            }
            for(int i = 0; i < 8; i++)
            {
                if(chesses[i] < 0 &&GetAllAbleIndex(i)!=null)
                {
                    enemyFail = false;
                }
            }
            if(enemyFail)
            {
                if(lastIgNored != 3)
                    chesses[ignored] = lastIgNored;
                if(lastInstead != 3)
                    chesses[instead] = lastInstead;
                return 1;
            }
            if(instead >= 0)
                chesses[instead] = -1;
            for(int i = 0; i < 8; i++)
            {
                if(chesses[i] > 0 && GetAllAbleIndex(i) != null)
                {
                    if(lastIgNored != 3)
                        chesses[ignored] = lastIgNored;
                    if(lastInstead != 3)
                        chesses[instead] = lastInstead;
                    return 0;
                }
            }
            if(lastIgNored != 3)
                chesses[ignored] = lastIgNored;
            if(lastInstead != 3)
                chesses[instead] = lastInstead;
            return -1;
        }
        /// <summary>
        /// 游戏结束判定
        /// </summary>
        /// <param name="src">要走的下一步棋子原位置</param>
        /// <param name="dest">要走的下一步目标位置</param>
        /// <returns>即将结束，返回true</returns>
        public bool WillEndGame(int src = -1, int dest = -1)
        {
            if(IsAtonceWin() != 0)
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
                if(index > 7 || index < 0)
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
                for(int i = 0; i < 8; i++)
                {
                    if(chesses[i] > 1 || chesses[i] < -1)
                        return i;
                }
                return -1;
            }
            set
            {
                if(value != SelectedItem)
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
                for(int i = 0; i < 8; i++)
                {
                    if(chesses[i] < 0)
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
                for(int i = 0; i < 8; i++)
                {
                    if(chesses[i] > 0)
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
            chesses[4] = 1;
            chesses[5] = 1;
            chesses[6] = 1;
            chesses[7] = 0;
        }

        private void ShowMessage(string message, bool isInList = true)
        {
            statueBar.Text = message;
            if(isInList)
                messageList.Items.Add(message);
        }

        private void CheckToCut()
        {
            for(int i = 0; i < 8; i++)
            {
                var ret = GetAllAbleIndex(i, true);
                System.Diagnostics.Debug.WriteLine("位置" + i + "还剩下" + (ret == null ? 0 : ret.Length) + "个可用节点");
                if(ret == null)
                {
                    chesses[i] = 0;
                    System.Diagnostics.Debug.WriteLine("位置" + i + "已经被吞吃");
                }
            }
        }

        private void GameRiver_Load(object sender, EventArgs e)
        {
            ListBox.CheckForIllegalCrossThreadCalls = false;
            player1 = new GameRiver_Local(this, true);
            player2 = new GameRiver_Local(this, false);
            pics = new PictureBox[8];
            pics[0] = chess1;
            pics[1] = chess2;
            pics[2] = chess3;
            pics[3] = chess4;
            pics[4] = chess5;
            pics[5] = chess6;
            pics[6] = chess7;
            pics[7] = chess8;
            player1.OnEndStep += () =>
            {
                statue = GameRiverStatue.OurRound;
                isStepReady = true;
                CheckToCut();
                player2.StepOn();
                ShowMessage("黑棋走毕，轮到白棋回合");
            };
            player2.OnEndStep += () =>
            {
                statue = GameRiverStatue.EnemyRound;
                isStepReady = true;
                CheckToCut();
                player1.StepOn();
                ShowMessage("白棋走毕，轮到黑棋回合");
            };
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
                    if(statue != GameRiverStatue.Resulting && statue != GameRiverStatue.Waiting)
                        return;
                    InitChesses();
                    lastChesses[0] = 3;
                    lastChesses[1] = 3;
                    lastChesses[2] = 3;
                    lastChesses[3] = 3;
                    lastChesses[4] = 3;
                    lastChesses[5] = 3;
                    lastChesses[6] = 3;
                    lastChesses[7] = 3;
                    ShowMessage("初始化完毕！");
                    player1.StartPlay();
                    player2.StartPlay();
                    statue = GameRiverStatue.OurRound;
                    player2.StepOn();
                    ShowMessage("游戏开始！轮到白棋回合");
                    break;
                case "configItem":
                    if(GameConfig.ShowDialog(this) == DialogResult.OK)
                        MessageBox.Show("配置保存成功！");
                    break;
                case "exitItem":
                    Close();
                    break;
            }
        }

        private void GameRiver_FormClosing(object sender, FormClosingEventArgs e)
        {
            mainUIThreadEnd = true;
            player1.OverPlay();
            player2.OverPlay();
            if(mainUIThread.ThreadState == ThreadState.Running)
                mainUIThread.Join();
        }

        private void chess_Click(object sender, EventArgs e)
        {
            var chess = (PictureBox)sender;
            for(int i = 0; i < 8; i++)
            {
                if(chess == pics[i])
                {
                    if(chesses[i] <= 0 && OnClickChess1 != null)
                        OnClickChess1(i, chesses[i] == -2);
                    if(chesses[i] >= 0 && OnClickChess2 != null)
                        OnClickChess2(i, chesses[i] == 2);
                }
            }
        }

        private void mainUIThreadFunc()
        {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(GameRiver));
            while(!mainUIThreadEnd)
            {
                Thread.Sleep(1);
                for(int i = 0; i < 8; i++)
                {
                    if(chesses[i] != lastChesses[i])
                    {
                        lastChesses[i] = chesses[i];
                        switch(chesses[i])
                        {
                            case -2:
                                pics[i].Image = Properties.Resources.GameRiverChessBlackChoosen;
                                break;
                            case -1:
                                pics[i].Image = Properties.Resources.GameRiverChessBlack;
                                break;
                            case 0:
                                pics[i].Image = null;
                                break;
                            case 1:
                                pics[i].Image = Properties.Resources.GameRiverChessWhite;
                                break;
                            case 2:
                                pics[i].Image = Properties.Resources.GameRiverChessWhiteChoosen;
                                break;
                        }
                    }
                }
                if(isStepReady)
                {
                    isStepReady = false;
                    System.Diagnostics.Debug.WriteLine("检测胜利条件");
                    var winret = IsAtonceWin();
                    if(winret != 0)
                    {
                        statue = GameRiverStatue.Resulting;
                        player1.OverPlay();
                        player2.OverPlay();
                    }
                    switch(winret)
                    {
                        case 1:
                            player1Statue.Text = "失败";
                            player1Statue.ForeColor = Color.Red;
                            player2Statue.Text = "获胜";
                            player2Statue.ForeColor = Color.Green;
                            MessageBox.Show("白方获胜！");
                            break;
                        case -1:
                            player1Statue.Text = "获胜";
                            player1Statue.ForeColor = Color.Green;
                            player2Statue.Text = "失败";
                            player2Statue.ForeColor = Color.Red;
                            MessageBox.Show("黑方获胜！");
                            break;
                    }
                }
                if(statue != lastStatue)
                {
                    lastStatue = statue;
                    switch(statue)
                    {
                        case GameRiverStatue.OurRound:
                            player1Statue.Text = "状态：等待中";
                            player1Statue.ForeColor = Color.Orange;
                            player2Statue.Text = "状态：正在行动";
                            player2Statue.ForeColor = Color.Blue;
                            break;
                        case GameRiverStatue.EnemyRound:
                            player1Statue.Text = "状态：正在行动";
                            player1Statue.ForeColor = Color.Blue;
                            player2Statue.Text = "状态：等待中";
                            player2Statue.ForeColor = Color.Orange;
                            break;
                    }
                }
            }
        }
    }

    public interface IGameRiverPlayer : IPlayer
    {
    }

    sealed public class GameRiver_Local : IGameRiverPlayer
    {
        public bool isEnemy;
        public GameRiver_Local(GameRiver parent, bool isEnemy = false)
        {
            this.parent = parent;
            this.isEnemy = isEnemy;
            statue = PlayerStatue.Unknown;
            name = "匿名玩家";
        }
        public bool StartPlay()
        {
            System.Diagnostics.Debug.WriteLine("玩家" + (isEnemy ? 1 : 2) + "准备就绪");
            statue = PlayerStatue.Ready;
            mainThread = new Thread(ThreadFunc);
            mainThread.Start();
            if(isEnemy)
                parent.OnClickChess1 += ClickCall;
            else
                parent.OnClickChess2 += ClickCall;
            return true;
        }
        public void OverPlay()
        {
            System.Diagnostics.Debug.WriteLine("玩家" + (isEnemy ? 1 : 2) + "游戏结束");
            statue = PlayerStatue.Unknown;
            if(isEnemy)
                parent.OnClickChess1 -= ClickCall;
            else
                parent.OnClickChess2 -= ClickCall;
            if(mainThread != null && mainThread.ThreadState == ThreadState.Running)
                mainThread.Join();
        }
        public void StepOn()
        {
            System.Diagnostics.Debug.WriteLine("玩家" + (isEnemy ? 1 : 2) + "回合开始");
            statue = PlayerStatue.Playing;
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
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public event VoidFunc OnEndStep;

        private Thread mainThread;
        private GameRiver parent;
        private PlayerStatue statue;
        private string name;
        private void ThreadFunc()
        {
            while(statue != PlayerStatue.Unknown)
            {
                if(statue == PlayerStatue.End)
                {
                    System.Diagnostics.Debug.WriteLine("玩家" + (isEnemy ? 1 : 2) + "回合完成");
                    statue = PlayerStatue.Ready;
                    OnEndStep();
                }
                Thread.Sleep(1);
            }
        }

        private void ClickCall(int index, bool isCanceled)
        {
            System.Diagnostics.Debug.WriteLine("玩家" + (isEnemy ? 1 : 2) + "点击了棋子");
            if(statue == PlayerStatue.Playing)
            {
                System.Diagnostics.Debug.WriteLine("玩家" + (isEnemy ? 1 : 2) + "点击有效");
                if(parent[index] != 0)
                    parent.SelectChess(index);
                else if(parent.SelectedItem >= 0)
                    if(parent.StepOn(parent.SelectedItem, index))
                        statue = PlayerStatue.End;
            }
        }
    }
}
