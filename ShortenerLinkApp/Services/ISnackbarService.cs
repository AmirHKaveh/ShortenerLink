using MudBlazor;

namespace ShortenerLinkApp.Services
{

    public interface ISnackbarService
    {
        void Add(string message, Severity severity = Severity.Normal);
    }
}
