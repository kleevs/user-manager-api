using System.Collections.Generic;

namespace Web.Configuration
{
    public class AppConfig
    {
        public Cors Cors { get; set; } 
    }

    public class Cors
    {
        public IEnumerable<string> CorsAcceptedDomain { get; set; }
    }
}
