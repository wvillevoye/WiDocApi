using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiDocApi_Blazor.WiDocApi.Models
{
    public class GroupedApiEndpoints
    {
        public string? GroupName { get; set; }
        public List<ApiEndpoint>? Endpoints { get; set; }
    }
  
}
