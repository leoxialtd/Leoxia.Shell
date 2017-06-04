using System;
using System.Globalization;
using System.IO;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using Leoxia.Abstractions.IO;
using Leoxia.Commands;

namespace Leoxia.Commands
{
    public class ExtendedEntryInfo
    {
        private bool showList;

        public ExtendedEntryInfo(IFileSystemInfo systemInfo, bool showList)
        {
            Info = systemInfo;
            if (showList)
            {
                Rights = systemInfo.GetRightsListing();
                ChildNumber = systemInfo.GetChildNumber();
                Owner = systemInfo.GetOwner();
                Group = systemInfo.GetGroup();
                Size = systemInfo.GetSize();
                Date = systemInfo.GetDate().ToString("MMM dd yyyy", CultureInfo.InvariantCulture);
            }
        }

        public string Date { get; set; }

        public long Size { get; }

        public string Group { get; }

        public string Owner { get; }

        public int ChildNumber { get; }

        public string Rights { get; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append((string) Rights.PadRight(RightsPadding));
            builder.Append((string) ChildNumber.ToString().PadRight(ChildPadding));
            builder.Append((string) Owner.PadRight(OwnerPadding));
            builder.Append((string) Group.PadRight(GroupPadding));
            builder.Append((string) Size.ToString().PadRight(SizePadding));
            builder.Append((string) Date);
            return builder.ToString();
        }

        public int SizePadding { get; set; }

        public int GroupPadding { get; set; }

        public int OwnerPadding { get; set; }

        public int ChildPadding { get; set; }

        public int RightsPadding { get; set; }
        public IFileSystemInfo Info { get; }


        public ConsoleColor GetColor(out string qualifier, ConsoleColor defaultColor, 
            IFileSystemInfoFactory factory, ILinkManager manager)
        {
            qualifier = string.Empty;
            ConsoleColor entryColor = defaultColor;
            string target;
            if (manager.TryGetLink(Info, out target))
            {
                var targetInfo = factory.Build(target);
                if (targetInfo.Exists)
                {
                    entryColor = ConsoleColor.Cyan;
                    qualifier = "@";
                }
                else
                {
                    entryColor = ConsoleColor.Red;
                    qualifier = "@";
                }
            }
            else if (Info.Attributes.HasFlag(FileAttributes.Directory))
            {
                entryColor = ConsoleColor.Blue;
                qualifier = "/";
            }
            else if (Info.Attributes.HasFlag(FileAttributes.System))
            {
                entryColor = ConsoleColor.DarkYellow;
            }
            else if (Info.Attributes.HasFlag(FileAttributes.Compressed) ||
                     Info.Attributes.HasFlag(FileAttributes.Archive) ||
                     Info.IsCompressionExtension())
            {
                entryColor = ConsoleColor.Magenta;
            }
            else if (Info.IsExecutable(ClaimsPrincipal.Current.Identity.Name))
            {
                entryColor = ConsoleColor.Green;
                qualifier = "*";
            }
            return entryColor;
        }
    }
}