using System.Diagnostics;
using System.Reflection;

namespace InvoisEGS.Utilities
{
    public static class VersionHelper
    {
        public static long GetNumberOnly(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return 0;
            }

            string numberString = new(input.Where(char.IsDigit).ToArray());
            return long.TryParse(numberString, out long result) ? result : 0;
        }
        public static string GetVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return "v" + fvi.FileVersion;
        }

        public static long GetLongVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return GetNumberOnly(fvi.FileVersion);
        }
    }
}