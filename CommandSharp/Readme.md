# CommandSharp
Built for:`.NET6.0` `.NET 5.0` `.NET Core 3.1` `.NET Standard 2.1` and `.NET Framework 4.7.1`
 
[![NuGet](https://img.shields.io/nuget/v/CommandSharp?logoColor=black&style=flat-square)](https://www.nuget.org/packages/CommandSharp/) [![Downloads](https://img.shields.io/nuget/dt/CommandSharp?color=teal&logoColor=black&style=flat-square)](https://www.nuget.org/api/v2/package/CommandSharp/2.9.4) [![Issues](https://img.shields.io/bitbucket/issues/nerdhubtech/commandsharp?color=gold&logoColor=black&style=flat-square)](https://bitbucket.org/nerdhubtech/commandsharp/issues?status=new&status=open)

---
[Nuget Repository](https://www.nuget.org/packages/CommandSharp) | [BitBucket Repository](http://www.bitbucket.org/nerdhubtech/commandsharp/src/master) | [Documentation](https://docs.nerdhub.net/CommandSharp) | [Issues](http://www.bitbucket.org/nerdhubtech/commandsharp/issues) | [Send a Message](https://www.nuget.org/packages/CommandSharp/2.9.4/ContactOwners)

---

CommandSharp is a command processing API for .NET built with C#.

Look below for documentation on how to implement *CommandSharp* into your Console project.

### Table of contents:

 - Getting Started
 - Implement `CommandPrompt`
	 - Internal Commands
 - Creating Commands
	 - `CommandData` Parameters
 - Registering Commands

---

### Getting Started
Firstly before you can setup *CommandSharp* you must add a reference to your project. For this example, we'll show you how to install with the "dotnet CLI" since the dotnet CLI is cross-platform.

Use the command: `dotnet add package CommandSharp.Net` this will add *CommandSharp* to your project.

Next you need to add a `using` directive, add the following to your project class: `using CommandSharp`, once the directive is added you're all setup and ready to go. Look at the next sections below to utilize *CommandSharp*.

### Implement `CommandPrompt`

In-order to use *CommandSharp* we need to create an instance of the `CommandPrompt` class, which will show a prompt where the user can enter text.

In your main method,  create a variable named `prompt` and call the `CommandPrompt` constructor.
```CSharp
var prompt = new CommandPrompt();
```
The `CommandPrompt` class has only 2 Constructors.
The first constructor is empty.
```csharp
new CommandPrompt();
```

The second utilizes a custom `CommandInvoker` class, which parses, processes and invokes the commands.

```csharp
CommandInvoker(bool parseQuotes = true, bool iqnoreInnerQuotes = false)
```

`CommandInvoker` at this time has only 2 parameters:

 1. *`parseQuotes`*: (default: true) If true, the invoker will process any data inside unescaped quotes as it's own argument. This ignores the spaces inside the quotes.
 2. *`iqnoreInnerQuotes`*: (default: false) If false any escaped quotes `\"` will be left alone leaving a quote, if true the escaped quotes are removed. 
 
Other then setting these parameters, utilizing a custom  invoker is not necessary as one is created automatically when a prompt is initialized.

If all default settings are left alone, when the application is run  you will see a custom colored message which looks like:

![CommandSharp Echo Output](https://www.nerdhub.net/assets/images/CommandSharp_EchoPrompt.png)

Neat right?! I think so. Best of all, that message can be customized, even the color(s)! Right now, we can only process internal commands, since we don't have any custom commands setup.

#### Internal Commands:
 - Help
 - Echo
 - Clear
 - CD
 - LS/DIR (WIP)
 - Version (Rewritten, will be re-added in next release. This was done to include OS-Information in the version command.)

The next section explains how to create a command and register said command with the calling invoker.

### Creating Commands

For this example we'll create a single command called `Hello` it will say hello to the user.

Create a class and make sure to extend the `Command` class. Be sure to provide the `Command` class' constructor with `CommandData` not doing so will throw an error.

There's numerous parameters that can be passed into `CommandData` that will affect how the command is viewed and processed. 

We'll use basic parameters for now. Format your 'Hello' class like the following:

```csharp
using System;
using System.Linq;
using System.Text;
using CommandSharp;

namespace MyConsoleProject
{
	private static readonly CommandData data = new CommandData("hello", "Say hello to the user.", new string[] { "hi" });
	
	public sealed class HelloCommand : Command(data)
	{
		
	}
}
```

The readonly field we applied to the command:

```csharp
private static readonly CommandData data = new CommandData("hello", "Say hello to the user.", new string[] { "hi" });
```
says that the commands' name is "hello" and it's alias is "hi", so that either "hello" or "hi" can be passed and the `Hello` Command will still be called (or invoked).

#### `CommandData` Parameters:
(Parameter names are Case-Sensitive.)

 - `name`: Set's the name of the command.
 - `description:` The description of the command which is displayed in the default help command.
 - `cmdAliases`: Set's any aliases of the command. (Alternate names.)
 - `hideCommand`: Hides a command from the internal `Help` command. This can also be achieved by adding a '`#`', '`@`', '`.`', and '`!`' at the start of the value in the `name` parameter.
 - `developer`: The developer of the command. This information is displayed in the command-specific help.

Override the `OnInvoke(CommandInvokeParameters e)` method. This is the ONLY method that is required by the base `Command` class. Other methods, such as `OnSyntaxError(SyntaxErrorParameters e)`, are optional.

So, now overriding the `OnInvoke(CommandInvokeParameters e)` method your class should look similar to the following HelloCommand class.

```csharp
using System;
using System.Linq;
using System.Text;
using CommandSharp;

namespace MyConsoleProject
{
	private static readonly CommandData data = new CommandData("hello", "Say hello to the user.", new string[] { "hi" });
	public sealed class HelloCommand : Command(data)
	{
		//This was the method that was overridden
		public override void OnInvoke(object sender, CommandInvokedEventArgs e)
		{
			//Anything that would be ran on invoke would be placed here.
		}
	}
}
```

Next, say hello to the user. You can pass information to the console by using `Console.WriteLine()` or by invoking the `Echo` command. See the wiki for information on how to manually invoke an existing command.

Completing the command, your code should look like the following:

```csharp
using System;
using System.Linq;
using System.Text;
using CommandSharp;

namespace MyConsoleProject
{
	private static readonly CommandData data = new CommandData("hello", "Say hello to the user.", new string[] { "hi" });
	public sealed class HelloCommand : Command(data)
	{
		public override void OnInvoke(object sender, CommandInvokedEventArgs e)
		{
			//Say hello to the user.
			Console.WriteLine("Hello from CommandSharp!");
		}
	}
}
```

That concludes the creation of the `Hello` command, all that's left to do is register the command manually by registering it with the `CommandInvoker` (this step will be automated in a future version so long as the `CommandData` class is utilized by a command.)

For a reference point to create your commands off of, see the [ExampleCommand]() class on the CommandSharp BitBucket repository.

Continue reading to learn how to use the command.

### Registering Commands

Now, we need to register the commands in-order for the commands to be recognized by the invoker. To do this, go back to your program class and create a variable called 'invoker' that calls `prompt.GetInvoker()`. Then, use the new variable to call `Register()` and pass the instance of your command(s) into that class.

Here's an example on registering the `HelloCommand`

```csharp
var invoker = prompt.GetInvoker();
invoker.Register(new HelloCommand());
```

Any commands that are registered with the invoker will be visible to the invoker, thus when a command is called it will be invoked.

Well, that's it for the main tutorial. For more information, please see the wiki or checkout the additional files at the end of this welcome file.

---
For more information see the Wiki. For API reference see the Docs.