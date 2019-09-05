using System.Windows;
using RestaurantApp.Infrastructure.Abstractions;

namespace RestaurantApp.Infrastructure.Implementations
{
    public class DialogManager : IDialogManager
    {
        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
