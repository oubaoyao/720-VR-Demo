using System;
using System.Collections.Generic;


namespace MTFrame.MTEvent
{
    /// <summary>
    /// 事件参数
    /// </summary>
    public class EventParamete : IDisposable
    {
        /// <summary>
        /// 事件名称
        /// </summary>
        public string EvendName;
        /// <summary>
        /// 数据保存列表
        /// </summary>
        private Dictionary<string, object> ParameterDic = new Dictionary<string, object>();

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameter"></param>
        public void AddParameter<T>(params T[] parameter)
        {
            string typeName = typeof(T).Name;
            if (!ParameterDic.ContainsKey(typeName))
            {
                ParameterDic.Add(typeName, parameter);
            }
            else
            {
                ParameterDic[typeName] = null;
                ParameterDic[typeName] = parameter;
            }
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T[] GetParameter<T>()
        {
            string typeName = typeof(T).Name;
            object result = null;

            if (ParameterDic.TryGetValue(typeName, out result))
            {
                return (T[])result;
            }
            return null;
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        public void ClearParameter()
        {
            ParameterDic.Clear();
        }

        public virtual void Dispose()
        {
            ParameterDic.Clear();
            ParameterDic = null;
        }
    }
}