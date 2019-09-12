using System;
using System.Collections.Generic;

namespace Egor92.UINavigation.Wpf.Internal
{
    internal class DataStorage : IDataStorage
    {
        private readonly IDictionary<string, NavigationData> _navigationDataByKey = new Dictionary<string, NavigationData>();

        public void Add(string key, NavigationData navigationData)
        {
            key = key ?? throw new ArgumentNullException(nameof(key));
            navigationData = navigationData ?? throw new ArgumentNullException(nameof(navigationData));

            _navigationDataByKey[key] = navigationData;
        }

        public bool IsExist(string key)
        {
            key = key ?? throw new ArgumentNullException(nameof(key));

            return _navigationDataByKey.ContainsKey(key);
        }

        public NavigationData Get(string key)
        {
            key = key ?? throw new ArgumentNullException(nameof(key));

            return _navigationDataByKey[key];
        }
    }
}
