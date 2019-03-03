namespace Egor92.UINavigation.Abstractions
{
    public interface INavigationAware
    {
        void OnNavigatingTo(object arg);

        void OnNavigatingFrom();
    }
}
