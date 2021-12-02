@echo off
cls
color 1f

Echo "Publishing Windows"
Echo "Windows-x86"
dotnet publish Desmin.csproj -c Release -f net6.0 -r "win-x86" --self-contained true --force --nologo --verbosity m
Echo "Windows-x64"
dotnet publish Desmin.csproj -c Release -f net6.0 -r "win-x64" --self-contained true --force --nologo --verbosity m
Echo "Windows-arm"
dotnet publish Desmin.csproj -c Release -f net6.0 -r "win-arm" --self-contained true --force --nologo --verbosity m
Echo "Windows-arm64"
dotnet publish Desmin.csproj -c Release -f net6.0 -r "win-arm64" --self-contained true --force --nologo --verbosity m

Echo "Publishing Linux"
Echo "Linux-x64"
dotnet publish Desmin.csproj -c Release -f net6.0 -r "linux-x64" --self-contained true --force --nologo --verbosity m
Echo "Linux-arm"
dotnet publish Desmin.csproj -c Release -f net6.0 -r "linux-arm" --self-contained true --force --nologo --verbosity m
Echo "Linux-arm64"
dotnet publish Desmin.csproj -c Release -f net6.0 -r "linux-arm64" --self-contained true --force --nologo --verbosity m
Echo "Linux-MUSL-arm"
dotnet publish Desmin.csproj -c Release -f net6.0 -r "linux-musl-arm" --self-contained true --force --nologo --verbosity m

Echo "Publishing Mac OSX"
Echo Sierra or Heigher"
dotnet publish Desmin.csproj -c Release -f net6.0 -r "osx-x64" --self-contained true --force --nologo --verbosity m

Echo "Done."