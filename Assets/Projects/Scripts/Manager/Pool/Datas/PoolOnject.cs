using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MTFrame.MTPool
{
    /// <summary>
    /// 池对象
    /// </summary>
    public class PoolOnject
    {
        /// <summary>
        /// 存储的资源
        /// </summary>
        public ISources sources;
        /// <summary>
        /// 目标的名字
        /// </summary>
        public string targetName;
    }
}