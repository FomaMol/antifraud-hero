using System;
using System.Collections.Generic;

namespace Common.Extensions
{
    public static class EnumExtensions
    {
        public static T Next<T>(this T src) where T : struct
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException($"Argument {typeof(T).FullName} is not an Enum");

            T[] Arr = (T[]) Enum.GetValues(src.GetType());
            int j = Array.IndexOf<T>(Arr, src) + 1;
            return (Arr.Length == j) ? Arr[0] : Arr[j];
        }
        
        public static IEnumerable<T> GetValues<T>() where T : Enum
        {
            return (T[]) Enum.GetValues(typeof(T));
        }
    }
}