using ApexCharts;
using Blazored.LocalStorage;
using Blazored.Toast;
using BlazorTable;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PricePredict.Web;
using PricePredict.Web.ApiClients;
using PricePredict.Web.Services;
using Refit;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#main-wrapper");
builder.RootComponents.Add<HeadOutlet>("head::after");

var pricePredictionUrl = builder.Configuration["ApiSettings:PricePredictionUrl"];
if (string.IsNullOrEmpty(pricePredictionUrl))
{
    pricePredictionUrl = builder.HostEnvironment.BaseAddress;
}

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(pricePredictionUrl) });
builder.Services.AddRefitClient<IIndicatorApi>().ConfigureHttpClient(c => c.BaseAddress = new Uri(pricePredictionUrl));

// Register for internal service
builder.Services.AddScoped<IMenuService, MenuService>();

// Register for app
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredToast();
builder.Services.AddBlazorTable();
builder.Services.AddSweetAlert2();
builder.Services.AddApexCharts();

await builder.Build().RunAsync();
