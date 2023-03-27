using System.Collections;
using System.Collections.Generic;

public static class Dictionary
{
    /// <summary>
    /// 对TryGetValue()方法进行重写，如果找到值，返回值；如果找不到值，返回是null，而不是返回bool
    /// </summary>
    /// <typeparam name="Tkey">键</typeparam>
    /// <typeparam name="Tvalue">值</typeparam>
    /// <param name="dict">当前操作字典</param>
    /// <param name="key">键</param>
    /// <returns></returns>
    public static Tvalue TryGetValue<Tkey, Tvalue>(this Dictionary<Tkey, Tvalue> dict, Tkey key)
    {
        Tvalue tvalue;
        dict.TryGetValue(key, out tvalue);

        return tvalue;
    }
}