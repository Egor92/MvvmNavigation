using Egor92.UINavigation.Abstractions;

namespace SampleWithUnity.ViewModels
{
    public class DonatelloViewModel : TurtleViewModel
    {
        public DonatelloViewModel(INavigationManager navigationManager)
            : base(navigationManager)
        {
        }

        public override string Name
        {
            get { return "Donatello"; }
        }

        public override string Color
        {
            get { return "Violet"; }
        }
    }
}
