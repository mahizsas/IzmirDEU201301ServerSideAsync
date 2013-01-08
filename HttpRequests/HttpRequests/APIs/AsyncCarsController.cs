using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using HttpRequests.Models;
using System.Net;
using Newtonsoft.Json;
using System.Threading;

namespace HttpRequests.APIs {

    public class AsyncCarsController : ApiController {

        //HTTP service base address
        const string CountryAPIBaseAddress = "http://localhost:11338/api/cars";

        public async Task<IEnumerable<Car>> Get() {

            using (WebClient client = new WebClient()) {

                var result = await client.DownloadStringTaskAsync(CountryAPIBaseAddress);
                var cars = JsonConvert.DeserializeObject<IEnumerable<Car>>(result);

                return cars;
            }
        }
    }
}