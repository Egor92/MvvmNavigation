using System;
using System.Collections.Generic;

namespace Egor92.MvvmNavigation.Internal
{
    internal class DataStorage : IDataStorage
    {
        private readonly IDictionary<string, RegistrationData> _navigationDataByKey = new Dictionary<string, RegistrationData>();

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

            _navigationDataByKey[key] = registrationData;
        }

        public bool IsExist(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            return _navigationDataByKey.ContainsKey(key);
        }

        public RegistrationData Get(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            return _navigationDataByKey[key];
        }
    }
}
