using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCreamRatings.Models;

public class Product
{
    [JsonProperty(propertyName: "productId")]
    public Guid? Id { get; set; }

    [JsonProperty(propertyName: "productName")]
    public string Name { get; set; }

    [JsonProperty(propertyName: "productDescription")]
    public string Description { get; set; }
}
