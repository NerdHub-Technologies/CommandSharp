using System;
using System.Collections.Generic;

namespace CommandSharp
{
    // EventArgs dependency removed for NativeAOT / Cosmos Gen3 compatibility.
    public class CommandInvokeParameters
    {
        public CommandArguments Arguments { get; internal set; }
        public CommandInvoker Invoker { get; internal set; }
        public CommandPrompt Prompt { get; internal set; }
    }
}
