using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace IceCreamRatings.Data;

public class ApiContext
{
    private readonly HttpClient http;
    public ApiContext(IHttpClientFactory http) => this.http = http.CreateClient("api");

    public async Task<T> InvokeAsync<T>(string endpoint, params object[] queries) where T : class
    {
        if (queries is not null)
        {
            endpoint = string.Format(endpoint, queries);
        }

        HttpResponseMessage response = await http.GetAsync(endpoint);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsAsync<T>();
        }
        else if (response.StatusCode != HttpStatusCode.BadRequest)
        {
            response.EnsureSuccessStatusCode();
        }

        return null;
    }
}
