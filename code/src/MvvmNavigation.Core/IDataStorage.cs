using JetBrains.Annotations;

namespace Egor92.MvvmNavigation
{
    public interface IDataStorage
    {
        void Add([NotNull] string key, [NotNull] RegistrationData registrationData);

        bool IsExist([NotNull] string key);

        RegistrationData Get([NotNull] string key);
    }
}
