# UINavigation

[![Build status](https://img.shields.io/appveyor/ci/Egor92/UINavigation/master)](https://ci.appveyor.com/project/Egor92/UINavigation/branch/master)
[![Version](https://img.shields.io/nuget/vpre/UINavigation.Wpf.svg)](https://www.nuget.org/packages/UINavigation.Wpf)
[![Downloads](https://img.shields.io/nuget/dt/UINavigation.Wpf.svg)](https://www.nuget.org/packages/UINavigation.Wpf)
[![CodeFactor](https://www.codefactor.io/repository/github/egor92/uinavigation/badge/master)](https://www.codefactor.io/repository/github/egor92/uinavigation/overview/master)
[![GitHub contributors](https://img.shields.io/github/contributors/Egor92/UINavigation.svg)](https://github.com/Egor92/UINavigation/graphs/contributors)
[![License](https://img.shields.io/github/license/Egor92/UINavigation.svg)](https://github.com/Egor92/UINavigation/blob/master/LICENSE)
[![Join the Gitter chat!](https://badges.gitter.im/Egor92/UINavigation.svg)](https://gitter.im/UINavigation/community?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

Go to [english page](https://github.com/Egor92/UINavigation/blob/master/README.md)

Данная библиотека позволяет настраивать поведение навигации вашего WPF приложения и реализовать навигацию на уровне слоя ViewModel. Библиотека полностью придерживается паттерна MVVM.

## Содерание

- [Как использовать](#Как-использовать)

- [Как использовать с Unity](#Как-использовать-с-Unity)

- [Примеры](#Примеры)

## Как использовать

1. Установите NuGet пакет [UINavigation.Wpf](https://www.nuget.org/packages/UINavigation.Wpf/)

1. Определите привала навигации:
   ```csharp
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var window = new MainWindow();

            //1. Создайте менеджер навигации
            var navigationManager = new NavigationManager(window);

            //2. Определите правила навигации: зарегистрируйте ключ (строку) с соответствующими View и ViewModel для него
            navigationManager.Register<FirstView>("FirstKey", () => new FirstViewModel(navigationManager));
            navigationManager.Register<SecondView>("SecondKey", () => new SecondViewModel(navigationManager));

            //3. Отобразите стартовый UI
            navigationManager.Navigate("FirstKey");

            window.Show();
        }
    }
   ```

1. Вызовите метод *Navigate* в вашей ViewModel, чтобы сменить UI
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
            // Сменить интерфейс
            _navigationManager.Navigate("SecondKey");
        }
    }
   ```

   Пример доступен [здесь](https://github.com/Egor92/UINavigation/blob/master/samples/RestaurantApp/App.xaml.cs)

## Как использовать с Unity

UINavigation поддерживает Unity из коробки.

1. Установите 2 NuGet пакета:
 - [UINavigation.Wpf](https://www.nuget.org/packages/UINavigation.Wpf/)
 - [UINavigation.Unity](https://www.nuget.org/packages/UINavigation.Unity/)

1. Определите привала навигации через UnityContainer:

   ```csharp
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //1. Создайте Window и UnityContainer
            var mainWindow = new MainWindow();
            var unityContainer = new UnityContainer();

            //2. Зарегистрируйте менеджер навигации
            unityContainer.RegisterNavigationManager(mainWindow);

            //3. Определите правила навигации
            unityContainer.RegisterNavigationRule<FirstViewModel, FirstView>("FirstKey");
            unityContainer.RegisterNavigationRule<SecondViewModel, SecondView>("SecondKey");
		 
            //4. Отобразите стартовый UI
            var navigationManager = unityContainer.Resolve<INavigationManager>();
            navigationManager.Navigation("FirstKey");

            window.Show();
        }
    }
    ```

1. Вызовите метод *Navigate* в вашей ViewModel, чтобы сменить UI
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
            // Сменить интерфейс
            _navigationManager.Navigate("SecondKey");
        }
    }
   ```

## Примеры

Все примеры доступны [здесь](https://github.com/Egor92/UINavigation/tree/master/samples).
