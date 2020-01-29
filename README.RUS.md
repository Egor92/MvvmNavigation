
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

## Как использовать

1. Установите NuGet пакет [UINavigation.Wpf](https://www.nuget.org/packages/UINavigation.Wpf/ "UINavigation.Wpf")
1. Используйте следующий код в вашем проекте:

    ```csharp
    //1. Создайте менеджер навигации
    var navigationManager = new NavigationManager(window);

    //2. Определите правила навигации: зарегистрируйте ключ (строку) с соответствующими View и ViewModel для него
    navigationManager.Register<FirstView>("FirstKey", () => new FirstViewModel(navigationManager));
    navigationManager.Register<SecondView>("SecondKey", () => new SecondViewModel(navigationManager));

    //3. В любом месте вызовите метод Navigate, чтобы сменить UI
    navigationManager.Navigate("FirstKey");
    ```

    Данный код должен быть размещён точке сборки приложения. Обычно это метод **App.OnStartup**:

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

1. Передайте менеджер навигации в вашу ViewModel и в нужном месте вызовите метод Navigate для смены UI на другой

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
            //Перейти на вторую страницу
            _navigationManager.Navigate("SecondKey");
        }
    }
    ```