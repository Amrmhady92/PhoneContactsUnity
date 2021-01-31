using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public static class ListExtentions {
   
    private static System.Random rng = new System.Random();
    // FISHER YATES SHUFFLE
    /// <summary>
    /// Shuffles the List using the Fisher-Yates shuffling method
    /// </summary>
    //public static void Shuffle<T>(this IList<T> list)
    //{
    //        int n = list.Count;
    //        while (n > 1)
    //        {
    //            n--;
    //            int k = rng.Next(n + 1);
    //            T value = list[k];
    //            list[k] = list[n];
    //            list[n] = value;
    //        }
    //}


    public static void Shuffle<T>(this IList<T> list)
    {
        RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
        int n = list.Count;
        while (n > 1)
        {
            byte[] box = new byte[1];
            do provider.GetBytes(box);
            while (!(box[0] < n * (Byte.MaxValue / n)));
            int k = (box[0] % n);
            n--;
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }


    /// <summary>
    /// Returns a Random Value from the List, make sure the List isn't Empty
    /// </summary>
    /// 

    public static T GetRandomValue<T>(this IList<T> list)
    {
        
        if(list.Count > 0)
        {
            return list[UnityEngine.Random.Range(0, list.Count)];
        }
        else
        {
            return default(T);
        }
    }

    public static List<T> ShiftLeft<T>(this List<T> list, int shiftBy = 1)
    {
        T item;
        for (int i = 0; i < shiftBy; i++)
        {
            item = list[0];
            list.RemoveAt(0);
            list.Add(item);
        }
        return list;
        //if (list.Count <= shiftBy)
        //{
        //    Debug.Log("LESS");
        //    return list;
        //}

        //var result = list.GetRange(shiftBy, list.Count - shiftBy);
        //result.AddRange(list.GetRange(0, shiftBy));
        //return result;
    }
    //public static List<T> ShiftLeft<T>(this List<T> list, int shiftBy = 1)
    //{

    //    T item = list[0];
    //    if (list.Count <= shiftBy)
    //    {
    //        Debug.Log("LESS");
    //        return list;
    //    }

    //    var result = list.GetRange(shiftBy, list.Count - shiftBy);
    //    result.AddRange(list.GetRange(0, shiftBy));
    //    return result;
    //}

    public static List<T> ShiftRight<T>(this List<T> list, int shiftBy = 1)
    {
        if (list.Count <= shiftBy)
        {
            return list;
        }

        var result = list.GetRange(list.Count - shiftBy, shiftBy);
        result.AddRange(list.GetRange(0, list.Count - shiftBy));
        return result;
    }
}
