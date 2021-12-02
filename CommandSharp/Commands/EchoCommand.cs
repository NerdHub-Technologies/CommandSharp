/* EchoCommand.cs
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
using System.Collections.Generic;
using System.Text;

namespace CommandSharp.Commands
{
    public class EchoCommand : Command
    {
        private static readonly CommandData data = new CommandData("echo", "Returns data to the console.");

        public EchoCommand() : base(data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            var args = e.Arguments;
            if (!args.IsEmpty)
            {
                if (args.StartsWith("@off") || args.StartsWithSwitch("off"))
                {
                    //Turn the echo off.
                    var x = e.Prompt;
                    if (x != null)
                        x.AcceptEchoOut = false;
                }
                else if (args.StartsWith("@on") || args.StartsWithSwitch("on"))
                {
                    //Turn the echo on.
                    var x = e.Prompt;
                    if (x != null)
                        x.AcceptEchoOut = true;
                }
                else
                {
                    Console.WriteLine(args.GetArgumentAtPosition(0));
                }    
            }
            return true;
        }

        public override string OnSyntaxError(SyntaxErrorParameters e)
        {
            return $"{e.CommandNamePassed} [message | [@on | @off]]";
        }
    }
}
