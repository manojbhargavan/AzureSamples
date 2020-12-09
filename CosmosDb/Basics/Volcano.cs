using System.Collections.Generic;
using Newtonsoft.Json;

namespace cosmosDb
{
    public class Volcano
    {
        public string volcanoName { get; set; }
        public string country { get; set; }
        public string region { get; set; }
        public Location location { get; set; }
        public long? elevation { get; set; }
        public string type { get; set; }
        public string status { get; set; }
        public string lastKnownEruption { get; set; }
        public string id { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class Location
    {
        public string type { get; set; }
        public List<double> coordinates { get; set; }
    }


}