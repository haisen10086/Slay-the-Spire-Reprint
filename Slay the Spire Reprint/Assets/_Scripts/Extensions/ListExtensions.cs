
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class List 
{
    //从列表中随机取出一个并移除
    public static T Draw<T>(this List<T> list)
    {
        if (list.Count == 0) return default;
        int r = Random.Range(0, list.Count);
        T t = list[r];
        list.Remove(t);
        return t;
    }
}
