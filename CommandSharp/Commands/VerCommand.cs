/* VerCommand.cs
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandSharp.Commands
{
    /* public class VerCommand : Command
    {
        private static readonly CommandData data = new CommandData("@--ver", "Displays the version of the operating system.");

        private int os = 0;

        public VerCommand() : base(data) 
        {
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
                os = 0;
            else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux))
                os = 1;
            else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX))
                os = 2;
            else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.FreeBSD))
                os = 3;
            else
                os = -1;
        }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            /* var args = e.Arguments;
            if (args.IsEmpty)
            {
                //Get the os information.
                if (os == 0)
                {
                    //Is Windows.
                    var x = Environment.OSVersion;
                    var str = x.VersionString;
                    Console.WriteLine(str);
                }
                else if (os == 1 || os == 3)
                {
                    //Is linux or FreeBSD.
                    //Get the 'os-release' file at /etc/os-release
                    var x = "/etc/os-release";
                    x = Path.GetFullPath(x);
                    var f = File.ReadAllText(x);
                    var spl = f.Split("\n");

                }
            }
            return true; //Do not throw a syntax error.
        }
    } */
}
