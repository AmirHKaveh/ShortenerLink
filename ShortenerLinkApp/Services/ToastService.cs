using MudBlazor;

namespace ShortenerLinkApp.Services
{
    public class ToastService : ISnackbarService
    {
        private readonly ISnackbar _snackbar;

        public ToastService(ISnackbar snackbar)
        {
            _snackbar = snackbar;
        }

        public void Add(string message, Severity severity = Severity.Normal)
        {
            _snackbar.Add(message, severity);
        }
    }
}
