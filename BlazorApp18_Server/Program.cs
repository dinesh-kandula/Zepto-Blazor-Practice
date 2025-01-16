using BlazorApp18_Server.Components;
using BlazorApp18_Server.Services;
using Microsoft.AspNetCore.Builder;
using MudBlazor.Services;
using Toolbelt.Blazor.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();

builder.Services.AddApiServices();

builder.Services.AddHttpClient("ZeptoAPI", (sp, client) =>
{
    client.BaseAddress = new Uri("https://localhost:7174/");
    client.EnableIntercept(sp);
});

builder.Services.AddScoped(sp =>
{
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    return httpClientFactory.CreateClient("ZeptoAPI");
});

builder.Services.AddHttpClientInterceptor();

builder.Services.AddScoped<HttpInterceptorService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
