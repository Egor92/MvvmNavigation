namespace Egor92.UINavigation.Wpf.Unity.Internal
{
    internal static class ExceptionMessages
    {
        internal static string CanNotRegisterInterfaceAsViewModel<TViewModel>()
        {
            return $"Can't register interface {typeof(TViewModel).FullName} as ViewModel. Type of ViewModel must be instance class";
        }

        internal static string CanNotRegisterAbstractClassAsViewModel<TViewModel>()
        {
            return $"Can't register abstract class {typeof(TViewModel).FullName} as ViewModel. Type of ViewModel must be instance class";
        }

        internal static string CanNotRegisterAbstractClassAsView<TView>()
        {
            return $"Can't register abstract class {typeof(TView).FullName} as View. Type of View must be instance class";
        }
    }
}
