using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MTFrame
{
    public abstract class BaseProp : MainBehavior
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public abstract void Init();
        /// <summary>
        /// 关闭
        /// </summary>
        public abstract void Close();
    }
}