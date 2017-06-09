# Leoxia.Shell
Cross platform command interpreter. Written in C#/.NET Core.
Primarily intended to work on Windows.
Not tested on Linux/Mac but should work as well.

<img src="images/lxsh.png" />

## Navigation on command Line

- Use Ctrl+A, Home, Ctrl+E, End, Left Arrow, Right Arrow to navigate
- Ctrl+K, Backspace to delete characters.

## History

- Can access history of commands with up arrow, down arrow, Ctrl+P, Ctrl+N.
- Can modify current command or history without losing current command.

## Run executables in $PATH / %PATH%

- Search in path for the exe (add .exe on Windows).

## Capture Standard Output / Standard error

- Standard output/error is captured from child process and displayed in console.

## Environment Variables Expansion

Expand `${VARIABLE}` `$VARIABLE` `%VARIABLE%`

## Builtins

The following builtins are already implemented

- echo
- cd 
- mkdir
- ls (options -l, -a, --color)
- exit

## Alias management

- aliases are saved and loaded in `<directory_of_assembly>/.lxaliases`
- aliases are expanded on the commandline

## TODO

- History save between runs
- History search 
- Completion
- Standard Input redirection
- Redirection operators < | > 
- Prompt customization

## Development

In order to compile the projects, you'll need the following

- [Visual Studio 2017 Preview 2](https://www.visualstudio.com/vs/preview/)
- [.NET Core Preview 2](https://www.microsoft.com/net/core/preview#windowscmd)
