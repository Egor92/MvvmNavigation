using Egor92.UINavigation.Abstractions;

namespace SampleWithUnity.ViewModels
{
    public class MichelangeloViewModel : TurtleViewModel
    {
        public MichelangeloViewModel(INavigationManager navigationManager)
            : base(navigationManager)
        {
        }

        public override string Name
        {
            get { return "Michelangelo"; }
        }

        public override string Color
        {
            get { return "Orange"; }
        }
    }
}
