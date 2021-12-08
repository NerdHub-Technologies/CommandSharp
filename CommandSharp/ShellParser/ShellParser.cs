using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandSharp.ShellParser
{
    //NOTE: As of version 2.15.0 the shell parser only supports parsing of commands and some support for goto. (Goto in 2.15 only will call a line number and not a label or method.)
    //TODO: Create a new invoker that has special commands that aren't exposed to the normal command intr. Such as GOTO, OUTPUT, INPUT, METHOD. For any of these keywords call the special intr, else forward to normal command intr.
    public class ShellParser
    {
        private CommandInvoker invoker;
        private CommandInvoker keywordInvoker;

        public ShellParser(CommandInvoker invoker)
        {
            this.invoker = invoker;
            //Create the keyword invoker.
            keywordInvoker = new CommandInvoker();
            //Assign the keywords to the keyword invoker.
            keywordInvoker.Register(new Commands.Command[] 
            { 
                
            });
            parsedData = new List<string>();
        }

        private List<string> parsedData;

        //For now, keep things simple.
        public void ParseShell(string path, string EOL = "\n", bool forceDSH = false)
        {
            System.IO.Path.GetFullPath(path);
            var ext = System.IO.Path.GetExtension(path);
            if (forceDSH && ext != ".dsh")
                path = path.Replace(ext, ".dsh");
            else
                return;

            var data = System.IO.File.ReadAllText(path);
            var spl = Utilities.Split(data, EOL);
            foreach (var line in spl)
            {
                if (!Utilities.IsNullWhiteSpaceOrEmpty(line))
                    parsedData.Add(line);
            }

            //Handle the check and invokation.

        }
    }

    public sealed class KeywordCommands
    {
        private List<KeyValuePair<string, Commands.Command>> keywords;

        public KeywordCommands()
        {
            keywords = new List<KeyValuePair<string, Commands.Command>>();

        }

        public string[] GetKeywords()
        {
            List<string> s = new List<string>();
            foreach (var kvp in keywords)
            {
                if (!Utilities.IsNullWhiteSpaceOrEmpty(kvp.Key))
                    s.Add(kvp.Key);
            }
            return s.ToArray();
        }

        private bool KeyExists(string key)
            => Utilities.Contains(GetKeywords(), key);

        /* public Commands.Command this[string key]
        {
            get
            {
                if (!Utilities.IsNullWhiteSpaceOrEmpty(key.ToLower()) && KeyExists(key.ToLower()))
                {
                    
                }    
            }
        } */

        public sealed class GotoKeywordCommand : Commands.Command
        {
            private static readonly CommandData data = new CommandData("goto", "Takes the parser to a specific line or label.");

            private ShellParser parser;

            public GotoKeywordCommand(ShellParser parser) : base(data)
            {
                if (parser == null)
                    throw new NullReferenceException("The shell parser cannot be null.");
                if (this.parser == null)
                    this.parser = parser;
            }

            public override bool OnInvoke(CommandInvokeParameters e)
            {
                throw new NotImplementedException();
            }
        }
    }
}
