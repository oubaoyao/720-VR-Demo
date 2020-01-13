using System;
using System.Collections.Generic;
using UnityEngine;


namespace MTFrame
{
    /// <summary>
    /// 每个阶段的场景交互物体（建议每个场景只放一个）
    /// </summary>
    [ExecuteInEditMode]
    public abstract class BaseGame : MainBehavior,ISerializeButton
    {
        public List<string> SerializeButtonName
        {
            get
            {
                return new List<string>
                {
                    "Init"
                };
            }
        }
        public List<Action> SerializeButtonMethod
        {
            get
            {
                return new List<Action>
                {
                    Init
                };
            }
        }
        protected override void Awake()
        {
            base.Awake();
            name = GetType().Name;
        }
        /// <summary>
        ///初始化
        /// </summary>
        public virtual void Init()
        {

        }
    }
}