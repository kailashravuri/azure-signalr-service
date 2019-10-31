using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;
using PAREXEL.PSS.SignalR.EventsFeed.Contracts;

namespace PAREXEL.PSS.SignalR.EventsFeed.Triggers.Cosmos
{
    public static class CosmosDocumentEventTrigger
    {
        [FunctionName("OnCosmosDocumentsChanged")]
        public static async Task Run(
            [CosmosDBTrigger("%AzureCosmosDatabase%", "%AzureCosmosCollection%", ConnectionStringSetting =
                "AzureWebJobsCosmosDBConnectionString",
                LeaseConnectionStringSetting = "AzureWebJobsCosmosDBConnectionString",
                CreateLeaseCollectionIfNotExists = true,
                LeaseCollectionName = "leases")]
            IEnumerable<dynamic> updatedReadings,
            [SignalR(HubName = "flights", ConnectionStringSetting = "AzureSignalRConnectionString")] IAsyncCollector<SignalRMessage> signalRMessages,
            ILogger log)
        {
            foreach (var dayReadings in updatedReadings)
            {
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
}
