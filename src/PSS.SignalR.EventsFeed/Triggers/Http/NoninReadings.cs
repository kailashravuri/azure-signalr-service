using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PAREXEL.PSS.SignalR.EventsFeed.Contracts;

namespace PAREXEL.PSS.SignalR.EventsFeed.Triggers.Http
{
    public static class NoninReadings
    {
        [FunctionName("GetReadings")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous)] HttpRequest req,
            [Blob("%AzureBlobContainer%", FileAccess.Read, Connection = "AzureBlobStorageConnectionString")] IEnumerable<Stream> blobItems,
            ILogger log)
        {
            var readingsList = new List<Readings>();
            foreach (var blobItem in blobItems)
            {
                var streamReader = new StreamReader(blobItem);
                var jsonData = streamReader.ReadToEnd();
                var dayReadings = JsonConvert.DeserializeObject<Nonin3230DayReadings>(jsonData);
                foreach (var reading in dayReadings.PulseOximeterReadings)
                {
                    readingsList.Add(new Readings()
                    {
                        MeasurementTime = reading.Key.LocalDateTime,
                        PulseRate = reading.Value.PulseRate,
                        SpO2 = reading.Value.SpO2,
                        Id = reading.Value.TrackingId + reading.Key.UtcDateTime
                    });
                }
            }

            return new OkObjectResult(readingsList);
        }
    }
}
