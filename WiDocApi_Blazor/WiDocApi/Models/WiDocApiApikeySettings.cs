﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiDocApi_Blazor.WiDocApi.Models
{
    public class WiDocApiApikeySettings
    {

       public string? ApiKeyHeaderName { get; set; } = "X-Api-Key";
       public List<string?> ApiKeys { get; set; } = new List<string?>();

    }
}
