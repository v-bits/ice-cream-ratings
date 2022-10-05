using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using IceCreamRatings.Models;
using Microsoft.EntityFrameworkCore;
using IceCreamRatings.Data;

namespace IceCreamRatings.Functions;

public class GetRating
{
    private RatingsContext db;
    public GetRating(RatingsContext db) => this.db = db;

    [FunctionName("GetRating")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req, ILogger log)
    {
        log.LogInformation("GetRatings triggered");

        if (Guid.TryParse(req.Query["ratingId"], out Guid id))
        {
            if (await db.Ratings.AsNoTracking().SingleOrDefaultAsync(rating => rating.Id == id) is Ratings rating)
            {
                return new OkObjectResult(rating);
            }

            return new BadRequestObjectResult("Rating id not found.");
        }

        return new BadRequestObjectResult("Rating id must be a valid GUID.");
    }
}
