/* ListDirectoryCommand.cs
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
    public sealed class ListDirectoryCommand : Command
    {
        private static readonly CommandData data = new CommandData("ls", "Lists all files and folders in the current directory", new string[] { "dir", "listDir" });

        public ListDirectoryCommand() : base(data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            var args = e.Arguments;
            if (args.IsEmpty)
            {
                StringBuilder builder = new StringBuilder();
                var ct = Directory.GetCreationTime(Path.GetFullPath(Directory.GetCurrentDirectory())).ToString("MM/dd/yyyy  hh:mm:ss tt");
                var ctd = Directory.GetParent(Directory.GetCurrentDirectory());
                var ctn = "";
                if (ctd != null)
                    ctn = ctd.FullName;
                builder.AppendLine($"{ct}                     .");
                builder.AppendLine($"{ct}                     ..");
                int fileLength = 0;
                int dirLength = 0;
                var dirx = Directory.GetDirectories(Directory.GetCurrentDirectory(), "*.*", SearchOption.TopDirectoryOnly);
                var filex = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.*", SearchOption.TopDirectoryOnly);
                for (int i = 0; i < dirx.Length; i++)
                {
                    var path = Path.GetFullPath(dirx[i]);
                    try
                    {
                        var par = Directory.GetParent(path);
                        var name = (par != null) ? ExcludeParentDir(path, par.FullName) : path;
                        var tc = Directory.GetCreationTime(path).ToString("MM/dd/yyyy  hh:mm:ss tt");
                        builder.AppendLine($"{tc}        <DIR>        {name}");
                    }
                    catch
                    {

                    }
                }
                foreach (string path in filex)
                {
                    try
                    {
                        var ctx = File.GetCreationTime(path).ToString("MM/dd/yyyy  hh:mm:ss tt").ToUpper();
                        var bytes = File.ReadAllBytes(path);
                        var sizeInBytes = bytes.Length;
                        var name = Path.GetFileName(path);
                        builder.AppendLine($"{ctx}                     {name}");
                        fileLength++;
                    }
                    catch
                    {

                    }
                }

                builder.AppendLine($"    {fileLength} File(s)");
                builder.AppendLine($"    {dirLength} Dir(s)");

                Console.WriteLine(builder);
            }
            else
            {
                if (args.StartsWithSwitch('w') || args.StartsWithSwitch("--wide"))
                {
                    StringBuilder b = new StringBuilder();
                    string s = "";
                    int i = 0;
                    Console.WriteLine("Not done yet, sorry.");
                }
            }
            return true;
        }

        private string ExcludeParentDir(string dir, string parentDir)
        {
            dir = dir.Remove(0, parentDir.Length);
            if (dir.StartsWith("/") || dir.StartsWith("\\"))
                dir = dir.Remove(0, 1);
            return dir;
        }
    }

    
}
