/* ChangeDirectoryCommand.cs
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
using System.IO;

namespace CommandSharp.Commands
{
    public sealed class ChangeDirectoryCommand : Command
    {
        private static readonly CommandData data = new CommandData("cd", "Changes to a specified directory.", new string[] { "chgDir" });

        public ChangeDirectoryCommand() : base(data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            var args = e.Arguments;
            var nPath = args.GetArgumentAtPosition(0);
            if (args.IsEmpty)
                return false;
            else
            {
                if (nPath == "~")
                {
                    var x = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                    x = Path.GetFullPath(x);
                    Directory.SetCurrentDirectory(x);
                    return true;
                }
                else if (nPath == "..")
                {
                    //Navigate to the parent directory if any.
                    var curr = Directory.GetCurrentDirectory();
                    var par = Directory.GetParent(Directory.GetCurrentDirectory());
                    if (par != null)
                    {
                        var x = par.FullName;
                        Environment.CurrentDirectory = x;
                    }
                    return true;
                }
                else if (nPath.Equals("."))
                {
                    Environment.CurrentDirectory = Directory.GetCurrentDirectory();
                }
                else
                {
                    if (nPath.StartsWith("~"))
                    {
                        var x = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                        x = Path.GetFullPath(x);
                        if (nPath.StartsWith("~\\") || nPath.StartsWith("~/"))
                            nPath = nPath.Remove(0, 2);

                        var n = Path.Combine(x, nPath);
                        System.Diagnostics.Debug.WriteLine(n);
                        if (!Utilities.IsNullWhiteSpaceOrEmpty(n))
                            Directory.SetCurrentDirectory(n);
                    }
                    else if (nPath.StartsWith(".."))
                    {
                        
                    }
                    else if (nPath.StartsWith("."))
                    {
                        
                    }
                    else
                    {
                        //Check for any immediate child directories.
                        //Then get the full path of the item, and if it's a directory and exists, navigate to it.
                        //Get the current directory.
                        var currDir = Directory.GetCurrentDirectory();
                        currDir = Path.GetFullPath(currDir);
                        //Get all child directories.
                        var dirs = Directory.GetDirectories(currDir, "*", SearchOption.TopDirectoryOnly);
                        foreach (string s in dirs)
                        {
                            var path = Path.GetFullPath(s);
                            var dName = Path.GetDirectoryName(path);
                            if (dName.ToLower().Equals(nPath.ToLower()))
                            {
                                Environment.CurrentDirectory = path;
                                return true;
                            }
                        }
                        var files = Directory.GetFiles(currDir, "*.*", SearchOption.TopDirectoryOnly);
                        foreach (string s in files)
                        {
                            var path = Path.GetFullPath(s);
                            var fName = Path.GetFileName(path);
                            if (fName.ToLower().Equals(nPath.ToLower()))
                                throw new InvalidOperationException($"Cannot navigate to the path \"{fName}\"", new IOException($"The path: \"{path}\", is a file and not a directory, cannot navigate to a file."));
                            
                        }

                        var x = Path.GetFullPath(nPath);
                        //Try to get the full path and explicitly navigate to the directory.
                        if (Directory.Exists(nPath))
                        {
                            //Is directory.
                            Environment.CurrentDirectory = x;
                            return true;
                        }
                        else if (File.Exists(nPath))
                            throw new InvalidOperationException($"Cannot navigate to the path \"{x}\"", new IOException($"The path: \"{x}\", is a file and not a directory, cannot navigate to a file."));
                        else
                            return true;
                    }
                }
            }
            return true;
        }

        public override string OnSyntaxError(SyntaxErrorParameters e)
            => e.CommandNamePassed + " <name | path>";
    }
}

/* Path validation:
 * 
 * Check if the path is a drive (windows) or a root. ('/' unix)
 * Check if the path is a relative token. ('~', '.', '..', '@', or ':')
 * Check if the path is relative and is a child folder. Or a parent folder if starts with '..'.
 * Check if the path is a full path and exists on the system.
 */

/* DuskOS Relative tokens:
 * 
 * '@': Get first file with name. Searches all child directories for the folder of the specified name.
 * ':' Get first file with name. Searches all parent directories for the folder of the specified name.
 * '@:' Search all child and parent directories for the first file with the specified name.
 */