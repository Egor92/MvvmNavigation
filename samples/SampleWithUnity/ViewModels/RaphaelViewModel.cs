using Egor92.UINavigation.Abstractions;

namespace SampleWithUnity.ViewModels
{
    public class RaphaelViewModel : TurtleViewModel
    {
        public RaphaelViewModel(INavigationManager navigationManager)
            : base(navigationManager)
        {
        }

        public override string Name
        {
            get { return "Raphael"; }
        }

        public override string Color
        {
            get { return "Red"; }
        }
    }
}
