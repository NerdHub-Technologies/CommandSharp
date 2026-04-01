# CommandSharp – Cosmos Gen3 & NativeAOT Migration Guide

This document describes how to use the `cosmos-main` branch of CommandSharp in a
[Cosmos Gen3](https://github.com/CosmosOS/Cosmos) kernel or any
[NativeAOT](https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/) application.

---

## What Changed in `cosmos-main`

| Change | Reason |
|--------|--------|
| Removed `EventArgs` base from `CommandInvokeParameters` | `System.EventArgs` may not be available in minimal Cosmos runtimes |
| Removed `EventArgs` base from `SyntaxErrorParameters` | Same reason (comments already noted this in the original source) |
| Gated `System.IO` usage with `#if !COSMOS` | `System.IO` (File, Directory, Path) is not available in Cosmos Gen3 |
| Gated `Environment.UserName`, `Environment.MachineName`, `Environment.CurrentDirectory`, `Environment.GetFolderPath` with `#if !COSMOS` | These environment APIs are unavailable in Cosmos |
| Gated `System.Diagnostics.Debug.WriteLine` with `#if !COSMOS` | Not available in Cosmos |
| `cd` command returns a graceful message in Cosmos mode | Filesystem navigation requires `System.IO` |
| `ls`/`dir` command returns a graceful message in Cosmos mode | Directory listing requires `System.IO` |
| `ShellParser.ParseShell` throws `NotSupportedException` in Cosmos mode | Requires `System.IO.File` |
| Added `net8.0` to `TargetFrameworks` | NativeAOT requires .NET 7 or later |
| Set `IsAotCompatible=true` for `net8.0` target | Enables AOT analysis and marks the library safe for AOT consumers |
| Removed unused `using` directives | Reduces surface area; helps with environments with limited BCL |

All changes are backward-compatible with non-Cosmos / non-AOT consumers.

---

## Building for NativeAOT (Standard .NET)

Standard NativeAOT publishing targets **net8.0** (or later). Because CommandSharp is a class
library, you do not publish it directly as a NativeAOT binary; instead, the consuming application
is published as NativeAOT and CommandSharp is included as a reference.

### Add reference to your NativeAOT application

```xml
<!-- YourApp.csproj -->
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
    <PublishAot>true</PublishAot>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include="..\CommandSharp\CommandSharp.csproj" />
  </ItemGroup>
</Project>
```

### Publish

```bash
dotnet publish YourApp.csproj -r linux-x64 -c Release
```

There are no reflection-only code paths in CommandSharp's core (no `Expression.Compile`,
no `Assembly.Load`, no `Type.GetType` by string), so the AOT publish produces no trim/AOT
warnings from CommandSharp code.

---

## Building for Cosmos Gen3

### 1. Reference CommandSharp in your Cosmos kernel project

Add a `ProjectReference` (or NuGet reference) to CommandSharp in your Cosmos kernel `.csproj`.

### 2. Define the `COSMOS` symbol

Cosmos kernels are typically built with a custom SDK that defines compilation symbols. You need
to ensure `COSMOS` is defined so that the conditional code is excluded.

**In your kernel `.csproj`:**

```xml
<PropertyGroup>
  <DefineConstants>$(DefineConstants);COSMOS</DefineConstants>
</PropertyGroup>
```

**Or when calling `dotnet build` directly:**

```bash
dotnet build YourKernel.csproj -p:DefineConstants=COSMOS
```

### 3. What works in Cosmos mode

The following features are available when `COSMOS` is defined:

- `CommandInvoker` – full registration and invocation of commands
- `CommandPrompt` – prompt display (without `Console.Clear()` requiring a reset; directory is
  stored as a plain string, default `"/"`)
- `CommandData`, `CommandArguments`, `SyntaxErrorParameters`, `CommandInvokeParameters` –
  unchanged
- `HelpCommand`, `EchoCommand`, `ClearCommand`, `ExampleCommand` – fully functional
- `Utilities` – all string/array helpers

### 4. What is gated behind `#if !COSMOS`

| Feature | Behavior in Cosmos mode |
|---------|------------------------|
| `ChangeDirectoryCommand.OnInvoke` | Prints a "not supported" message and returns `true` |
| `ListDirectoryCommand.OnInvoke` | Prints a "not supported" message and returns `true` |
| `ShellParser.ParseShell` | Throws `NotSupportedException` |
| `CommandPrompt.CurrentDirectory` setter | Sets the stored string only (no `Directory.SetCurrentDirectory`) |
| `CommandPrompt` constructor auto-detecting `Environment.UserName` / `Environment.MachineName` | Skipped; defaults remain `"Administrator"` / `"CommandSharp"` |
| `System.Diagnostics.Debug.WriteLine` in error handler | Skipped |

### 5. Providing filesystem commands for Cosmos

Override the built-in `cd` and `ls` commands (or avoid registering them), and register your own
platform-specific implementations using the `Register` API:

```csharp
var invoker = new CommandInvoker(registerMinimalCommands: true);
invoker.Register(new MyCosmosChangeDirectoryCommand());
invoker.Register(new MyCosmosListDirectoryCommand());
var prompt = new CommandPrompt(invoker);
prompt.CurrentUser = "root";
prompt.MachineName = "MyKernel";
prompt.CurrentDirectory = @"0:\";
prompt.Prompt(loop: true);
```

---

## Breaking Changes

| Area | Change | Migration |
|------|--------|-----------|
| `CommandInvokeParameters` | No longer inherits `EventArgs` | If you cast it to `EventArgs`, remove the cast |
| `SyntaxErrorParameters` | No longer inherits `EventArgs` | Same as above |

The public surface of both types is otherwise identical.

---

## Version

These changes are tracked in the `cosmos-main` branch (library version `2.16.0`).
