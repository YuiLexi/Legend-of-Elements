using System.Collections;
using System.Collections.Generic;

public static class Dictionary
{
    /// <summary>
    /// ��TryGetValue()����������д������ҵ�ֵ������ֵ������Ҳ���ֵ��������null�������Ƿ���bool
    /// </summary>
    /// <typeparam name="Tkey">��</typeparam>
    /// <typeparam name="Tvalue">ֵ</typeparam>
    /// <param name="dict">��ǰ�����ֵ�</param>
    /// <param name="key">��</param>
    /// <returns></returns>
    public static Tvalue TryGetValue<Tkey, Tvalue>(this Dictionary<Tkey, Tvalue> dict, Tkey key)
    {
        Tvalue tvalue;
        dict.TryGetValue(key, out tvalue);

        return tvalue;
    }
}