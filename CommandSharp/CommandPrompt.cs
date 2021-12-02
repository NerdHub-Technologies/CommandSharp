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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace CommandSharp
{
    public sealed class CommandPrompt
    {
        /// <summary>
        /// Defines the forecolor to set when the prompt is loaded for the first time.
        /// </summary>
        public ConsoleColor DefaultForeColor
        {
            get => GlobalSettings.DefaultForeColor;
            set => GlobalSettings.DefaultForeColor = value;
        }

        /// <summary>
        /// Defines the backcolor to set when the prompt is loaded for the first time.
        /// </summary>
        public ConsoleColor DefaultBackColor
        {
            get => GlobalSettings.DefaultBackColor;
            set => GlobalSettings.DefaultBackColor = value;
        }

        /// <summary>
        /// Denotes the current forecolor set to the console.
        /// </summary>
        public ConsoleColor CurrentForeColor
        {
            get => GlobalSettings.CurrentForeColor;
            set => GlobalSettings.CurrentForeColor = value;
        }

        /// <summary>
        /// Denotes the current backcolor set to the console.
        /// </summary>
        public ConsoleColor CurrentBackColor
        {
            get => GlobalSettings.CurrentBackColor;
            set => GlobalSettings.CurrentBackColor = value;
        }

        /// <summary>
        /// Denotes whether a echo (excluding echo commands) will be returned to the console.
        /// </summary>
        public bool AcceptEchoOut
        {
            get => GlobalSettings.AcceptEchoOut;
            set => GlobalSettings.AcceptEchoOut = value;
        }

#if DEBUG
        public bool OutputDebugData
        {
            get => GlobalSettings.OutputDebugData;
            set => GlobalSettings.OutputDebugData = value;
        }
#endif

        /// <summary>
        /// Get or set the current user.
        /// </summary>
        public string CurrentUser
        {
            get => GlobalSettings.CurrentUser;
            set => GlobalSettings.CurrentUser = value;
        }

        /// <summary>
        /// Get or set the current directory.
        /// </summary>
        public string CurrentDirectory
        {
            get => GlobalSettings.CurrentDirectory;
            set => GlobalSettings.CurrentDirectory = Path.GetFullPath(value);
        }

        /// <summary>
        /// Get or set the name of the machine.
        /// </summary>
        public string MachineName
        {
            get => GlobalSettings.MachineName;
            set => GlobalSettings.MachineName = value;
        }

        public EchoMessage EchoMessage { get; set; } = new EchoMessage(
            MessageNode.NewMessageNode("["),
            MessageNode.NewMessageNode("$", MessageNode.USERNAME.GetMessageColor()),
            MessageNode.USERNAME,
            MessageNode.NewMessageNode("@", MessageNode.MACHINE_NAME.GetMessageColor()),
            MessageNode.MACHINE_NAME,
            MessageNode.NewMessageNode("]: "),
            MessageNode.CURRENT_DIRECTORY,
            MessageNode.NewMessageNode(" > ")
            );

        private readonly string rawMsg = $"[$%user%@%sys%]: %path% > ";

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
            if (Utilities.IsNullWhiteSpaceOrEmpty(CurrentUser))
                CurrentUser = Environment.UserName;
            if (Utilities.IsNullWhiteSpaceOrEmpty(MachineName))
                CurrentUser = Environment.MachineName;
            if (Utilities.IsNullWhiteSpaceOrEmpty(CurrentDirectory))
                CurrentDirectory = Environment.CurrentDirectory;
            if (EchoMessage == null)
                EchoMessage = new EchoMessage(
            MessageNode.NewMessageNode("["),
            MessageNode.NewMessageNode("$", MessageNode.USERNAME.GetMessageColor()),
            MessageNode.USERNAME,
            MessageNode.NewMessageNode("@", MessageNode.MACHINE_NAME.GetMessageColor()),
            MessageNode.MACHINE_NAME,
            MessageNode.NewMessageNode("]: "),
            MessageNode.CURRENT_DIRECTORY,
            MessageNode.NewMessageNode(" > ")
            );
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
                System.Diagnostics.Debug.WriteLine(errMsg);
            }
        }

        /// <summary>
        /// Show a prompt and accept command input.
        /// </summary>
        /// <param name="loop">Defines whether looping is handled by the prompt or by another process or thread.</param>
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
                EchoMessage.Display();
            //Accept input.
            var input = Console.ReadLine();
            if (!Utilities.IsNullWhiteSpaceOrEmpty(input))
                invoker.Invoke(input);
            else
                return;
        }

        private string SetCur(string currentDirectory)
        {
            currentDirectory = Path.GetFullPath(currentDirectory);
            Environment.CurrentDirectory = currentDirectory;
            return Environment.CurrentDirectory;
        }

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
