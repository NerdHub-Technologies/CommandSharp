using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using CommandSharp;
using CommandSharp.Commands;

namespace CommandSharp
{
    public sealed class Program
    {
        public static void Main()
            => new Program().Run();

        private CommandPrompt prompt;

        public Program()
        {
            prompt = new CommandPrompt();
            prompt.CurrentUser = "Administrator";
            prompt.DefaultBackColor = ConsoleColor.Black;
            //Get shorter dir.
            var x = Path.GetFullPath(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
            prompt.CurrentDirectory = x;
        }

        public void Run()
        {
            Clear();
            prompt.Prompt(true);
        }

        private void Clear()
            => Console.Clear();

        private void Clear(ConsoleColor color)
        {
            Console.BackgroundColor = color;
            Clear();
        }
    }
}
