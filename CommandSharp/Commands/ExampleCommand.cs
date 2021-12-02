/* ExampleCommand.cs
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
    /// <summary>
    /// This class can be used as a template for creating additional commands.
    /// </summary>
    public class ExampleCommand : Command
    {
        private static readonly CommandData data = new CommandData("@example", "An template for creating commands.");

        public ExampleCommand() : base(data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            //The arguments that were passed to the invoker.
            var args = e.Arguments;
            //Display a message.
            Console.WriteLine("This is an example command, use it to create additonal commands.");
            //Display all arugments that were passed if any.
            if (!args.IsEmpty)
            {
                StringBuilder builder = new StringBuilder();
                foreach (var a in args.GetArguments())
                {
                    if (builder.Length > 0)
                        builder.Append($"{Environment.NewLine}[{a}]");
                    else
                        builder.Append($"[{a}]");
                }
                Console.WriteLine(builder);
            }
            return true; //When true the syntax will NOT be thrown, when false the syntax will be displayed to the user.
        }

        public override string OnSyntaxError(SyntaxErrorParameters e)
        {
            return ""; //Write your syntax message in 'OnSyntaxError' be sure to return the value.
        }
    }
}
