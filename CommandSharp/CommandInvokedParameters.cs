/* CommandInvokedParameters.cs
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
    //FOR COSMOS PORT, REMOVE EVENTARGS
    public class CommandInvokeParameters : EventArgs
    {
        public CommandArguments Arguments { get; internal set; }
        public CommandInvoker Invoker { get; internal set; }
        public CommandPrompt Prompt { get; internal set; }
    }
}
