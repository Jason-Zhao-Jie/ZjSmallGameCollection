using System;
using System.Collections;
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
    using IntPairList = System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<int, int>>;
    public enum GameRiverStatue
    {
        Waiting,
        EnemyRound,
        OurRound,
        Resulting
    }
    public partial class GameRiver : Form
    {
        private System.Xml.XmlElement cfg;
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
            cfg = GameConfig.GetCfgXml()["GameRiver"];
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
            if(IsAtonceWin() != 0)
                return true;
            return false;
        }

        public bool WillBeCut(int src, int dest)
        {
            int[] mine;
            int[] others;
            if(chesses[src] > 0)
            {
                mine = Ours;
                others = Enemies;
            }
            else
            {
                mine = Enemies;
                others = Ours;
            }

            for(int i = 0; i < mine.Length; i++)
            {
                //本棋子
                int csM = chesses[mine[i]];
                if(mine[i] == src)
                {
                    mine[i] = dest;
                }
                //若在中线
                if(mine[i] / 2 == 1)
                    continue;
                //两个相邻位
                int a;
                int b;
                if(mine[i] == 0)
                {
                    a = 7;
                    b = 1;
                }
                else if(mine[i] == 7)
                {
                    a = 6;
                    b = 0;
                }
                else
                {
                    a = mine[i] - 1;
                    b = mine[i] + 1;
                }

                //相邻位的值
                int csA = chesses[a];
                if(a == src)
                    csA = 0;
                else if(a == dest)
                    csA = csM;
                int csB = chesses[b];
                if(b == src)
                    csB = 0;
                else if(b == dest)
                    csB = csM;

                //相邻有我方棋子
                if(csA * chesses[mine[i]] > 0 || csA * chesses[mine[i]] > 0)
                    continue;
                //相邻无敌方
                if(csA == 0 && csB == 0)
                    continue;

                //相邻的后位
                int a1, a2, b1, b2;
                if(a == 0)
                {
                    a1 = 7;
                    a2 = 6;
                }
                else if(a == 1)
                {
                    a1 = 6;
                    a2 = 5;
                }
                else
                {
                    a1 = a - 1;
                    a2 = a - 2;
                }
                if(b == 6)
                {
                    b1 = 7;
                    b2 = 0;
                }
                else if(b == 7)
                {
                    b1 = 0;
                    b2 = 1;
                }
                else
                {
                    b1 = b + 1;
                    b2 = b + 2;
                }
                //空相邻点敌方无法到达
                if(csB == 0 && (b1 == src || b1 == dest || chesses[b1] == 0) && (b2 == src || b2 == dest || chesses[b2] == 0))
                    continue;
                if(csA == 0 && (a1 == src || a1 == dest || chesses[a1] == 0) && (a2 == src || a2 == dest || chesses[a2] == 0))
                    continue;
                return true;
            }

            return false;
        }
        /// <summary>
        /// 按此走棋能否让我方棋子处于一个角落处，这在AI思维里比随意走棋优先级高
        /// 如果我方只剩两颗棋，则只要相邻就返回true
        /// </summary>
        /// <param name="src">目标棋子</param>
        /// <param name="dest">目标地点</param>
        /// <returns>三棋处于一个角落，或仅剩的两棋相邻，返回true</returns>
        public bool WillBeAllNear(int src, int dest)
        {
            int num = 0;
            if(chesses[src] < 0)
            {
                num = Enemies.Length;
            }
            else if(chesses[src] > 0)
            {
                num = Ours.Length;
            }
            else
                throw new Exception("The chess founded in " + src + " is missed !");
            if(chesses[dest] != 0)
                throw new Exception("The chess going to " + dest + " is not empty !");

            if(num == 2)
            {
                var res = GetAllNearIndex(dest, src);
                for(int i = 0; res != null && i < res.Length; i++)
                {
                    if(chesses[src] * chesses[res[i]] > 0)
                        return true;
                }
            }
            else if(num == 3)
            {
                int[] total = { -1, -1 };
                int index = 0;
                var res = GetAllNearIndex(dest, src);
                for(int i = 0; res != null && i < res.Length; i++)
                {
                    if(chesses[src] * chesses[res[i]] > 0)
                        total[index++] = res[i];
                    if(index >= 2)
                    {
                        if(dest % 2 == 0)
                        {
                            if(total[0] == dest + 1 && total[1] == dest - 1)
                                return true;
                            if(total[0] == dest - 1 && total[1] == dest + 1)
                                return true;
                            if(dest == 0 && total[0] == 7 && total[1] == 1)
                                return true;
                            if(dest == 0 && total[0] == 1 && total[1] == 7)
                                return true;
                        }
                        else
                        {
                            if(total[0] == dest + 1 && total[1] == dest + 2)
                                return true;
                            if(total[1] == dest + 1 && total[0] == dest + 2)
                                return true;
                            if(total[0] == dest - 1 && total[1] == dest - 2)
                                return true;
                            if(total[1] == dest - 1 && total[0] == dest - 2)
                                return true;
                            if(dest == 8 && total[0] == 0 && total[1] == 1)
                                return true;
                            if(dest == 8 && total[1] == 0 && total[0] == 1)
                                return true;
                            if(dest == 1 && total[0] == 0 && total[1] == 8)
                                return true;
                            if(dest == 1 && total[1] == 0 && total[0] == 8)
                                return true;
                        }
                    }
                }
            }

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
                if(chesses[i] < 0 && GetAllAbleIndex(i) != null)
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
            if(IsAtonceWin(src, dest) != 0)
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
        /// 获取敌人（黑方）剩余的棋子
        /// </summary>
        public int[] Enemies
        {
            get
            {
                int[] res;
                int num = 0;
                for(int i = 0; i < 8; i++)
                {
                    if(chesses[i] < 0)
                        num++;
                }
                res = new int[num];
                for(int i = 0, j = 0; i < 8; i++)
                {
                    if(chesses[i] < 0)
                    {
                        res[j] = i;
                        j++;
                    }
                }
                return res;
            }
        }
        /// <summary>
        /// 获取我方（红方）剩余的棋子
        /// </summary>
        public int[] Ours
        {
            get
            {
                int num = 0;
                for(int i = 0; i < 8; i++)
                {
                    if(chesses[i] > 0)
                        num++;
                }
                int[] res = new int[num];
                for(int i = 0, j = 0; i < 8; i++)
                {
                    if(chesses[i] > 0)
                    {
                        res[j] = i;
                        j++;
                    }
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
            pics = new PictureBox[8];
            pics[0] = chess1;
            pics[1] = chess2;
            pics[2] = chess3;
            pics[3] = chess4;
            pics[4] = chess5;
            pics[5] = chess6;
            pics[6] = chess7;
            pics[7] = chess8;
            InitPlayers();
            mainUIThread = new Thread(mainUIThreadFunc);
            mainUIThreadEnd = false;
            mainUIThread.Start();
        }

        private void InitPlayers()
        {
            switch(cfg["player1"]["type"].InnerText)
            {
                case "local":
                    player1 = new GameRiver_Local(this, cfg["player1"]["name"].InnerText, true);
                    break;
                case "auto":
                    player1 = new GameRiver_Auto(this, Convert.ToInt32(cfg["player1"]["AIdt"].InnerText), true);
                    break;
                case "network":
                    player1 = new GameRiver_Local(this, cfg["player1"]["name"].InnerText, true);
                    break;
            }
            player1Name.Text = player1.Name;
            switch(cfg["player2"]["type"].InnerText)
            {
                case "local":
                    player2 = new GameRiver_Local(this, cfg["player2"]["name"].InnerText, false);
                    break;
                case "auto":
                    player2 = new GameRiver_Auto(this, Convert.ToInt32(cfg["player2"]["AIdt"].InnerText), false);
                    break;
                case "network":
                    player2 = new GameRiver_Local(this, cfg["player2"]["name"].InnerText, false);
                    break;
            }
            player2Name.Text = player2.Name;
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
        }

        private void gameMenuItem_ChildClick(object sender, EventArgs e)
        {
            var sdr = (ToolStripMenuItem)sender;
            switch(sdr.Name)
            {
                case "startNewItem":
                    if(statue != GameRiverStatue.Resulting && statue != GameRiverStatue.Waiting)
                        return;
                    bool isBlackFirst = Convert.ToInt32(cfg["isWhiteFirst"].InnerText) == 0;
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
                    InitPlayers();
                    player1.StartPlay();
                    player2.StartPlay();
                    if(isBlackFirst)
                    {
                        statue = GameRiverStatue.EnemyRound;
                        player1.StepOn();
                        ShowMessage("游戏开始！轮到黑棋回合");
                    }
                    else
                    {
                        statue = GameRiverStatue.OurRound;
                        player2.StepOn();
                        ShowMessage("游戏开始！轮到白棋回合");
                    }
                    break;
                case "configItem":
                    if(GameConfig.ShowDialog(this) == DialogResult.OK)
                    {
                        cfg = GameConfig.GetCfgXml()["GameRiver"];
                        MessageBox.Show("配置保存成功！");
                    }
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
    /// <summary>
    /// 继承了玩家接口的GameRiver游戏玩家操作总抽象类
    /// </summary>
    public abstract class IGameRiverPlayer : IPlayer
    {
        public bool isEnemy;
        public event VoidFunc OnEndStep;

        protected Thread mainThread;
        protected GameRiver parent;
        protected PlayerStatue statue;
        protected string name;

        public IGameRiverPlayer(GameRiver parent, string name, bool isEnemy = false)
        {
            this.parent = parent;
            this.isEnemy = isEnemy;
            statue = PlayerStatue.Unknown;
            this.name = name;
        }

        public virtual bool StartPlay()
        {
            System.Diagnostics.Debug.WriteLine("玩家" + (isEnemy ? 1 : 2) + "准备就绪");
            statue = PlayerStatue.Ready;
            mainThread = new Thread(ThreadFunc);
            mainThread.Start();
            return true;
        }
        public virtual void OverPlay()
        {
            System.Diagnostics.Debug.WriteLine("玩家" + (isEnemy ? 1 : 2) + "游戏结束");
            statue = PlayerStatue.Unknown;
            if(mainThread != null && mainThread.ThreadState == ThreadState.Running)
                mainThread.Join();
        }
        public virtual void StepOn()
        {
            System.Diagnostics.Debug.WriteLine("玩家" + (isEnemy ? 1 : 2) + "回合开始");
            statue = PlayerStatue.Playing;
        }
        protected abstract void ThreadFunc();
        protected void CallEndEvent()
        {
            OnEndStep();
        }
        public virtual PlayerType Type
        {
            get;
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
    }
    /// <summary>
    /// 本机用户操作处理
    /// </summary>
    sealed public class GameRiver_Local : IGameRiverPlayer
    {
        public GameRiver_Local(GameRiver parent, string name, bool isEnemy = false)
            : base(parent, name, isEnemy)
        {
        }
        public override bool StartPlay()
        {
            if(isEnemy)
                parent.OnClickChess1 += ClickCall;
            else
                parent.OnClickChess2 += ClickCall;
            return base.StartPlay();
        }
        public override void OverPlay()
        {
            if(isEnemy)
                parent.OnClickChess1 -= ClickCall;
            else
                parent.OnClickChess2 -= ClickCall;
            base.OverPlay();
        }
        public override PlayerType Type
        {
            get
            {
                return PlayerType.Local;
            }
        }

        protected override void ThreadFunc()
        {
            while(statue != PlayerStatue.Unknown)
            {
                if(statue == PlayerStatue.End)
                {
                    System.Diagnostics.Debug.WriteLine("玩家" + (isEnemy ? 1 : 2) + "回合完成");
                    statue = PlayerStatue.Ready;
                    CallEndEvent();
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
    /// <summary>
    /// 本机电脑玩家操作处理
    /// </summary>
    sealed public class GameRiver_Auto : IGameRiverPlayer
    {
        private int delay;
        public GameRiver_Auto(GameRiver parent, int delay, bool isEnemy = false)
            : base(parent, "电脑", isEnemy)
        {
            this.delay = delay;
        }
        public override PlayerType Type
        {
            get
            {
                return PlayerType.AutoIntel;
            }
        }
        protected override void ThreadFunc()
        {
            while(statue != PlayerStatue.Unknown)
            {
                if(statue == PlayerStatue.Playing)
                {
                    System.Diagnostics.Debug.WriteLine("玩家" + (isEnemy ? 1 : 2) + "正在思考");
                    Thread.Sleep(this.delay);
                    //这里进行AI步骤
                    AnalysisToGo();
                    System.Diagnostics.Debug.WriteLine("玩家" + (isEnemy ? 1 : 2) + "回合完成");
                    statue = PlayerStatue.Ready;
                    CallEndEvent();
                }
                Thread.Sleep(1);
            }
        }

        private void AnalysisToGo()
        {
            var steps = GetAllAvailableStep();
            if(steps == null || steps.Count <= 0)
                throw new Exception("No available steps to go");
            var step = steps.First();
            var score = ScoreStep(step.Key, step.Value);
            System.Diagnostics.Debug.WriteLine("AI 获取总共可走步骤数：" + steps.Count);
            System.Diagnostics.Debug.WriteLine("AI 获取可走步骤：" + step.Key + " -> " + step.Value + " ,评分：" + score);


            for(int i = 1; i < steps.Count; i++)
            {
                System.Diagnostics.Debug.WriteLine("AI 获取可走步骤：" + steps[i].Key + " -> " + steps[i].Value + " ,评分：" + ScoreStep(steps[i].Key, steps[i].Value));
                if(ScoreStep(steps[i].Key, steps[i].Value) > score)
                {
                    step = steps[i];
                    score = ScoreStep(step.Key, step.Value);
                }
            }
            parent.StepOn(step.Key, step.Value);
        }

        private IntPairList GetAllAvailableStep()
        {
            var ret = new IntPairList();
            for(int i = 0; i < 8; i++)
            {
                if((isEnemy && parent[i] >= 0) || (!isEnemy && parent[i] <= 0))
                    continue;
                var keyv = parent.GetAllAbleIndex(i);
                for(int j = 0; keyv != null && j < keyv.Length; j++)
                {
                    var ins = new KeyValuePair<int, int>(i, keyv[j]);
                    ret.Add(ins);
                }
            }
            return ret;
        }
        /// <summary>
        /// 电脑AI对某个可行步骤的评分，所有可行步骤中评分最高的步骤会被选择
        /// </summary>
        /// <param name="src">我方棋子原位置</param>
        /// <param name="dest">此步骤的目标位置</param>
        /// <returns>评分值</returns>
        private int ScoreStep(int src, int dest)
        {
            int cutScore = 0;
            if(parent.WillBeCut(src, dest))
                cutScore += 25;
            if(parent.WillEndGame(src, dest))       //如果此步结束游戏
                return 100;
            else if(parent.WillCutEnemy(src, dest)) //如果此步吃掉棋子
                return 90 - cutScore;
            else if(src % 2 == 0 && dest % 2 == 1)  //如果此步从角到中
                return 60 - cutScore;
            else if(parent.WillBeAllNear(src, dest))//如果此步让我方棋子集中在一起
                return 50 - cutScore;
            else if(src % 2 == 1 && dest % 2 == 1)  //如果此步从中到中
                return 40 - cutScore;
            else
                return 30 - cutScore;
        }
    }
}

