using System;

namespace IceCreamRatings.Models;

public class Ratings
{
    public Guid? Id { get; set; }
    public Guid? UserId { get; set; }
    public Guid? ProductId { get; set; }
    public DateTime? Timestamp { get; set; }
    public string LocationName { get; set; }
    public int? Rating { get; set; }
    public string UserNotes { get; set; }
}
