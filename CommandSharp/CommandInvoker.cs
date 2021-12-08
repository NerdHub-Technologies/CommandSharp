/* CommandInvoker.cs
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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CommandSharp
{
    public sealed class CommandInvoker
    {
        private CommandPrompt prompt;
        private List<Commands.Command> commands = null;
        private bool parseQuotes = true, iqnoreInnerQuotes = false;

        #region Ctor and Internal Register

        public CommandInvoker(bool parseQuotes = true, bool iqnoreInnerQuotes = false, bool registerMinimalCommands = false)
        {
            commands = new List<Commands.Command>();

            this.parseQuotes = parseQuotes;
            this.iqnoreInnerQuotes = iqnoreInnerQuotes;

            RegisterInternalCommands(registerMinimalCommands);
        }

        private void RegisterInternalCommands(bool registerMinimal)
        {
            if (registerMinimal)
            {
                Register(new Command[]
                {
                    new HelpCommand(),
                    new ClearCommand()
                });
            }
            else
            {
                Register(new Command[]
            {
                new HelpCommand(),
                new EchoCommand(),
                new ClearCommand(),
                new ChangeDirectoryCommand(),
                new ExampleCommand(),
                new ListDirectoryCommand()
                //new DataCalculatorCommand()
            });
            }
        }

        #endregion

        #region Register Functions

        /// <summary>
        /// Register a command.
        /// </summary>
        /// <param name="command">The command to register.</param>
        public void Register(Command command)
        {
            if (!CommandExists(command) && (GetCommand(command.GetName()) == null))
                //Register Command.
                commands.Add(command);
        }

        /// <summary>
        /// Register an array of commands.
        /// </summary>
        /// <param name="commands">The array of commands to register.</param>
        public void Register(Command[] commands)
        {
            foreach (Command cmd in commands)
                Register(cmd);
        }

        /// <summary>
        /// Register a list of commands.
        /// </summary>
        /// <param name="commands">The list of commands to register.</param>
        public void Register(List<Command> commands)
            => Register(commands.ToArray());

        #endregion

        #region Unregister Functions

        /// <summary>
        /// Unregister a command.
        /// </summary>
        /// <param name="command">The command to unregister.</param>
        public void Unregister(Command command)
        {
            if (CommandExists(command))
                commands.Remove(command);
        }

        /// <summary>
        /// Unregister an array of commands.
        /// </summary>
        /// <param name="commands">The array of commands to unregister.</param>
        public void Unregister(Command[] commands)
        {
            foreach (Command cmd in commands)
                Unregister(cmd);
        }

        /// <summary>
        /// Unregister a list of commands.
        /// </summary>
        /// <param name="commands">The list of commands to unregister.</param>
        public void Unregister(List<Command> commands)
            => Unregister(commands.ToArray());

        #endregion

        #region Override Functions

        /// <summary>
        /// Override the command with the specified name or alias with the command specified.
        /// </summary>
        /// <param name="oldCommand">The command with the name or alias to replace.</param>
        /// <param name="newCommand">The replacement command.</param>
        public void Override(string oldCommand, Command newCommand)
        {
            Command cmd = GetCommand(oldCommand);
            if (CommandExists(cmd))
                Override(cmd, newCommand);
        }

        /// <summary>
        /// Explicitly overrides a command by the instance with another command.
        /// </summary>
        /// <param name="oldCommand">The command to replace.</param>
        /// <param name="newCommand">The replacement.</param>
        public void Override(Command oldCommand, Command newCommand)
        {
            var index = IndexOf(oldCommand);
            if (index <= -1)
                throw new Exception("Could not continue, index is outside the bounds of the command registry.");
            if (CommandExists(oldCommand) && !CommandExists(newCommand)) //We don't want a duplicate value of newCommand.
                commands[index] = newCommand; //Inject the newCommand at the index of oldCommand.
        }

        #endregion

        #region Checks, and Command Getters

        /// <summary>
        /// Checks if a command exists.
        /// </summary>
        /// <param name="command">The command to check.</param>
        /// <returns>Returns <see langword="true"/>if the command was found.</returns>
        public bool CommandExists(Command command)
        {
            if (command == null)
                return false;
            foreach (Command c in GetCommands())
            {
                if (c == command)
                    return true;
                else
                    continue;
            }
            return false;
        }

        /// <summary>
        /// Gets a command at the specified index. (Returns -1 if the command does not exist.)
        /// </summary>
        /// <param name="command">The command to check.</param>
        /// <returns>The id of the specified command.</returns>
        public int IndexOf(Command command)
        {
            for (int i = 0; i < GetCommands().Length; i++)
            {
                Command cmd = commands[i];
                if (CommandExists(command) && (cmd == command))
                    return i;
                else
                    continue;
            }
            return -1;
        }

        /// <summary>
        /// Gets an array of all commands that were registered with this <see cref="CommandInvoker"/>
        /// </summary>
        /// <returns>An array of commands.</returns>
        public Command[] GetCommands() => commands.ToArray();

        /// <summary>
        /// Gets a command with the specified name. (The value will be null if the command does not exist.)
        /// </summary>
        /// <param name="name">The name of the command.</param>
        /// <returns>The command with the specified name.</returns>
        public Command GetCommandByName(string name)
        {
            foreach (Command c in GetCommands())
            {
                if (c.GetName().ToLower().Equals(name.ToLower()))
                {
                    return c;
                }
                else
                    continue;
            }
            return null;
        }

        /// <summary>
        /// Gets a command with the specfied alias. (The value will be null if the command does not exist.)
        /// </summary>
        /// <param name="alias">The alias of the command..</param>
        /// <returns>The command with the specified name.</returns>
        public Command GetCommandByAlias(string alias)
        {
            Command c = null;
            foreach (Command cmd in GetCommands())
            {
                var aliases = cmd.GetAliases();
                if ((aliases != null) && aliases.Length > 0)
                {
                    foreach (string a in aliases)
                    {
                        bool isNull = Utilities.IsNullWhiteSpaceOrEmpty(a);
                        if ((!isNull) && a.ToLower().Equals(alias.ToLower()))
                        {
                            c = cmd;
                            break;
                        }
                        else continue;
                    }
                }
                else continue;
            }
            return c;
        }

        /// <summary>
        /// Gets the command with the specified value. The value can be a name or alias. (The result will be null if the command with the name or alias does not exist.)
        /// </summary>
        /// <param name="value">The name or alias of the command.</param>
        /// <returns>The command with the specified name or alias.</returns>
        public Command GetCommand(string value)
        {
            Command cname = GetCommandByName(value);
            Command calias = GetCommandByAlias(value);

            bool nameNull = cname == null;
            bool aliasNull = calias == null;

            if (!nameNull && aliasNull)
                return cname;
            else if (nameNull && !aliasNull)
                return calias;
            else if (!nameNull && !aliasNull)
                return cname;
            else
                return null;
        }

        #endregion

        #region Invokation Functions

        /// <summary>
        /// Parse and invoke a string containing a command and all arguments.
        /// </summary>
        /// <param name="input">The string of a command or arguments.</param>
        public void Invoke(string input)
        {
            string name = "";
            string[] rawArgs = null;

            //Split for only the name, then ignore remaining values.
            name = Utilities.Split(input, " ")[0];

            //Get the arguments
            rawArgs = Utilities.Skip(ParseArguments(input), 1);

            //Search for instance of cmd.
            Command cmd = GetCommand(name);

            //Init args.
            CommandArguments args = new CommandArguments(rawArgs);

            ManualInvoke(cmd, args, name);
        }

        /// <summary>
        /// Explicitly invokes a command with the explicitly specified command instance and arguments.
        /// </summary>
        /// <param name="command">The instance of the command to invoke.</param>
        /// <param name="args">The arguments to pass.</param>
        /// <param name="inputName">The name used that represents the command. (Do not pass unless you know what this is for.)</param>
        public void ManualInvoke(Command command, CommandArguments args, string inputName = "")
        {
            if (CommandExists(command))
            {
                if ((!args.IsEmpty) && args.StartsWith("help") && !command.GetName().ToLower().Equals("help"))
                {
                    //Forward to help.
                    ManualInvoke(GetCommand("help"), new CommandArguments(new string[] { command.GetName(), "-s" }), inputName ?? command.GetName());
                    return;
                }

                CommandInvokeParameters @params = new CommandInvokeParameters()
                {
                    Prompt = GetPrompt(),
                    Invoker = this,
                    Arguments = args
                };

                var x = command.OnInvoke(@params);
                if (!x)
                {
                    //If false.
                    var syntax = RequestSyntax(command, inputName ?? command.GetName());
                    Console.WriteLine(syntax);
                }
            }
            else
            {
                var x = NoCommandMessage;
                x = x.Replace("%input%", inputName ?? command.GetName());
                Console.WriteLine(x);
            }
        }

        #endregion

        #region Parser Functions

        /// <summary>
        /// Parses a string and splits any data in quotes if any. Parses the string using spaces if no quotes either " or escaped \" exist.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>An array of data split either by quotes or by spaces.</returns>
        internal string[] ParseArguments(string input)
        {
            if (parseQuotes && (input.Contains("\"") || input.Contains("\\\"")))
            {
                List<string> tokens = new List<string>();
                string currentToken = "";
                bool isInQuotes = false;

                for (int i = 0; i < input.Length; i++)
                {
                    var @char = input[i];
                    if (@char == '\"')
                    {
                        if (!isInQuotes)
                            isInQuotes = true;
                        else
                        {
                            if (currentToken.Length > 0)
                                tokens.Add(currentToken);
                            currentToken = "";
                            isInQuotes = false;
                        }
                    }
                    else if ((@char == '\\' && isInQuotes) && !iqnoreInnerQuotes)
                    {
                        var nCharI = input.IndexOf(@char);
                        var nChar = input[nCharI];
                        if (nChar == '\"')
                        {
                            currentToken += "$[&quote];";
                            //Remove the next value.
                            input = input.Remove(i, 1);
                            continue;
                        }
                        else continue;
                    }
                    else
                    {
                        if (@char == ' ' && !isInQuotes)
                        {
                            if (currentToken.Length > 0)
                                tokens.Add(currentToken);
                            currentToken = "";
                        }
                        else
                            currentToken += @char;
                    }
                }

                isInQuotes = false;

                //Flush currentToken if it's not null.
                if (currentToken.Length > 0)
                    tokens.Add(currentToken);
                currentToken = ""; //Clear the token.

                //Replace the inner quotes now that parsing is done.
                if (!iqnoreInnerQuotes)
                {
                    for (int i = 0; i < tokens.Count; i++)
                    {
                        if (tokens[i].Contains("$[&quote];"))
                            tokens[i] = tokens[i].Replace("$[&quote];", "\"");
                    }
                }

                return tokens.ToArray();
            }
            else
            {
                //Split only spaces as no quotes exist.
                var x = Utilities.Split(input, " ");
                return x;
            }
        }

        #endregion

        #region Misc Functions

        public string RequestSyntax(Commands.Command command, string inputName = null)
        {
            if (command == null)
                throw new NullReferenceException("Cannot get the syntax of a null command.");
            SyntaxErrorParameters @params = new SyntaxErrorParameters()
            {
                CommandNamePassed = inputName ?? command.GetName()
            };

            StringBuilder synt = new StringBuilder();
            var n = @params.GetLegend(out int length);
            if (IncludeLegendInSyntax)
                synt.AppendLine(new string('-', length));
            if (IncludeLegendInSyntax)
                synt.AppendLine(n);
            var syntax = command.OnSyntaxError(@params);
            if (!Utilities.IsNullWhiteSpaceOrEmpty(syntax))
            {
                if (IncludeLegendInSyntax)
                    synt.AppendLine(new string('-', length));
                synt.Append(syntax);
            }
            if (!Utilities.IsNullWhiteSpaceOrEmpty(syntax))
                return synt.ToString();
            else
                return "";
        }

        internal void SetPrompt(CommandPrompt prompt)
            => this.prompt = prompt;

        public CommandPrompt GetPrompt() => prompt;

        #endregion

        #region Optional Properties

        /// <summary>
        /// The message that is displayed when an unknown command is passed.
        /// '%input%': is the input value passed.
        /// </summary>
        public string NoCommandMessage { get; set; } = "\"%input%\", is not a valid command.";

        /// <summary>
        /// If true, a syntax legend is displayed when the syntax is requested.
        /// </summary>
        public bool IncludeLegendInSyntax { get; set; } = true;

        #endregion
    }
}
