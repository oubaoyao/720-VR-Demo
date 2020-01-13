using MTFrame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 序列化累的Inspector面板显示
/// </summary>
[CustomEditor(typeof(MainBehavior), true)]
public class SerializeInspector : Editor
{
    /// <summary>
    /// 开始绘画InspectorGUI
    /// </summary>
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

       var targets =serializedObject.targetObjects;


        for (int j = 0; j < targets.Length; j++)
        {
            ISerializeButton target =targets[j]as ISerializeButton;
            if (target == null)
                return;
            for (int i = 0; i < target.SerializeButtonName.Count; i++)
            {
                if (GUILayout.Button( target.SerializeButtonName[i]))//在类面板绘画一个Init的按钮
                {
                    target.SerializeButtonMethod[i]?.Invoke();
                }
            }

        }
    
    }
    //public List<ISerializeButton> Get()
    //{
    //    var types = Assembly.GetCallingAssembly().GetTypes();
    //    var aType = typeof(ISerializeButton);
    //    Debug.Log(aType.FullName);
    //    List<ISerializeButton> ass = new List<ISerializeButton>();
    //    var typess = Assembly.GetCallingAssembly().GetTypes();  //获取所有类型
    //    foreach (var t in typess)
    //    {
    //        Type[] tfs = t.GetInterfaces();  //获取该类型的接口
    //        foreach (var tf in tfs)
    //        {
    //            if (tf.FullName == aType.FullName)  //判断全名，是否在一个命名空间下面
    //            {
    //                ISerializeButton a = Activator.CreateInstance(t) as ISerializeButton;
    //                ass.Add(a);
    //            }
    //        }
    //    }
    //    return ass;
    //}
}
