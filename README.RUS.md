
# UINavigation

[![Build status](https://img.shields.io/appveyor/ci/Egor92/UINavigation/master)](https://ci.appveyor.com/project/Egor92/UINavigation/branch/master)
[![Version](https://img.shields.io/nuget/vpre/UINavigation.Wpf.svg)](https://www.nuget.org/packages/UINavigation.Wpf)
[![Downloads](https://img.shields.io/nuget/dt/UINavigation.Wpf.svg)](https://www.nuget.org/packages/UINavigation.Wpf)
[![GitHub contributors](https://img.shields.io/github/contributors/Egor92/UINavigation.svg)](https://github.com/Egor92/UINavigation/graphs/contributors)
[![License](https://img.shields.io/github/license/Egor92/UINavigation.svg)](https://github.com/Egor92/UINavigation/blob/master/LICENSE)
[![Join the Gitter chat!](https://badges.gitter.im/Egor92/UINavigation.svg)](https://gitter.im/UINavigation/community?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

Go to [english page](https://github.com/Egor92/UINavigation/blob/master/README.md)

Данная библиотека позволяет настраивать поведение навигации вашего WPF приложения и реализовать навигацию на уровне слоя ViewModel. Библиотека полностью придерживается паттерна MVVM.

## Как использовать

1. Установите NuGet пакет [UINavigation.Wpf](https://www.nuget.org/packages/UINavigation.Wpf/ "UINavigation.Wpf")
1. Используйте следующий код в вашем проекте:

```csharp
//1. Создайте менеджера навигации
var navigationManager = new NavigationManager(window);

//2. Зарегистрируйте набор: ключ навигации, View и ViewModel
navigationManager.Register<FirstView>(NavigationKeys.First, () => new FirstViewModel());
navigationManager.Register<SecondView>(NavigationKeys.Second, () => new SecondViewModel());

//3. В любом месте вызовите метод Navigate, чтобы сменить пользовательский интерфейс на другой
navigationManager.Navigate(NavigationKeys.First);
```

Данный код должен быть размещён точке сборки приложения. Обычно это метод **App.OnStartup**:

```csharp
public partial class App : Application
{
	protected override void OnStartup(StartupEventArgs e)
	{
		var window = new MainWindow();
		var navigationManager = new NavigationManager(window);

		navigationManager.Register<FirstView>(NavigationKeys.First, () => new FirstViewModel());
		navigationManager.Register<SecondView>(NavigationKeys.Second, () => new SecondViewModel());

		navigationManager.Navigate(NavigationKeys.First);
		window.Show();
	}
}
```
