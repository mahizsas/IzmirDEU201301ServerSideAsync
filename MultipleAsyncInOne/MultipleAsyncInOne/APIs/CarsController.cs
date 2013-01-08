using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace MultipleAsyncInOne.APIs {

    public class Car {

        public int Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public float Price { get; set; }
    }

    public class CarsController : ApiController {

        readonly List<string> _payloadSources = new List<string> { 
            "http://localhost:2700/api/cars/cheap",
            "http://localhost:2700/api/cars/expensive"
        };

        readonly HttpClient _httpClient = new HttpClient();

        // http://localhost:2419/api/cars/AllCars
        [HttpGet]
        public async Task<IEnumerable<Car>> AllCars() {

            List<Car> carsList = new List<Car>();

            foreach (var uri in _payloadSources) {

                var cars = await getCarsAsync(uri);
                carsList.AddRange(cars);
            }

            return carsList;
        }

        // http://localhost:2419/api/cars/AllCarsInParallel
        [HttpGet]
        public async Task<IEnumerable<Car>> AllCarsInParallel() {

            var allTasks = _payloadSources.Select(uri => 
                getCarsAsync(uri)
            );

            IEnumerable<Car>[] allResults = await Task.WhenAll(allTasks);

            return allResults.SelectMany(cars => cars);
        }

        //private helper which gets the payload and hands it back as IEnumerable<Car>
        private async Task<IEnumerable<Car>> getCarsAsync(string uri) {

            var response = await _httpClient.GetAsync(uri);
            var content = await response.Content.ReadAsAsync<IEnumerable<Car>>();

            return content;
        }
    }
}