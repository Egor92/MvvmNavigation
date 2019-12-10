using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Egor92.UINavigation.Tests.Common
{
    public class TestCaseDataBuilder
    {
        private readonly IList<object> _values = new List<object>();
        private readonly IList<string> _displayNames = new List<string>();

        public static TestCaseDataBuilder Create()
        {
            return new TestCaseDataBuilder();
        }

        public TestCaseDataBuilder WithParameter(object value, string displayName)
        {
            _values.Add(value);
            _displayNames.Add(displayName);
            return this;
        }

        public TestCaseData Build()
        {
            var values = _values.ToArray();
            var displayNames = _displayNames.ToArray();
            return new TestCaseData(values).SetArgDisplayNames(displayNames);
        }
    }
}
