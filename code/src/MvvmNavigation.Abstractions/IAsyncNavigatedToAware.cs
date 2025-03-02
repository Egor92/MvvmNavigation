using System.Threading;
using System.Threading.Tasks;

namespace Egor92.MvvmNavigation.Abstractions
{
    public interface IAsyncNavigatedToAware
    {
        Task OnNavigatedToAsync(object arg, CancellationToken token = default);
    }
}