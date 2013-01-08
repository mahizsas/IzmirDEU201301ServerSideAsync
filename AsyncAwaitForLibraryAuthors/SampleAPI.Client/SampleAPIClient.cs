using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SampleAPI.Client {

    public class SampleAPIClient {

        private const string ApiUri = "http://localhost:17257/api/cars";

        public async Task<IEnumerable<Car>> GetCarsInAWrongWayAsync() {

            using (HttpClient client = new HttpClient()) {

                var response = await client.GetAsync(ApiUri);
                var cars = await response.Content.ReadAsAsync<IEnumerable<Car>>();
                return cars;
            }
        }

        public async Task<IEnumerable<Car>> GetCarsAsync() {

            using (HttpClient client = new HttpClient()) {

                var response = await client.GetAsync(ApiUri)
                    .ConfigureAwait(continueOnCapturedContext: false);

                var cars = await response.Content.ReadAsAsync<IEnumerable<Car>>()
                    .ConfigureAwait(continueOnCapturedContext: false);

                return cars;
            }
        }
    }
}