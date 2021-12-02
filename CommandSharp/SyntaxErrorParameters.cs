/* SyntaxErrorParameters.cs
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CommandSharp
{
    //FOR COSMOS PORT, REMOVE EVENTARGS
    public class SyntaxErrorParameters : EventArgs
    {
        public string CommandNamePassed { get; internal set; }
        internal string[] GetLegendArr() => new string[]
        { 
            "Legend:",
            "<>: Required Argument",
            "[]: Optional Argument",
            "|: One OR the other"
        };

        internal int GetLongestLine(string[] lines)
        {
            string longestLine = "";
            for (int i = 0; i < lines.Length; i++)
            {
                var x = lines[i];
                for (int j = 0; j < lines.Length; j++)
                {
                    var y = lines[j];
                    if (y.Length >= x.Length)
                        longestLine = y;
                    else if (x.Length >= x.Length)
                        longestLine = x;
                    else
                        continue;
                }
            }
            return longestLine.Length;
        }

        public string GetLegend(out int length)
        {
            List<string> sL = new List<string>();
            var n = GetLegendArr();
            var x = GetLongestLine(n);
            foreach (string s in n)
            {
                sL.Add(s);
            }
            StringBuilder b = new StringBuilder();
            for (int i = 0; i < sL.Count; i++)
            {
                if (i == (sL.Count - 1))
                    b.Append(sL);
            }
            length = x;
            return b.ToString();
        }
    }
}
