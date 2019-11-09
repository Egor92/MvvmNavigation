using System;
using JetBrains.Annotations;
using Unity;

namespace Egor92.UINavigation.Unity
{
    public class UnityDataStorage : IDataStorage
    {
        private readonly IUnityContainer _unityContainer;

        public UnityDataStorage([NotNull] IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer ?? throw new ArgumentNullException(nameof(unityContainer));
        }

        public void Add(string key, NavigationData navigationData)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (navigationData == null)
            {
                throw new ArgumentNullException(nameof(navigationData));
            }

            _unityContainer.RegisterInstance(key, navigationData);
        }

        public bool IsExist(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            return _unityContainer.IsRegistered<NavigationData>(key);
        }

        public NavigationData Get(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            return _unityContainer.Resolve<NavigationData>(key);
        }
    }
}
