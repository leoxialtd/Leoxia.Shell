using System;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using Leoxia.Abstractions.IO;

namespace Leoxia.Commands
{
    public static class FileSystemInfoExtensions
    {

        public static bool IsCompressionExtension(this IFileSystemInfo systemInfo)
        {
            var entryInfoExtension = systemInfo.Extension.ToLowerInvariant().TrimStart('.');
            if (string.IsNullOrEmpty(entryInfoExtension))
            {
                return false;
            }
            return FileExtensions.Compressed.Contains(entryInfoExtension);
        }

        public static bool IsHidden(this IFileSystemInfo systemInfo)
        {
            return systemInfo.Name.StartsWith(".") || systemInfo.Attributes.HasFlag(FileAttributes.Hidden);
        }

        public static int GetChildNumber(this IFileSystemInfo systemInfo)
        {
            if (systemInfo.Attributes.HasFlag(FileAttributes.Directory))
            {
                var directoryInfo = systemInfo as IDirectoryInfo;
                return directoryInfo.GetFileSystemInfos().Length;
            }
            return 1;
        }

        public static long GetSize(this IFileSystemInfo systemInfo)
        {
            if (systemInfo.Attributes.HasFlag(FileAttributes.Directory))
            {
                return 4096;
            }
            var fileInfo = systemInfo as IFileInfo;
            return fileInfo.Length;
        }
        public static DateTime GetDate(this IFileSystemInfo systemInfo)
        {
            return systemInfo.LastWriteTime;
        }

        public static string GetOwner(this IFileSystemInfo systemInfo)
        {
            FileSecurity security = GetFileSecurity(systemInfo);
            return security.GetOwner(typeof(NTAccount)).Value;
        }

        public static string GetGroup(this IFileSystemInfo systemInfo)
        {
            FileSecurity security = GetFileSecurity(systemInfo);
            return security.GetGroup(typeof(NTAccount)).Value;
        }

        public static string GetRightsListing(this IFileSystemInfo systemInfo)
        {
            var builder = new StringBuilder();
            builder.Append((char) GetDirectoryIndication(systemInfo));
            try
            {
                FileSecurity security = GetFileSecurity(systemInfo);
                var owner = security.GetOwner(typeof(NTAccount));
                if (owner == null)
                {
                    
                }
                var ownerRights = new Rights();
                var group = security.GetGroup(typeof(NTAccount));
                var groupRights = new Rights();
                var others = new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null).Translate(typeof(NTAccount));
                var othersRights = new Rights();
                var authorizationRules = security.GetAccessRules(true, true, typeof(NTAccount));
                foreach (AuthorizationRule rule in authorizationRules)
                {
                    FileSystemAccessRule fileRule = rule as FileSystemAccessRule;
                    if (fileRule != null)
                    {
                        if (owner != null && fileRule.IdentityReference == owner)
                        {
                            ReadRights(fileRule, ownerRights);
                        }
                        else if (group != null && fileRule.IdentityReference == group)
                        {
                            ReadRights(fileRule, groupRights);
                        }
                        if (fileRule.IdentityReference == others)
                        {
                            ReadRights(fileRule, othersRights);
                        }
                    }
                }
                builder.Append(ownerRights);
                builder.Append(groupRights);
                builder.Append(othersRights);
            }
            catch (Exception)
            {
                // Silently hide exception
                builder.Append("---------");
            }
            return builder.ToString();
        }

        private static FileSecurity GetFileSecurity(IFileSystemInfo systemInfo)
        {
            return new FileSecurity(systemInfo.FullName, 
                AccessControlSections.Owner | 
                AccessControlSections.Group |
                AccessControlSections.Access);
        }

        private static void ReadRights(FileSystemAccessRule fileRule, Rights ownerRights)
        {
            if (fileRule.FileSystemRights.HasFlag(FileSystemRights.ExecuteFile) ||
                fileRule.FileSystemRights.HasFlag(FileSystemRights.ReadAndExecute) ||
                fileRule.FileSystemRights.HasFlag(FileSystemRights.FullControl))
            {
                ownerRights.IsExecutable = true;
            }
            if (fileRule.FileSystemRights.HasFlag(FileSystemRights.Read) ||
                fileRule.FileSystemRights.HasFlag(FileSystemRights.ReadAndExecute) ||
                fileRule.FileSystemRights.HasFlag(FileSystemRights.FullControl))
            {
                ownerRights.IsReadable = true;
            }
            if (fileRule.FileSystemRights.HasFlag(FileSystemRights.Write) ||
                fileRule.FileSystemRights.HasFlag(FileSystemRights.FullControl))
            {
                ownerRights.IsWritable = true;
            }
        }

        private static char GetDirectoryIndication(IFileSystemInfo systemInfo)
        {
            if (systemInfo.Attributes.HasFlag(FileAttributes.Directory))
            {
                return 'd';
            }
            return '-';
        }

        public static bool IsExecutable(this IFileSystemInfo systemInfo, string accountName)
        {
            try
            {
                FileSecurity security = GetFileSecurity(systemInfo);
                var authorizationRules = security.GetAccessRules(true, true, typeof(NTAccount));
                foreach (AuthorizationRule rule in authorizationRules)
                {
                    FileSystemAccessRule fileRule = rule as FileSystemAccessRule;
                    if (fileRule != null && fileRule.IdentityReference.Value == accountName)
                    {
                        if (fileRule.FileSystemRights.HasFlag(FileSystemRights.ExecuteFile) ||
                            fileRule.FileSystemRights.HasFlag(FileSystemRights.ReadAndExecute) ||
                            fileRule.FileSystemRights.HasFlag(FileSystemRights.FullControl))
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Silently hide exception
            }
            return false;
        }
    }
}