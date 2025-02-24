namespace Egor92.MvvmNavigation.Abstractions
{
    public record NavigationData
    {
        public required object View { get; init; }

        public required object ViewModel { get; init; }
    }
}