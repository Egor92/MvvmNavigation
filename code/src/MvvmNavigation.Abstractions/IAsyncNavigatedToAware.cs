using System.Threading.Tasks;

namespace Egor92.MvvmNavigation.Abstractions
{
    public interface IAsyncNavigatedToAware
    {
        Task OnNavigatedToAsync(object arg);
    }
}