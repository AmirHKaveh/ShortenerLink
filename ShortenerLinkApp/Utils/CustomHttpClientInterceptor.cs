using Microsoft.AspNetCore.Components;

using ShortenerLinkApp.Services;

using System.Net;

namespace ShortenerLinkApp.Utils
{
    public class CustomHttpClientInterceptor : DelegatingHandler
    {
        private readonly ISnackbarService _snackbarService;
        private readonly NavigationManager _navigationManager;

        public CustomHttpClientInterceptor(NavigationManager navigationManager, ISnackbarService snackbarService)
        {
            _navigationManager = navigationManager;
            _snackbarService = snackbarService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                _snackbarService.Add("To access this section, first log in to your account.", MudBlazor.Severity.Warning);

                //  _navigationManager.NavigateTo("/login"); // Redirect to login page
            }

            return response;
        }
    }
}
