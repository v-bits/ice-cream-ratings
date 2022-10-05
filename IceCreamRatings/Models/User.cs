﻿using Newtonsoft.Json;
using System;

namespace IceCreamRatings.Models;

public class User
{
    [JsonProperty(propertyName: "userId")]
    public Guid? Id { get; set; }

    [JsonProperty(propertyName: "userName")]
    public string Name { get; set; }

    [JsonProperty(propertyName: "fullName")]
    public string FullName { get; set; }
}
