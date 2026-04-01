/* CommandPrompt.cs
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
using System.Text;

#if !COSMOS
using System.IO;
#endif


namespace CommandSharp
{
    public sealed class CommandPrompt
    {
        private ConsoleColor defaultFGCol = ConsoleColor.White, defaultBGCol = ConsoleColor.Black;

        /// <summary>
        /// Defines the forecolor to set when the prompt is loaded for the first time.
        /// </summary>
        public ConsoleColor DefaultForeColor
        {
            get => defaultFGCol;
            set => defaultFGCol = value;
        }

        /// <summary>
        /// Defines the backcolor to set when the prompt is loaded for the first time.
        /// </summary>
        public ConsoleColor DefaultBackColor
        {
            get => defaultBGCol;
            set => defaultBGCol = value;
        }

        /// <summary>
        /// Denotes the current forecolor set to the console.
        /// </summary>
        public ConsoleColor CurrentForeColor
        {
            get => Console.ForegroundColor;
            set => Console.ForegroundColor = value;
        }

        /// <summary>
        /// Denotes the current backcolor set to the console.
        /// </summary>
        public ConsoleColor CurrentBackColor
        {
            get => Console.BackgroundColor;
            set => Console.BackgroundColor = value;
        }

        private bool acceptEcho = true;

        /// <summary>
        /// Denotes whether a echo (excluding echo commands) will be returned to the console.
        /// </summary>
        public bool AcceptEchoOut
        {
            get => acceptEcho;
            set => acceptEcho = value;
        }

#if DEBUG
        public bool OutputDebugData
        {
            get => GlobalSettings.OutputDebugData;
            set => GlobalSettings.OutputDebugData = value;
        }
#endif

        private string currUsr = "Administrator";

        /// <summary>
        /// Get or set the current user.
        /// </summary>
        public string CurrentUser
        {
            get => currUsr;
            set => currUsr = value;
        }

        // In COSMOS mode there is no file-system; default to "/".
#if COSMOS
        private string currDir = "/";
#else
        private string currDir = Directory.GetCurrentDirectory();
#endif

        /// <summary>
        /// Get or set the current directory.
        /// </summary>
        public string CurrentDirectory
        {
            get
            {
#if COSMOS
                return currDir;
#else
                return currDir ?? Directory.GetCurrentDirectory();
#endif
            }
            set
            {
#if COSMOS
                currDir = value;
#else
                currDir = Path.GetFullPath(value);
                Directory.SetCurrentDirectory(value);
#endif
            }
        }

        /// <summary>
        /// Get or set the name of the machine.
        /// </summary>
        public string MachineName
        {
            get => GlobalSettings.MachineName;
            set => GlobalSettings.MachineName = value;
        }

        private EchoMessage echoMsg = new EchoMessage(
            MessageNode.NewMessageNode("["),
            MessageNode.NewMessageNode("$", MessageNode.USERNAME.GetMessageColor()),
            MessageNode.USERNAME,
            MessageNode.NewMessageNode("@", MessageNode.MACHINE_NAME.GetMessageColor()),
            MessageNode.MACHINE_NAME,
            MessageNode.NewMessageNode("]: "),
            MessageNode.CURRENT_DIRECTORY,
            MessageNode.NewMessageNode(" > ")
            );

        public EchoMessage EchoMessage 
        {
            get => echoMsg;
            set
            {
                if (value == null)
                    throw new NullReferenceException("Cannot set a null Echo Message.");
                echoMsg = value;
            }
        } 

        private CommandInvoker invoker = null;
        public CommandPrompt(CommandInvoker invoker = null)
        {
            if (invoker != null)
                this.invoker = invoker;
            else
                this.invoker = new CommandInvoker();

            this.invoker.SetPrompt(this);
#if DEBUG
            OutputDebugData = false;
#endif
#if !COSMOS
            if (Utilities.IsNullWhiteSpaceOrEmpty(CurrentUser))
                CurrentUser = Environment.UserName;
            if (Utilities.IsNullWhiteSpaceOrEmpty(MachineName))
                MachineName = Environment.MachineName;
            if (Utilities.IsNullWhiteSpaceOrEmpty(CurrentDirectory))
                CurrentDirectory = Environment.CurrentDirectory;
#endif
            if (EchoMessage == null)
            {
                EchoMessage = new EchoMessage(
            MessageNode.NewMessageNode("["),
            MessageNode.NewMessageNode("$", MessageNode.USERNAME.GetMessageColor()),
            MessageNode.USERNAME,
            MessageNode.NewMessageNode("@", MessageNode.MACHINE_NAME.GetMessageColor()),
            MessageNode.MACHINE_NAME,
            MessageNode.NewMessageNode("]: "),
            MessageNode.CURRENT_DIRECTORY,
            MessageNode.NewMessageNode(" > "));
            }
        }

        /// <summary>
        /// Get the invoker that was created by, or specified to the command invoker.
        /// </summary>
        /// <returns><see cref="CommandInvoker"/> the invoker passed or created.</returns>
        public CommandInvoker GetInvoker() => invoker;

        private bool exitLoop = false, doOnce = true;

        /// <summary>
        /// Displays a command prompt. A prompt which accepts command input.
        /// </summary>
        /// <param name="loop">Infinitatly show the command prompt?</param>
        public void Prompt(bool loop = false)
        {
            try
            {
                if (loop)
                    do
                        DisplayPrompt();
                    while (loop && !exitLoop);
                else
                    DisplayPrompt();
            }
            catch (Exception ex)
            {
                var errMsg = $"Unhandled Exception:" + Environment.NewLine + ex.ToString();
                Console.WriteLine(errMsg);
#if !COSMOS
                System.Diagnostics.Debug.WriteLine(errMsg);
#endif
            }
        }

        /// <summary>
        /// Show a prompt and accept command input.
        /// </summary>
        private void DisplayPrompt()
        {
            if (doOnce)
            {
                CurrentBackColor = DefaultBackColor;
                CurrentForeColor = DefaultForeColor;
                Console.Clear();
                doOnce = false;
            }

            if (AcceptEchoOut)
                EchoMessage.Display(this);
            //Accept input.
            var input = Console.ReadLine();
            if (!Utilities.IsNullWhiteSpaceOrEmpty(input))
                invoker.Invoke(input);
            else
                return;
        }

#if !COSMOS
        private string SetCur(string currentDirectory)
        {
            currentDirectory = Path.GetFullPath(currentDirectory);
            Environment.CurrentDirectory = currentDirectory;
            return Environment.CurrentDirectory;
        }
#endif

        /// <summary>
        /// If true, the current prompt will stop being shown.
        /// </summary>
        public bool ExitLoop
        {
            get => exitLoop;
            set => exitLoop = value;
        }
    }
}

