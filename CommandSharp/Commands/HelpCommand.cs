/* HelpCommand.cs
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
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandSharp.Commands
{
    public sealed class HelpCommand : Command
    {
        private static readonly CommandData data = new CommandData("help", "Displays information on commands or of a specific command.", new string[] { "?" });

        public HelpCommand() : base(data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            var args = e.Arguments;
            if (args.IsEmpty)
            {
                //Display all commands.
                StringBuilder b = new StringBuilder();
                foreach (Command c in e.Invoker.GetCommands())
                {
                    if (c.IsCommandHidden)
                        continue;
                    if (b.Length > 0)
                        b.Append($"{Environment.NewLine}{c.GetName()}: {c.GetDescription()}");
                    else
                        b.Append($"{c.GetName()}: {c.GetDescription()}");
                }
                Console.WriteLine(b.ToString());
            }
            else if (!args.IsEmpty && (args.StartsWithSwitch('w') || args.StartsWithSwitch("wide")))
            {
                //Display wide mode.
                StringBuilder b = new StringBuilder();
                string s = "";
                int i = 0;
                foreach (Command c in e.Invoker.GetCommands())
                {
                    if (c.IsCommandHidden)
                        continue;
                    i++;
                    if (i == 5)
                    {
                        if (b.Length > 0)
                        {
                            b.Append($"\n{s}");
                            s = "";
                            i = 0;
                            s = c.GetName();
                        }
                        else
                        {
                            b.Append(s);
                            s = "";
                            i = 0;
                            s = c.GetName();
                        }
                    }
                    else
                    {
                        //Append s.
                        if (s.Length > 0)
                            s += $", {c.GetName()}";
                        else
                            s = c.GetName();
                    }
                }
            }
            else
            {
                var cx = args.GetArgumentAtPosition(0);
                var cmd = e.Invoker.GetCommand(cx);
                var syntOnly = args.ContainsSwitch('s');
                if (cmd != null)
                {
                    //Command exits.
                    if (syntOnly)
                        Console.WriteLine(e.Invoker.RequestSyntax(cmd, cx));
                    else
                    {
                        StringBuilder builder = new StringBuilder();
                        var s = $"==========| {cmd.GetName()} |==========";
                        var synt = e.Invoker.RequestSyntax(cmd, cx);
                        bool printDesc = !Utilities.IsNullWhiteSpaceOrEmpty(cmd.GetDescription());
                        bool printSyntax = !Utilities.IsNullWhiteSpaceOrEmpty(synt);
                        var a = cmd.GetAliases();
                        bool printAliases = !(a.Length < 1 || a == null);
                        builder.AppendLine(s);
                        if (printDesc)
                            builder.AppendLine("Description: " + cmd.GetDescription());
                        if (printAliases)
                            builder.AppendLine("Aliases: " + GetStringFromArray(a));
                        if (printSyntax)
                            builder.AppendLine(synt);
                        builder.Append(new string('=', s.Length));
                        Console.WriteLine(builder.ToString());
                    }
                }
                else
                    return false;
            }
            return true;
        }

        public override string OnSyntaxError(SyntaxErrorParameters e)
        {
            return $"{e.CommandNamePassed} [[-w | --wide] | [command | command -s]]";
        }

        private string GetStringFromArray(string[] array)
        {
            StringBuilder b = new StringBuilder();
            foreach (string s in array)
            {
                if (b.Length > 0)
                    b.Append($", {s}");
                else
                    b.Append(s);
            }
            return b.ToString();
        }
    }
}
