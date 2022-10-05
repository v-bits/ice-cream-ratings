using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using IceCreamRatings.Models;
using IceCreamRatings.Data;

namespace IceCreamRatings.Functions;

public class CreateRating
{
    private RatingsContext db;
    private readonly ApiContext api;

    public CreateRating(RatingsContext db, ApiContext api)
    {
        this.db = db;
        this.api = api;
    }

    [FunctionName("CreateRating")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req, ILogger log)
    {
        log.LogInformation("CreateRating triggered");

        string json = await new StreamReader(req.Body).ReadToEndAsync();
        Ratings rating = JsonConvert.DeserializeObject<Ratings>(json) ?? throw new ArgumentNullException(nameof(Ratings));

        if (await api.InvokeAsync<User>("/api/GetUser?userId={0}", rating.UserId) is not User user)
        {
            return new BadRequestObjectResult("User id not found.");
        }

        if (await api.InvokeAsync<Product>("/api/GetProduct?productId={0}", rating.ProductId) is not Product product)
        {
            return new BadRequestObjectResult("Product id not found.");
        }

        if (!(rating.Rating >= 0 && rating.Rating <= 5))
        {
            return new BadRequestObjectResult("Rating must be from 0 to 5.");
        }

        rating.Id = null;
        rating.Timestamp = DateTime.UtcNow;

        db.Ratings.Add(rating);
        await db.SaveChangesAsync();

        return new OkObjectResult(rating);
    }
}
