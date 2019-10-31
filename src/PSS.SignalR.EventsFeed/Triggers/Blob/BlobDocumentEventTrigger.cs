using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PAREXEL.PSS.SignalR.EventsFeed.Contracts;

namespace PAREXEL.PSS.SignalR.EventsFeed.Triggers.Blob
{
    public static class BlobDocumentEventTrigger
    {
        [FunctionName("OnBlobDocumentChanged")]
        public static async Task Run(
            [BlobTrigger("%AzureBlobContainer%/{name}", Connection = "AzureBlobStorageConnectionString")]
            Stream myBlob,
            string name,
            [SignalR(HubName = "nonin3230", ConnectionStringSetting = "AzureSignalRConnectionString")]
            IAsyncCollector<SignalRMessage> signalRMessages,
            ILogger log)
        {
            string jsonData;
            using (var reader = new StreamReader(myBlob))
            {
                jsonData = reader.ReadToEnd();
            }

            var dayReadings = JsonConvert.DeserializeObject<Nonin3230DayReadings>(jsonData);
            
            foreach (var reading in dayReadings.PulseOximeterReadings)
            {
                await signalRMessages.AddAsync(new SignalRMessage
                {
                    Target = "readingsUpdated",
                    Arguments = new object[]
                    {
                        new Readings()
                        {
                            MeasurementTime = reading.Key.LocalDateTime,
                            PulseRate = reading.Value.PulseRate,
                            SpO2 = reading.Value.SpO2,
                            Id = reading.Value.TrackingId + reading.Key.UtcDateTime
                        }
                    }
                });
            }
        }
    }
}
