using CommandSharp;
using CommandSharp.Commands;

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO;
using System;

public sealed class Program
{
    public static void Main()
        => new Program().Run();

    private CommandPrompt prompt;

    public Program()
    {
        prompt = new CommandPrompt();
        prompt.DefaultBackColor = ConsoleColor.Black;
        prompt.DefaultForeColor = ConsoleColor.White;
        //register commands.
        var invoker = prompt.GetInvoker();
        invoker.Register(new Command[] 
        { 
            new DesminCommands.VerCommand(),
            new DesminCommands.ExitCommand(),
            new DesminCommands.ColorCommand(),
            new DesminCommands.ShellCommand(),
        });
    }

    public void Run()
    {
        //Load the prompt.
        prompt.Prompt(true);
    }

    public static bool IsWindows() => System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows);

    public static bool IsLinux() => System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux);

    public static bool IsOSX() => System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX);
}

public sealed class DesminCommands
{
    //Include ver and winver
    public sealed class VerCommand : Command
    {
        //Rather then writing two seperate commands for windows and all non-windows systems, I checked if the os is windows and set the name as "winver" for windows, and "ver" for all others.
        private static readonly CommandData data = new CommandData((Program.IsWindows()) ? "winver" : "ver", "", (Program.IsWindows()) ? new string[] { "version", "ver" } : new string[] { "version" }, hideCommand: true);

        public VerCommand() : base(data) {}

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            //The arguments passed.
            var args = e.Arguments;
            //Get the version of the current os.
            var isWindows = Program.IsWindows();
            var isMac = Program.IsOSX();
            if (isWindows || isMac)
            {
                var x = Environment.OSVersion.VersionString;
                Console.WriteLine(x);
            }
            else
            {
                var n = "/etc/os-release";
                var x = File.OpenText(Path.GetFullPath(n)).ReadToEnd();
                var nSpl = x.Split("\n");
                Dictionary<string, string> vals = new Dictionary<string, string>();
                foreach (var _x in nSpl)
                {
                    var _iSpl = _x.Split('=');
                    var name = _iSpl[0];
                    var value = _iSpl[1];
                    if (value.StartsWith("\"") && value.EndsWith("\""))
                    {
                        value = value.Remove(0, 1);
                        value = value.Remove(value.Length - 1, 1);
                    }

                    vals.Add(name, value);
                }

                var vName = vals["NAME"];
                var vVer = vals["VERSION"];
                var result = $"{vName} {vVer}";
                Console.WriteLine(result);
            }
            return true;
        }
    }
    public sealed class ExitCommand : Command
    {
        private static readonly CommandData data = new CommandData("exit", "Exits desmin.", new string[] { "stop", "quit", "end" });

        public ExitCommand() : base(data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            //Perform an exit.
            Environment.Exit(0);
            return true;
        }
    }
    public sealed class ColorCommand : Command
    {
        private static readonly CommandData data = new CommandData("color", "Allows changing of the console foreground and background colors.", new string[] { "col", "cols", "colors", "colour", "colours" });

        public ColorCommand() : base(data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            //Arg format: [name] [arg1] [arg2] [arg3] [arg4] [arg5]
            var args = e.Arguments;
            if (args.IsEmpty || (args.StartsWithSwitch('l') || args.StartsWithSwitch("list")))
            {
                foreach (var kvp in ColorMap.GetColorMap().GetColors())
                {
                    WriteColorAndText(kvp.Value, kvp.Key.ToString());
                }
                return true;
            }
            else
            {
                if (args.ContainsSwitch("--foreground") || args.ContainsSwitch('f'))
                {
                    //Getting the argument at position 1 would get the value. Alternativly, getting the value after --foreground or -f would have the same effect.
                    var x = args.GetArgumentAfterSwitch("--foreground");
                    if (Utilities.IsNullWhiteSpaceOrEmpty(x))
                        x = args.GetArgumentAfterSwitch('f');
                    //If x is null by this point, throw the syntax.
                    if (Utilities.IsNullWhiteSpaceOrEmpty(x))
                        return false;
                    //Get the console color of the name or ident of the color.
                    var col = ColorMap.GetColorMap()[x];
                    //Set the fg color.
                    if (!(col == null))
                        Console.ForegroundColor = (ConsoleColor)col;
                }
                else if (args.ContainsSwitch("--background") || args.ContainsSwitch('b'))
                {
                    //Getting the argument at position 1 would get the value. Alternativly, getting the value after --foreground or -f would have the same effect.
                    var x = args.GetArgumentAfterSwitch("--foreground");
                    if (Utilities.IsNullWhiteSpaceOrEmpty(x))
                        x = args.GetArgumentAfterSwitch('f');
                    //If x is null by this point, throw the syntax.
                    if (Utilities.IsNullWhiteSpaceOrEmpty(x))
                        return false;
                    //Get the console color of the name or ident of the color.
                    var col = ColorMap.GetColorMap()[x];
                    //Set the fg color.
                    if (!(col == null))
                        //NOTE: The background will not be applied to the entire console unless the console is cleared.
                        Console.BackgroundColor = (ConsoleColor)col;
                    //If -n is passed do NOT clear the console.
                    //If n is not anywhere in the array list.
                    if (!args.ContainsSwitch('n'))
                        Console.Clear();
                    return true;
                }
                else
                    return false;
            }
            return true;
        }

        public override string OnSyntaxError(SyntaxErrorParameters e)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"{e.CommandNamePassed} <options>");
            builder.AppendLine($"-f: The value following this argument sets the foreground color of the console.");
            builder.AppendLine($"-b: The value following this argument sets the background color of the console. (Note: To change the background color of the entire terminal window, the console must be cleared. Use -n to skip clearing the console, however, this may cause issues.)");
            builder.AppendLine($"-l | --list: Lists the color identifiers and names that can be used to change the color. When passing the value into -f or -b the id or the name can be used");
            return builder.ToString();
        }

        private void WriteColorAndText(ConsoleColor color, string text)
        {
            var x = Console.BackgroundColor;
            Console.Write("|");
            Console.BackgroundColor = color;
            Console.Write("  ");
            Console.BackgroundColor = x;
            Console.Write("| ");
            Console.WriteLine(text);
        }

        public sealed class ColorMap
        {
            public sealed class ColorIdentifier
            {
                public readonly string name;
                public readonly string identifier;

                internal ColorIdentifier(string identifier, string name)
                {
                    this.name = name;
                    this.identifier = identifier;
                }

                public string GetName() => name;
                public string GetIdentifier() => identifier;

                public override string ToString()
                    => $"{GetIdentifier().ToUpper()}: {GetName().ToLower()}";
            }

            private Dictionary<ColorIdentifier, ConsoleColor> colors = null;

            ColorMap()
            {
                colors = new Dictionary<ColorIdentifier, ConsoleColor>();
                colors.Add(new ColorIdentifier("0", "black"), ConsoleColor.Black);
                colors.Add(new ColorIdentifier("1", "blue"), ConsoleColor.Blue);
                colors.Add(new ColorIdentifier("2", "green"), ConsoleColor.Green);
                colors.Add(new ColorIdentifier("3", "cyan"), ConsoleColor.Cyan);
                colors.Add(new ColorIdentifier("4", "red"), ConsoleColor.Red);
                colors.Add(new ColorIdentifier("5", "purple"), ConsoleColor.DarkMagenta);
                colors.Add(new ColorIdentifier("6", "yellow"), ConsoleColor.Yellow);
                colors.Add(new ColorIdentifier("7", "silver"), ConsoleColor.Gray);
                colors.Add(new ColorIdentifier("8", "gray"), ConsoleColor.DarkGray);
                colors.Add(new ColorIdentifier("9", "gold"), ConsoleColor.DarkYellow);
                colors.Add(new ColorIdentifier("A", "magenta"), ConsoleColor.Magenta);
                colors.Add(new ColorIdentifier("B", "teal"), ConsoleColor.DarkCyan);
                colors.Add(new ColorIdentifier("C", "dark_blue"), ConsoleColor.DarkBlue);
                colors.Add(new ColorIdentifier("D", "dark_green"), ConsoleColor.DarkGreen);
                colors.Add(new ColorIdentifier("E", "dark_red"), ConsoleColor.DarkRed);
                colors.Add(new ColorIdentifier("F", "white"), ConsoleColor.White);
            }

            public ConsoleColor? this[string nameOrIdent]
            {
                get
                {
                    foreach (var kvp in colors)
                    {
                        if (kvp.Key.GetName().Equals(nameOrIdent) || kvp.Key.identifier.Equals(nameOrIdent))
                            return kvp.Value;
                        else continue;
                    }
                    return null;
                }
            }

            public IReadOnlyDictionary<ColorIdentifier, ConsoleColor> GetColors() => colors;

            public static ColorMap GetColorMap() => new ColorMap();
        }
    }
    public sealed class ShellCommand : Command
    {
        private static readonly CommandData data = new CommandData("shell", "Runs a shell file.", new string[] { "sh", "dsh", "duskshell", "desminshell" });

        public ShellCommand() : base(data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            var args = e.Arguments;
            if (args.IsEmpty)
                return false;
            else
            {
                if (e.Arguments.Count == 1)
                {
                    var path = Path.GetFullPath(args.GetArgumentAtPosition(0));
                    CommandSharp.ShellParser.ShellParser parser = new CommandSharp.ShellParser.ShellParser(e.Invoker);
                    parser.ParseShell(path, "\r\n", forceDSH: true);
                    return true;
                }
                else
                    return false;
            }
        }
    }
    public sealed class UtilsTestCommand : Command
    {
        private static readonly CommandData data = new CommandData("utils", "", hideCommand: true);

        public UtilsTestCommand() : base(data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            var args = e.Arguments;
            if (!(args.IsEmpty) && args.StartsWith("insert"))
            {
                //Insert At Tests.
                return true;
            }
            return false;
        }
    }
}