

namespace MTFrame.MTKinect
{
    public class Player
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserID { get; private set; }
        /// <summary>
        /// 玩家索引
        /// </summary>
        public int PlayerIndex { get;  set; }
        /// <summary>
        /// 玩家是否活动中
        /// </summary>
        public bool isActivate { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="playerIndex"></param>
        public Player(long userID, int playerIndex)
        {
            UserID = userID;
            PlayerIndex = playerIndex;
        }

        /// <summary>
        /// 激活玩家
        /// </summary>
        public void ActivatePlayer()
        {
            isActivate = true;
        }
        /// <summary>
        /// 停止玩家
        /// </summary>
        public void StopPlayer()
        {
            isActivate = false;
        }
    }
}