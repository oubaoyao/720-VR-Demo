
using System.Collections.Generic;

public interface ISerializeButton
{
    /// <summary>
    /// 记录序列化按钮名字
    /// </summary>
    List<string> SerializeButtonName { get; }
    /// <summary>
    /// 记录序列化按钮回调方法
    /// </summary>
    List<System.Action> SerializeButtonMethod { get;}
}