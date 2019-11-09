namespace Egor92.UINavigation.Tests.Common
{
    public static class ExceptionMessages
    {
        public static string NullArgument(string argumentName)
        {
            return $"Value cannot be null.\r\nParameter name: {argumentName}";
        }
    }
}
