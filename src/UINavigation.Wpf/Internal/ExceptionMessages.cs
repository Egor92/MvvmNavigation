namespace Egor92.UINavigation.Wpf.Internal
{
    public static class ExceptionMessages
    {
        public const string CanNotRegisterKeyTwice = "Cann't register navigation key twice";

        public static string KeyIsNotRegistered(string navigationKey)
        {
            return $"Key '{navigationKey}' is not registered for navigation";
        }
    }
}
