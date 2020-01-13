using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 只会接收和发送对应枚举的数据
/// </summary>
public enum OperateCodes : byte
{
    Game
}

/// <summary>
/// 只会接收和发送对应枚举的数据
/// </summary>
public enum ParmaterCodes : byte
{
    index,
}
