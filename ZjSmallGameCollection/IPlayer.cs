using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZjSmallGameCollection
{
    public delegate void VoidFunc();
    public enum PlayerType
    {
        /// <summary>本机玩家</summary>
        Local,
        /// <summary>网络玩家</summary>
        Network,
        /// <summary>电脑玩家</summary>
        AutoIntel
    }
    public enum PlayerStatue
    {
        Unknown,
        Ready,
        Playing,
        End
    }
    /// <summary>
    /// 用于定义玩家的公共基接口
    /// 应该为每个游戏单独派生一个接口
    /// </summary>
    public interface IPlayer
    {
        /// <summary>
        /// 开始本玩家游戏，用于初始化一些数据
        /// </summary>
        /// <returns>初始化是否成功</returns>
        bool StartPlay();
        /// <summary>
        /// 结束游戏通知，用于处理一些数据例如线程
        /// </summary>
        void OverPlay();
        /// <summary>
        /// 开始本玩家回合
        /// </summary>
        void StepOn();
        /// <summary>
        /// 玩家类型，每个具体的类的此属性应该输出常量
        /// </summary>
        PlayerType Type
        {
            get;
        }
        PlayerStatue Statue
        {
            get;
        }
        string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 当本玩家回合执行完毕之后调用的事件
        /// </summary>
        event VoidFunc OnEndStep;
    }
}
