﻿using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Egor92.MvvmNavigation.Core.ContractTests;
using NUnit.Framework;

namespace Egor92.MvvmNavigation.Wpf.UnitTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Children)]
    [Apartment(ApartmentState.STA)]
    public class NavigationManagerTests : NavigationManagerBaseContractTests<NavigationManager, ContentControl, FrameworkElement>
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        protected override NavigationManager CreateNavigationManager(ContentControl frameControl)
        {
            return new NavigationManager(frameControl);
        }

        protected override NavigationManager CreateNavigationManager(ContentControl frameControl, IDataStorage dataStorage)
        {
            return new NavigationManager(frameControl, dataStorage);
        }

        protected override void RegisterNavigationRule(NavigationManager navigationManager,
                                                       string navigationKey,
                                                       Func<object> viewModelFunc,
                                                       Func<object> viewFunc)
        {
            navigationManager.Register(navigationKey, viewModelFunc, viewFunc);
        }

        protected override object GetContent(ContentControl contentControl)
        {
            return contentControl.Content;
        }

        protected override object GetDataContext(object view)
        {
            return (view as FrameworkElement)?.DataContext;
        }

        protected override void SetupCanNavigate(NavigationManager navigationManager, string navigationKey, bool canNavigate)
        {
            if (canNavigate)
            {
                navigationManager.Register(navigationKey, () => new object(), () => new ContentControl());
            }
        }
    }
}
