using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace AsyncHandlerParallelSample {

    public class Download : HttpTaskAsyncHandler {

        public override async Task ProcessRequestAsync(HttpContext context) {

            Task<string> googleTask;
            Task<string> bingTask;
            using (HttpClient client = new HttpClient()) {

                googleTask = client.GetStringAsync("http://www.google.com");
                bingTask = client.GetStringAsync("http://www.bing.com");

                await Task.WhenAll(googleTask, bingTask);
            }

            context.Response.Write(googleTask.Result);
            context.Response.Write(bingTask.Result);

            await context.Response.FlushAsync();
        }
    }

    public static class HttpResponseExtensions {

        public static Task FlushAsync(this HttpResponse response) {
            
            // If you're running in IIS or IIS Express, 
            // you are guaranteed that this will return true.
            // ref: http://stackoverflow.com/questions/14161105
            if (response.SupportsAsyncFlush) {

                return Task.Factory.FromAsync(response.BeginFlush, response.EndFlush, null);
            }

            response.Flush();
            return Task.FromResult(0);
        }
    }
}