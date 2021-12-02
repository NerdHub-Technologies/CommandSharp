/* CommandArguments.cs
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
    //TODO: Add --key:value and --key=value optionals.
    //TODO: Add "multi-set switch" -xfzd which is essentially -x -f -z -d
    public class CommandArguments
    {
        private List<string> arguments;

        #region Ctors

        //TODO: Have the Ctor(s) check for chained switches. If there's a chained switch then:
        /*
         * Get the index of the chained switch. 
         * Remove the chained switch from the array.
         * Replaced the chained values with short switch values.
         * 
         * Example:
         * -abc
         * BECOMES:
         * -a -b -c
         * 
         * Verify the existance of a chained switch by checking for a short switch that is longer then 1.
         */

        public CommandArguments(string[] arguments)
        {
            if ((arguments != null) && (arguments.Length <= 0 || arguments == new string[0]))
                this.arguments = new List<string>(0);
            else
                this.arguments = new List<string>();
            if (arguments != null)
                this.arguments.AddRange(arguments);

            //if (!IsEmpty)
            //    MapChainedSwitches();
        }

        public CommandArguments(List<string> arguments)
        {
            if (arguments == null)
                this.arguments = new List<string>(0);
            else
                this.arguments = arguments;

            //if (!IsEmpty)
            //    MapChainedSwitches();
        }

        #endregion

        #region General Functions

        /// <summary>
        /// Gets the number of arguments present.
        /// </summary>
        public int Count
            => arguments.Count;

        /// <summary>
        /// Denotes whether arguments are present.
        /// </summary>
        public bool IsEmpty
            => arguments.Count <= 0;

        /// <summary>
        /// Gets a command at the specified argument position as it was processed.
        /// </summary>
        /// <param name="position">The position of an argument.</param>
        /// <returns>The argument at the specified position.</returns>
        public string GetArgumentAtPosition(int position)
        {
            if (!IsEmpty)
                return arguments[position];
            else return "";
        }

        /// <summary>
        /// Gets a list of all argumetns passed to the specified command.
        /// </summary>
        /// <returns></returns>
        public string[] GetArguments() => arguments.ToArray();

        /// <summary>
        /// Get an array of switches from the argument array. (This only searches for short and long-named switches, chained are directly excluded.)
        /// </summary>
        /// <returns></returns>
        public string[] GetSwitches()
        {
            List<string> switches = new List<string>();
            foreach (string s in GetArguments())
            {
                var c1 = s[0];
                var c2 = s[1];
                if (c1 == '-' || (c1 == '-' && c2 == '-'))
                //-a or --a
                {
                    switches.Add(s);
                }
                else continue;
            }
            return switches.ToArray();
        }

        #endregion

        #region Checks

        /// <summary>
        /// Checks if a long-named switch is at the specified position in the argument array.
        /// </summary>
        /// <param name="position">The position in the array.</param>
        /// <param name="s">The long-named switch.</param>
        /// <returns><see langword="true"/>if the switch at the postion exists.</returns>
        public bool IsSwitchAtPosition(int position, string s)
        {
            var x = GetArgumentAtPosition(position);
            return ((x != null) && x == $"--{s}");
        }

        /// <summary>
        /// Checks if a short-named switch is at the specified position.
        /// </summary>
        /// <param name="position">The position in the array.</param>
        /// <param name="c">The short-named swich.</param>
        /// <returns><see langword="true"/>if the switch at the position exists.</returns>
        public bool IsSwitchAtPosition(int position, char c)
        {
            var x = GetArgumentAtPosition(position);
            return ((x != null) && x == $"-{c}");
        }

        /// <summary>
        /// Checks if an argument was passed to a specific command.
        /// </summary>
        /// <param name="arg">The argument to verify.</param>
        /// <returns>True, if the argument was found.</returns>
        public bool ContainsArgument(string arg)
        {
            if (!(IsEmpty))
            {
                foreach (string s in arguments)
                {
                    if (s.Equals(arg))
                        return true;
                    else
                        continue;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if a 'long named switch <c>--help</c>' was passed to a command.
        /// </summary>
        /// <param name="s">The switch value.</param>
        /// <returns>True, if the switch was found.</returns>
        public bool ContainsSwitch(string s)
            => !IsEmpty && ContainsArgument($"--{s}");

        /// <summary>
        /// Checks if a 'short named switch <c>-h</c>' was passed to a command.
        /// </summary>
        /// <param name="c">The switch value.</param>
        /// <returns>True, if the switch was found.</returns>
        public bool ContainsSwitch(char c)
            => !IsEmpty && ContainsArgument($"-{c}");

        /// <summary>
        /// Checks if a variable '$variable or %variable' was passed to a command.
        /// </summary>
        /// <param name="v">The variable.</param>
        /// <returns>True, if the variable was found.</returns>
        public bool ContainsVariable(Variable v)
            => !IsEmpty && ContainsArgument(v.ToString());

        #endregion

        #region StartsWith Functions

        /// <summary>
        /// Checks if an argument is at the begining of the argument array.
        /// </summary>
        /// <param name="arg">The argument.</param>
        /// <returns>True, if the argument is the first argument in the argument array.</returns>
        public bool StartsWith(string arg)
            => !IsEmpty && GetArgumentAtPosition(0).Equals(arg);

        /// <summary>
        /// Checks if a 'long named switch <c>--help</c>' is at the begining of the argument array.
        /// </summary>
        /// <param name="s">The switch.</param>
        /// <returns>True, if the switch is the first argument in the argument array.</returns>
        public bool StartsWithSwitch(string s)
            => !IsEmpty && StartsWith($"--{s}");

        /// <summary>
        /// Checks if a 'short named switch <c>-h</c>' is at the begining of the argument array.
        /// </summary>
        /// <param name="c">The switch.</param>
        /// <returns>True, if the argument is at the begining of the array.</returns>
        public bool StartsWithSwitch(char c)
            => !IsEmpty && StartsWith($"-{c}");

        /// <summary>
        /// Checks if a variable '$VARIABLE or %variable%' is at the begining of the argument array.
        /// </summary>
        /// <param name="v">The variable.</param>
        /// <returns>True, if the variable is the first argument in the argument array.</returns>
        public bool StartsWithVariable(Variable v)
            => !IsEmpty && StartsWith(v.ToString());

        #endregion

        #region EndsWith Functions

        /// <summary>
        /// Checks if an argument is at the end of the argument array.
        /// </summary>
        /// <param name="arg">The argument</param>
        /// <returns>True, if the argument is the last argument in the argument array.</returns>
        public bool EndsWith(string arg)
        {
            int index = Count - 1;
            if (index < 0)
                index = 0;

            return !IsEmpty && GetArgumentAtPosition(index).Equals(arg);
        }

        /// <summary>
        /// Checks if a 'long named swith <c>--help</c>' is at the end of the argument array.
        /// </summary>
        /// <param name="s">The value of the switch.</param>
        /// <returns>True, if the switch is the last argument in the argument array.</returns>
        public bool EndsWithSwitch(string s)
            => !IsEmpty && EndsWith($"--{s}");

        /// <summary>
        /// Checks if a 'short named switch <c>-c</c>' is at the end of the argument array.
        /// </summary>
        /// <param name="c">The switch.</param>
        /// <returns>True, if the switch is at the end of the argument array.</returns>
        public bool EndsWithSwitch(char c)
            => !IsEmpty && EndsWith($"-{c}");

        /// <summary>
        /// Checks if a variable '$VARIABLE or %variable%' is at the end of the argument array.
        /// </summary>
        /// <param name="v">The variable.</param>
        /// <returns>True, if the variable is at the end of the argument array.</returns>
        public bool EndsWithVariable(Variable v)
            => !IsEmpty && EndsWith(v.ToString());

        #endregion

        #region IndexOf Functions

        /// <summary>
        /// Gets the index (position) of the argument within the argument array.
        /// </summary>
        /// <param name="arg">The argument.</param>
        /// <returns>A value based on the argumets position in the argument array if the argument was found.</returns>
        public int IndexOfArgument(string arg)
        {
            if (!IsEmpty)
            {
                for (int i = 0; i < Count; i++)
                {
                    var x = GetArgumentAtPosition(i);
                    if (x.Equals(arg))
                        return i;
                    else
                        continue;
                }
            }
            return -1;
        }

        /// <summary>
        /// Gets the index (position) of a 'long named switch <c>--help</c>' within the argument array.
        /// </summary>
        /// <param name="s">The switch value.</param>
        /// <returns>A value based on the switch's position within the argument array if the switch was found.</returns>
        public int IndexOfSwitch(string s)
            => !IsEmpty ? IndexOfArgument($"--{s}") : -1;

        /// <summary>
        /// Gets the index (position) of a 'short named switch <c>-c</c>' within the argument array.
        /// </summary>
        /// <param name="c">The switch value.</param>
        /// <returns>A value based on the switch's position within the argument array if the switch was found.</returns>
        public int IndexOfSwitch(char c)
            => !IsEmpty ? IndexOfArgument($"-{c}") : -1;

        /// <summary>
        /// Gets the index (position) of a variable '$VARIABLE or %variable%' within the argument array.
        /// </summary>
        /// <param name="v">The variable</param>
        /// <returns>A value based on the variable's position within the argument array if the variable was found.</returns>
        public int IndexOfVariable(Variable v)
            => !IsEmpty ? IndexOfArgument(v.ToString()) : -1;

        #endregion

        #region Misc Functions

        /// <summary>
        /// Gets an argument that comes after the specified 'long named switch <c>--help</c>' in the argument array.
        /// </summary>
        /// <param name="s">The switch.</param>
        /// <returns>An argument that is found at the position after the specified switch.</returns>
        public string GetArgumentAfterSwitch(string s)
        {
            int index = IndexOfSwitch(s);
            if (index == -1)
                index = 0;
            index += 1; //increase by one.
            if (index >= Count)
                index = Count;
            return GetArgumentAtPosition(index);
        }
        /// <summary>
        /// Gets an argument that comes after the specified 'short named switch <c>-h</c>' in the argument array.
        /// </summary>
        /// <param name="c">The switch.</param>
        /// <returns>An argument that is found at the position after the specified switch.</returns>
        public string GetArgumentAfterSwitch(char c)
        {
            int index = IndexOfSwitch(c);
            index++;
            if (index >= Count)
                index = Count;
            return GetArgumentAtPosition(index);
        }

        #endregion

        #region Internal Functions and Methods

        //private string[] GetAllChainedSwitches()
        //{
        //    List<string> chains = new List<string>();
        //    //Iterate through ALL arguments.
        //    foreach (var arg in GetArguments())
        //    {
        //        var c1 = arg[0];
        //        var c2 = arg[1];

        //        if (((c1 == '-') && !(c2 == '-')) && arg.Length > 2)
        //        {
        //            chains.Add(arg);
        //        }
        //        else
        //            continue;
        //    }
        //    return (!(chains == null) && chains.Count > 0) ? chains.ToArray() : new string[0];
        //}

        //private KeyValuePair<int, string[]>[] GetChainedValues()
        //{
        //    List<KeyValuePair<int, string[]>> vars = new List<KeyValuePair<int, string[]>>();
        //    var x = GetAllChainedSwitches();
        //    System.Diagnostics.Debug.WriteLine($"Chains: {Utilities.ArrayToString(x)}");
        //    if (x.Length <= 0)
        //        return new KeyValuePair<int, string[]>[0];
        //    else
        //    {
        //        for (int i = 0; i < x.Length; i++)
        //        {
        //            var val = x[i];
        //            var ix = val.Length;
        //            List<string> lx = new List<string>(ix);
        //            foreach (char c in val)
        //            {
        //                if (c != '-' && !(c == ' ' || c == '"' || c == '\\'))
        //                {
        //                    lx.Add($"-{c}");
        //                }
        //                else
        //                    continue;
        //            }
        //            if (lx.Count > 0)
        //                vars.Add(new KeyValuePair<int, string[]>(i, lx.ToArray()));
        //            else
        //                continue;
        //        }
        //    }

        //    return (vars.Count > 0) ? vars.ToArray() : new KeyValuePair<int, string[]>[0];
        //}

        //private void MapChainedSwitches()
        //{
        //    foreach (var kvp in GetChainedValues())
        //    {
                
        //    }
        //}

        //Get the actual chains.
        private string[] GetChains()
        {
            //-abcd would be a chain.
            if (!IsEmpty)
            {
                List<string> chains = new List<string>();
                var x = GetSwitches();
                foreach (var sw in x)
                {
                    //Check if the switch is a short-named switch, or if it's a long named switch.
                    var c1 = sw[0];
                    var c2 = sw[1];

                    if ((c1 == '-' && !(c2 == '-')) && sw.Length > 2)
                    {
                        chains.Add(sw);
                    }
                    else continue;
                }
                System.Diagnostics.Debug.WriteLine($"");
                if (chains.Count > 0)
                    return chains.ToArray();
            }
            return new string[0];
        }

        private KeyValuePair<int, string[]>[] GetChainResults()
        {
            List<KeyValuePair<int, string[]>> values = new List<KeyValuePair<int, string[]>>();
            var chains = GetChains();
            for (int i = 0; i < chains.Length; i++)
            {
                var x = chains[i];
                var c1 = x[0];
                var c2 = x[1];
                if ((c1 == '-' && c2 != '-') && x.Length > 2)
                {
                    List<string> sL = new List<string>();
                    foreach (char c in x)
                    {
                        sL.Add($"-{c}");
                    }
                    if (sL.Count > 0)
                        values.Add(new KeyValuePair<int, string[]>(i, sL.ToArray()));
                    else
                        continue;
                }
                else
                    continue;
            }
            return new KeyValuePair<int, string[]>[0];
        }

        private void MapChainedSwitches()
        {
            var x = GetChainResults();
            Dictionary<int, List<string>> chainedResults = new Dictionary<int, List<string>>();
            foreach (var x_ in x)
            {
                List<string> s_ = new List<string>();
                var i = x_.Key;
                foreach (string s in x_.Value)
                {
                    s_.Add(s);
                }
                chainedResults.Add(i, s_);
            }

            
        }

        #endregion

        /// <summary>
        /// Defines a variable.
        /// </summary>
        public sealed class Variable
        {
            private VariableType variableType;
            private string variable = "var";

            /// <summary>
            /// Creates a new instance of a variable.
            /// </summary>
            /// <param name="variableType">The defining type of variable.</param>
            /// <param name="variable">The name of the variable.</param>
            public Variable(VariableType variableType, string variable)
            {
                this.variableType = variableType;
                this.variable = variable;
            }
            /// <summary>
            /// The pysical name of the variable.
            /// </summary>
            /// <returns>The name of the variable.</returns>
            public string GetVariableString() => variable;
            /// <summary>
            /// The entire variable.
            /// </summary>
            /// <returns>The entire variable.</returns>
            public override string ToString()
            {
                if (variableType == VariableType.WIN_VARIABLE)
                    return $"%{variable}%";
                else
                    return "${" + variable + "}";
            }

            /// <summary>
            /// Represents the type of variables that can be used.
            /// </summary>
            public enum VariableType
            {
                /// <summary>
                /// Represents a windows-like variable. <c>%variable%</c>
                /// </summary>
                WIN_VARIABLE,
                /// <summary>
                /// Represents a *nix-like variable. <c>$VARIABLE</c>
                /// </summary>
                LINUX_VARIABLE
            }
        }
    }
}
