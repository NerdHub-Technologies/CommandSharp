/* GlobalSettings.cs
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
    public static class GlobalSettings
    {
        public static ConsoleColor DefaultForeColor { get; set; } = ConsoleColor.White;
        public static ConsoleColor DefaultBackColor { get; set; } = ConsoleColor.DarkBlue;
        public static ConsoleColor CurrentForeColor
        {
            get => Console.ForegroundColor;
            set => Console.ForegroundColor = value;
        }
        public static ConsoleColor CurrentBackColor
        {
            get => Console.BackgroundColor;
            set => Console.BackgroundColor = value;
        }
        public static bool AcceptEchoOut { get; set; } = true;
#if DEBUG
        public static bool OutputDebugData { get; set; } = false;
#endif
        private static string currUsr = Environment.UserName;
        private static string sysName = Environment.MachineName;

        public static string CurrentUser
        {
            get => currUsr;
            set => currUsr = value;
        }

        public static string CurrentDirectory
        {
            get => Directory.GetCurrentDirectory();
            set => Environment.CurrentDirectory = Path.GetFullPath(value);
        }

        public static string MachineName
        {
            get => sysName;
            set => sysName = value;
        }
    }
}
