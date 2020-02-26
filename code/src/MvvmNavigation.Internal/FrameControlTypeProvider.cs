using System;
using System.Linq;

namespace Egor92.MvvmNavigation.Internal
{
    internal class FrameControlTypeProvider : IFrameControlTypeProvider
    {
        public Type GetFrameControlType(Type navigationManagerType)
        {
            var attributeData = navigationManagerType.GetCustomAttributesData()
                                                     .FirstOrDefault(a => a.AttributeType == typeof(NavigationManagerAttribute));

            if (attributeData?.ConstructorArguments.Count != 1)
            {
                return null;
            }

            var argument = attributeData.ConstructorArguments[0];
            return (Type) argument.Value;
        }
    }
}
