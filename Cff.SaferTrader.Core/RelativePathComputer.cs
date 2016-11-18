using System;
using System.Web;

namespace Cff.SaferTrader.Core
{
    /// <summary>
    /// Computes relative path to root as javascript cannot easily resolve path
    /// when the current path is not the root path, e.g. Root/SubDirectory
    /// </summary>
    public static class RelativePathComputer
    {
        /// <summary>
        /// Computes relative path to root
        /// e.g. Root/SubDirectory/File.file would return "../"
        /// </summary>
        public static string ComputeRelativePathToRoot(string rootPath, string currentPath)
        {
            ArgumentChecker.ThrowIfNullOrEmpty(rootPath, "rootPath");
            ArgumentChecker.ThrowIfNullOrEmpty(currentPath, "currentPath");

            if (rootPath.EndsWith("\\"))
            {
                rootPath = rootPath.Substring(0, rootPath.Length - 1);
            }

            if (currentPath.IndexOf(rootPath) < 0)
            {
                throw new ArgumentException(currentPath, "currentPath");
            }

            string relativePathToRoot = string.Empty;
            string temp = currentPath.Substring(rootPath.Length);

            foreach (char c in temp)
            {
                if (c == '\\')
                {
                    relativePathToRoot += "../";
                }
            }

            return relativePathToRoot;
        }

        public static string ComputeRelativePathToRoot(HttpServerUtility serverUtility)
        {
            return ComputeRelativePathToRoot(serverUtility.MapPath("~"), serverUtility.MapPath("."));
        }
    }
}