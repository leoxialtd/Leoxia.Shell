using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography.X509Certificates;
using Leoxia.Abstractions.IO;

namespace Leoxia.Commands
{
    public class Ls : IBuiltin
    {
        private readonly IConsole _console;
        private readonly IDirectory _directory;
        private readonly IFileSystemInfoFactory _factory;

        public Ls(IConsole console, 
            IDirectory directory, IFileSystemInfoFactory factory)
        {
            _console = console;
            _directory = directory;
            _factory = factory;
        }

        public void Execute(List<string> tokens)
        {
            try
            {
                var showHidden = false;
                var showColors = false;
                var showList = false;
                var inError = false;
                var options = tokens.Where(x => x.StartsWith("-")).ToList();
                foreach (var option in options)
                {
                    if (IsShortOption(option))
                    {
                        var trimmed = option.TrimStart('-');
                        foreach (var c in trimmed)
                        {
                            if (c == 'l')
                            {
                                showList = true;
                            }
                            else if (c == 'a')
                            {
                                showHidden = true;
                            }
                            else
                            {
                                _console.Error.WriteLine("ls: unknown option: " + c);
                                inError = true;
                            }
                        }
                    }
                    else if (IsLongOption(option))
                    {
                        var trimmed = option.TrimStart('-');
                        if (trimmed == "color")
                        {
                            showColors = true;
                        }
                        else
                        {
                            _console.Error.WriteLine("ls: unknown option: " + option);
                            inError = true;
                        }
                    }
                    else
                    {
                        _console.Error.WriteLine("ls: unknown option: " + option);
                        inError = true;
                    }
                }
                if (inError)
                {
                    return;
                }
                tokens = tokens.Where(t => !t.StartsWith("-")).ToList();
                if (tokens.Count == 0)
                {
                    DirectoryListing(_directory.GetCurrentDirectory(), showHidden, showColors, showList);
                }
                else
                {
                    foreach (var directory in tokens)
                    {
                        DirectoryListing(directory, showHidden, showColors, showList);
                    }
                }
                _console.WriteLine();
            }
            catch (Exception e)
            {
                _console.Error.WriteLine(e.Message);
            }
        }

        private bool IsShortOption(string option)
        {
            return option.Length > 1 && option[1] != '-';
        }

        private bool IsLongOption(string option)
        {
            return option.Length > 2 && option[2] != '-';
        }

        private void DirectoryListing(string directory, bool showHidden, bool showColors, bool showList)
        {
            var extendedEntries = _directory.GetFileSystemEntries(directory).Select(x => _factory.Build(x))
                .Select(x => new ExtendedEntryInfo(x, showList)).ToList();
            if (showList)
            {
                var rightsPadding = extendedEntries.Max(x => x.Rights.Length) + 1;
                var childPadding = extendedEntries.Max(x => x.ChildNumber.ToString().Length) + 1;
                var ownerPadding = extendedEntries.Max(x => x.Owner.Length) + 1;
                var groupPadding = extendedEntries.Max(x => x.Group.Length) + 1;
                var sizePadding = extendedEntries.Max(x => x.Size.ToString().Length) + 1;
                var datePadding = extendedEntries.Max(x => x.Date.Length) + 1;
                foreach (var extendedEntry in extendedEntries)
                {
                    extendedEntry.RightsPadding = rightsPadding;
                    extendedEntry.ChildPadding = childPadding;
                    extendedEntry.OwnerPadding = ownerPadding;
                    extendedEntry.GroupPadding = groupPadding;
                    extendedEntry.SizePadding = sizePadding;
                    //extendedEntry.DatePadding = datePadding;
                }
            }
            foreach (var entry in extendedEntries)
            {
                string qualifier = string.Empty;
                ConsoleColor entryColor = _console.ForegroundColor;
                if (showColors)
                {
                    entryColor = entry.GetColor(out qualifier, _console.ForegroundColor, _factory);
                }
                if (showHidden || !entry.Info.IsHidden())
                {
                    Write(entryColor, entry, qualifier, showList);
                }
            }
        }

        public void Write(ConsoleColor color, ExtendedEntryInfo entry, string qualifier, bool showList)
        {
            if (showList)
            {
                _console.Write(entry + " ");                
            }
            var separator = "  ";
            var savedColor = _console.ForegroundColor;
            _console.ForegroundColor = color;
            _console.Write(entry.Info.Name);
            _console.ForegroundColor = savedColor;
            if (!string.IsNullOrEmpty(qualifier))
            {
                _console.Write(qualifier);
            }
            if (showList)
            {
                _console.WriteLine();
            }
            else
            {
                _console.Write(separator);
            }
        }
    }
}