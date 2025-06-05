global using GeekVerse.Shared;
global using System.Net.Http.Json;
global using GeekVerse.Client.Services.CategoryService;
global using GeekVerse.Client.Services.ProductService;
global using GeekVerse.Client.Services.CartService;
global using GeekVerse.Client.Services.AuthService;
global using GeekVerse.Client.Services.OrderService;
global using GeekVerse.Client.Services.AddressService;
global using GeekVerse.Client.Services.ProductTypeService;
global using Microsoft.AspNetCore.Components.Authorization;

using GeekVerse.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IProductTypeService, ProductTypeService>();


builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();


await builder.Build().RunAsync();
