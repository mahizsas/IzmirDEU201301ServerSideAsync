using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace AsyncHandlerParallelSample {

    /// <summary>
    /// Summary description for DownloadOneByOne
    /// </summary>
    public class DownloadOneByOne : HttpTaskAsyncHandler {

        public override async Task ProcessRequestAsync(HttpContext context) {

            // We even have CancellationTokens provided by ASP.NET infrastructure.
            // Isn't that great :)
            var cts = CancellationTokenSource.CreateLinkedTokenSource(
                context.Request.TimedOutToken,
                context.Response.ClientDisconnectedToken);

            using (HttpClient client = new HttpClient()) {

                var downloadTasks = new List<Task<HttpResponseMessage>> { 
                    client.GetAsync("http://google.com", cts.Token),
                    client.GetAsync("http://bing.com", cts.Token),
                    client.GetAsync("http://twitter.com", cts.Token),
                    client.GetAsync("http://github.com", cts.Token),
                    client.GetAsync("http://microsoft.com", cts.Token),
                    client.GetAsync("http://asp.net", cts.Token)
                };

                while (downloadTasks.Count > 0) {

                    var completedTask = await Task.WhenAny(downloadTasks);
                    downloadTasks.Remove(completedTask);

                    if (completedTask.IsCanceled) {

                        break;
                    }

                    string content = await completedTask.Result.Content.ReadAsStringAsync();
                    context.Response.Write(content);

                    // NOTE: You cannot flush concurently. 
                    // So, flush one at a time.
                    await context.Response.FlushAsync();
                }
            }

            cts.Dispose();
        }
    }
}