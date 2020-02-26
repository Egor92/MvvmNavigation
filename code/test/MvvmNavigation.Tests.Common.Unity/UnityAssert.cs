using System;
using NUnit.Framework;
using Unity;

namespace Egor92.MvvmNavigation.Tests.Common.Unity
{
    public static class UnityAssert
    {
        public static void IsRegistered<T>(IUnityContainer unityContainer)
        {
            var isRegistered = unityContainer.IsRegistered<T>();
            if (!isRegistered)
            {
                throw new AssertionException($"Type '{typeof(T).FullName}' isn't registered in UnityContainer");
            }
        }

        public static void IsRegisteredWithName<T>(IUnityContainer unityContainer, string name)
        {
            var isRegistered = unityContainer.IsRegistered<T>(name);
            if (!isRegistered)
            {
                throw new AssertionException($"Type '{typeof(T).FullName}' isn't registered in UnityContainer with name {name}");
            }
        }

        public static void CanResolve<T>(IUnityContainer unityContainer)
        {
            try
            {
                unityContainer.Resolve<T>();
            }
            catch (Exception e)
            {
                throw new AssertionException($"Type '{typeof(T).FullName}' can't be resolved by UnityContainer", e);
            }
        }
    }
}
