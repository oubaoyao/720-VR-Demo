using MTFrame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 状态管理器
/// </summary>
public  class StateManager
{
    public static BaseState CurrentState { get; private set; }

    /// <summary>
    /// 切换状态
    /// </summary>
    /// <param name="baseState"></param>
    public static void ChangeState(BaseState baseState)
    {
        if (CurrentState != null)
            CurrentState.Exit();
        CurrentState = baseState;
        CurrentState.Enter();
    }

}