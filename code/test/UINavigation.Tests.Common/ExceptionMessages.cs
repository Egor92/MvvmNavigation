using System;
using System.Runtime.InteropServices;

namespace Egor92.UINavigation.Tests.Common
{
    public static class ExceptionMessages
    {
        public static string NullArgument(string argumentName)
        {
            if (RuntimeInformation.FrameworkDescription.StartsWith(".NET Framework"))
            {
                return $"Value cannot be null.\r\nParameter name: {argumentName}";
            }

            if (RuntimeInformation.FrameworkDescription.StartsWith(".NET Core"))
            {
                return $"Value cannot be null. (Parameter '{argumentName}')";
            }

            throw new ArgumentException("Unhandled framework version", nameof(argumentName));
        }
    }
}
