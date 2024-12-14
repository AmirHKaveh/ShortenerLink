using Blazor.QrCodeGen;

using Blazored.LocalStorage;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;

using MudBlazor;
using MudBlazor.Services;


using ShortenerLinkApp;
using ShortenerLinkApp.Services;
using ShortenerLinkApp.ViewModels;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddSingleton<OTPSharedDataService>();
builder.Services.AddSingleton<ISnackbarService, ToastService>();
builder.Services.AddSingleton<ISnackbar,SnackbarService>();
builder.Services.AddTransient(sp => new ModuleCreator(sp.GetService<IJSRuntime>()));


builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
