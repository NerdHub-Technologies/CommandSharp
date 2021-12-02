/* ClearCommand.cs
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
    public class ClearCommand : Command
    {
        private static readonly CommandData data = new CommandData("clear", "Clears the console.", new string[] { "clr", "cls" });

        public ClearCommand() : base(data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            Console.Clear();
            return true;
        }
    }
}
