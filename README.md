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

## Contents

- [Usage](#Usage)

- [Usage with Unity](#Usage-with-Unity)

- [Samples](#Samples)

## Usage

1. Install NuGet package [UINavigation.Wpf](https://www.nuget.org/packages/UINavigation.Wpf/)

1. Define navigation rules:
   ```csharp
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var window = new MainWindow();

            //1. Create navigation manager
            var navigationManager = new NavigationManager(window);

            //2. Define navigation rules: register key and corresponding view and viewmodel for it
            navigationManager.Register<FirstView>("FirstKey", () => new FirstViewModel(navigationManager));
            navigationManager.Register<SecondView>("SecondKey", () => new SecondViewModel(navigationManager));

            //3. Display start UI
            navigationManager.Navigate("FirstKey");

            window.Show();
        }
    }
   ```

1. Сall *Navigate* method in your ViewModel in order to switch UI
   ```csharp
    public class FirstViewModel : ViewModelBase
    {
        private readonly INavigationManager _navigationManager;

        public FirstViewModel(INavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        private void GoToSecondPage()
        {
            // Switch UI
            _navigationManager.Navigate("SecondKey");
        }
    }
   ```

   Look it in [a sample](https://github.com/Egor92/UINavigation/blob/master/samples/RestaurantApp/App.xaml.cs)

## Usage with Unity

UINavigation supports Unity out of the box.

1. Install two NuGet packages
 - [UINavigation.Wpf](https://www.nuget.org/packages/UINavigation.Wpf/)
 - [UINavigation.Unity](https://www.nuget.org/packages/UINavigation.Unity/)

1. Define navigation rules via UnityContainer:

   ```csharp
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //1. Create Window and UnityContainer
            var mainWindow = new MainWindow();
            var unityContainer = new UnityContainer();

            //2. Register navigation manager
            unityContainer.RegisterNavigationManager(mainWindow);

            //3. Register navigation rules
            unityContainer.RegisterNavigationRule<FirstViewModel, FirstView>("FirstKey");
            unityContainer.RegisterNavigationRule<SecondViewModel, SecondView>("SecondKey");

            //4. Display start UI
            var navigationManager = unityContainer.Resolve<INavigationManager>();
            navigationManager.Navigation("FirstKey");

            window.Show();
        }
    }
   ```

1. Сall *Navigate* method in your ViewModel in order to switch UI
   ```csharp
    public class FirstViewModel : ViewModelBase
    {
        private readonly INavigationManager _navigationManager;

        public FirstViewModel(INavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        private void GoToSecondPage()
        {
            // Switch UI
            _navigationManager.Navigate("SecondKey");
        }
    }
   ```

## Samples

See all samples [here](https://github.com/Egor92/UINavigation/tree/master/samples).
