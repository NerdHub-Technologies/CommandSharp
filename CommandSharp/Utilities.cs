/* Utilities.cs
 * ----------------
 * PROJECT: CommandSharp
 * AUTHOR: WinMister332
 * LICENSE: MIT (https://opensource.org/licenses/MIT)
 * ----------------
 * NOTICE:
 *      You must include a copy of "license.txt" if you use CommandSharp. If you're using code pulled from the repo, you must also include this header.
 * ----------------
 * Copyright (c) 2017-2021 NerdHub Technologies, All Rights Reserved.
 */
using CommandSharp.Commands;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CommandSharp
{
    public sealed class Utilities
    {
        public static T[] Skip<T>(T[] objArray, int count)
        {
            List<T> sL = new List<T>();
            for (int i = 0; i < objArray.Length; i++)
            {
                if (i <= (count - 1))
                    continue;
                else
                    sL.Add(objArray[i]);
            }
            return sL.ToArray();
        }

        public static string[] Split(string str, string spl)
        {
            List<string> sL = new List<string>();
            string sx = "";
            for (int i = 0; i < str.Length; i++)
            {
                var c = str[i];
                //Do a constant test.
                if (sx.EndsWith(spl))
                {
                    //Manually remove the spl string, add the SX value to SL, then clear sx and continue parsing.
                    sx = sx.Remove(sx.Length - spl.Length, spl.Length);
                    sL.Add(sx);
                    sx = "";
                    //Add to SX.
                    sx += c;
                }
                else
                    sx += c;
            }
            //Check if the spl string exists at the start or end that was not processed and process it.
            if (!(string.IsNullOrEmpty(sx) && string.IsNullOrWhiteSpace(sx)))
            {
                if (sx.EndsWith(spl))
                    sx = sx.Remove(sx.Length - spl.Length, spl.Length);
                else if (sx.StartsWith(spl))
                    sx = sx.Remove(0, spl.Length);
                sL.Add(sx);
            }
            return sL.ToArray();
        }

        public static bool Contains<T>(T[] tArr, T value)
        {
            foreach (T t in tArr)
            {
                if (t.Equals(value))
                    return true;
                else continue;
            }
            return false;
        }

        public static string Replace(string dest, char oldValue, char newValue)
        {
            var cs = new char[dest.Length];

            for (int i = 0; i < dest.Length; i++)
            {
                if (dest[i] != oldValue)
                {
                    cs[i] = dest[i];
                }
                else
                {
                    cs[i] = newValue;
                }
            }

            return new string(cs);
        }

        public static string Replace(string dest, string oldValue, string newValue)
        {
            while (dest.IndexOf(oldValue) != -1)
            {
                int xIndex = dest.IndexOf(oldValue);
                dest = dest.Remove(xIndex, oldValue.Length);
                dest = dest.Insert(xIndex, newValue);
            }
            return dest;
        }

        public static string Insert(string dest, int aStartPos, string aValue)
        {
            return dest.Substring(0, aStartPos) + aValue + dest.Substring(aStartPos);
        }

        public static string Remove(string dest, int aStart, int aCount)
        {
            return dest.Substring(0, aStart) + dest.Substring(aStart + aCount, dest.Length - (aStart + aCount));
        }

        public static T[] Add<T>(T[] arr, T item)
        {
            List<T> tL = new List<T>(arr.Length + 1);
            foreach (T t in arr)
                tL.Add(t);
            tL.Add(item);
            return tL.ToArray();
        }

        public static T[] InsertAt<T>(T[] arr, int index, T item)
        {
            List<T> tL = new List<T>(arr.Length + 1);
            int n = 0;
            for (int i = 0; i < tL.Capacity; i++)
            {
                if (i == index)
                {
                    tL.Add(item);
                    n--; //Go back 1.
                }
                else
                {
                    var x = arr[n];
                    tL.Add(x);
                }
                if (n < arr.Length)
                    n++;
            }
            return tL.ToArray();
        }

        public static T[] InsertRangeAt<T>(T[] arr, int index, T[] range)
        {
            List<T> tL = new List<T>(arr.Length + range.Length);
            int m = 0;
            int n = 0;
            for (int i = 0; i < tL.Capacity; i++)
            {
                //Check if the 'i'is inbetween the 'index' and 'endIndex'.
                if (i <= (index + range.Length) && i >= index)
                {
                    //Add the range to the list.
                    tL.Add(range[m]);
                    if (!(n <= 0))
                        n--;
                }
                else
                {
                    var x = arr[n];
                    tL.Add(x);
                }
                if (m < range.Length)
                    m++;
                if (n < arr.Length)
                    n++;
            }
            return tL.ToArray();
        }

        public static T[] AddRange<T>(T[] arr, T[] range)
        {
            List<T> tL = new List<T>(arr.Length + range.Length);
            foreach (T t in arr)
                tL.Add(t);
            foreach (T r in range)
                tL.Add(r);
            return tL.ToArray();
        }

        public static bool IsNull(string str)
            => str == null;

        public static bool IsNullOrEmpty(string str)
            => IsNull(str) || str == "";

        public static bool IsNullOrWhiteSpace(string str)
            => IsNull(str) || str == " ";

        public static bool IsNullWhiteSpaceOrEmpty(string str)
            => IsNullOrEmpty(str) || IsNullOrWhiteSpace(str);

        public static string ArrayToString(string[] strArr)
        {
            StringBuilder b = new StringBuilder();
            foreach (string str in strArr)
            {
                if (b.Length > 0)
                    b.Append($" {str}");
                else
                    b.Append(str);
            }
            return b.ToString();
        }
    }
}
