using Egor92.UINavigation.Abstractions;

namespace SampleWithUnity.ViewModels
{
    public class LeonardoViewModel : TurtleViewModel
    {
        public LeonardoViewModel(INavigationManager navigationManager)
            : base(navigationManager)
        {
        }

        public override string Name
        {
            get { return "Leonardo"; }
        }

        public override string Color
        {
            get { return "DeepSkyBlue"; }
        }
    }
}
