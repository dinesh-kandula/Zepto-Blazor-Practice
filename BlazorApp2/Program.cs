using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorApp2;
using BlazorApp2.Services;
using MudBlazor.Services;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using System.Net.Http;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddScoped<ApiServices>();

builder.Services.AddScoped<ProductCartService>();

builder.Services.AddMudServices();

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7182/") });

builder.Services.AddHttpClient("ZeptoAPI", (sp, client) =>
{
    client.BaseAddress = new Uri("https://localhost:7182/");
    client.EnableIntercept(sp);
});

builder.Services.AddScoped(sp =>
{
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    return httpClientFactory.CreateClient("ZeptoAPI");
});

builder.Services.AddHttpClientInterceptor();

builder.Services.AddScoped<HttpInterceptorService>();

await builder.Build().RunAsync();
