namespace Egor92.MvvmNavigation.Abstractions
{
    public record NavigationData
    {
        public void Deconstruct(out object view, out object viewModel)
        {
            view = View;
            viewModel = ViewModel;
        }

        public required object View { get; init; }

        public required object ViewModel { get; init; }
    }
}