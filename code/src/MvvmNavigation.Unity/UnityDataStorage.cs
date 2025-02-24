using System;
using JetBrains.Annotations;
using Unity;

namespace Egor92.MvvmNavigation.Unity
{
    public class UnityDataStorage : IDataStorage
    {
        private readonly IUnityContainer _unityContainer;

        public UnityDataStorage([NotNull] IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer ?? throw new ArgumentNullException(nameof(unityContainer));
        }

        public void Add(string key, RegistrationData registrationData)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (registrationData == null)
            {
                throw new ArgumentNullException(nameof(registrationData));
            }

            _unityContainer.RegisterInstance(key, registrationData);
        }

        public bool IsExist(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            return _unityContainer.IsRegistered<RegistrationData>(key);
        }

        public RegistrationData Get(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            return _unityContainer.Resolve<RegistrationData>(key);
        }
    }
}
