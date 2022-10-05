using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.DependencyInjection;
using IceCreamRatings.Data;

[assembly: FunctionsStartup(typeof(IceCreamRatings.Startup))]

namespace IceCreamRatings;
internal class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        string connectionString = Environment.GetEnvironmentVariable("ConnectionStrings:ICR_CONNECTION_STRING");
        Uri uri = new Uri("https://serverlessohapi.azurewebsites.net");

        builder.Services.AddHttpClient("api", client => client.BaseAddress = uri);
        builder.Services.AddTransient<ApiContext>();
        builder.Services.AddTransient(_ => new RatingsContext(connectionString));
    }
}
