using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Collections.Generic;
using IceCreamRatings.Models;
using Microsoft.EntityFrameworkCore;
using IceCreamRatings.Data;

namespace IceCreamRatings.Functions;

public class GetRatings
{
    private RatingsContext db;
    public GetRatings(RatingsContext db) => this.db = db;

    [FunctionName("GetRatings")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req, ILogger log)
    {
        log.LogInformation("GetRatings triggered");

        if (Guid.TryParse(req.Query["userId"], out Guid id))
        {
            IEnumerable<Ratings> ratings = await db.Ratings.AsNoTracking().Where(rating => rating.UserId == id).ToArrayAsync();
            return new OkObjectResult(ratings);

        }

        return new BadRequestObjectResult("User id must be a valid GUID.");
    }
}
