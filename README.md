
# UINavigation

[![Build status](https://img.shields.io/appveyor/ci/Egor92/UINavigation/master)](https://ci.appveyor.com/project/Egor92/UINavigation/branch/master)
[![Version](https://img.shields.io/nuget/vpre/UINavigation.Wpf.svg)](https://www.nuget.org/packages/UINavigation.Wpf)
[![Downloads](https://img.shields.io/nuget/dt/UINavigation.Wpf.svg)](https://www.nuget.org/packages/UINavigation.Wpf)
[![CodeFactor](https://www.codefactor.io/repository/github/egor92/uinavigation/badge/master)](https://www.codefactor.io/repository/github/egor92/uinavigation/overview/master)
[![GitHub contributors](https://img.shields.io/github/contributors/Egor92/UINavigation.svg)](https://github.com/Egor92/UINavigation/graphs/contributors)
[![License](https://img.shields.io/github/license/Egor92/UINavigation.svg)](https://github.com/Egor92/UINavigation/blob/master/LICENSE)
[![Join the Gitter chat!](https://badges.gitter.im/Egor92/UINavigation.svg)](https://gitter.im/UINavigation/community?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

Перейти на [русскую страницу](https://github.com/Egor92/UINavigation/blob/master/README.RUS.md)

This library allows you to adjust navigation behavior in your WPF application and implement ViewModel-based navigation. This library completely adhere to MVVM pattern.

## How to use it

1. Install NuGet package [UINavigation.Wpf](https://www.nuget.org/packages/UINavigation.Wpf/ "UINavigation.Wpf")
1. Use the following code in your project:

    ```csharp
    //1. Create navigation manager
    var navigationManager = new NavigationManager(window);

    //2. Define navigation rules: register key and corresponding view and viewmodel for it
    navigationManager.Register<FirstView>("FirstKey", () => new FirstViewModel(navigationManager));
    navigationManager.Register<SecondView>("SecondKey", () => new SecondViewModel(navigationManager));

    //3. In any place call Navigate method in order to switch UI
    navigationManager.Navigate("FirstKey");
    ```

    This code should be placed in composition root. It is **App.OnStartup** method usually:

    ```csharp
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var window = new MainWindow();
            var navigationManager = new NavigationManager(window);

            navigationManager.Register<FirstView>("FirstKey", () => new FirstViewModel(navigationManager));
            navigationManager.Register<SecondView>("SecondKey", () => new SecondViewModel(navigationManager));

            navigationManager.Navigate("FirstKey");
            window.Show();
        }
    }
    ```

1. Pass navigation manager to your ViewModel and call Navigate method in the required place in order to switch to other UI

    ```csharp
    public class FirstViewModel : ViewModelBase
    {
        private readonly INavigationManager _navigationManager;

        public FirstViewModel(INavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
            GoToSecondPageCommand = new DelegateCommand(GoToSecondPage);
        }

        public ICommand GoToSecondPageCommand { get; }

        private void GoToSecondPage()
        {
            //Switch to second UI
            _navigationManager.Navigate("SecondKey");
        }
    }
    ```