# Azure Functions - Cosmos DB Real-time Update Demo

A simple demo of broadcasting real-time updates from Azure Blob or Cosmos DB over websockets. 

Uses:

* Azure Cosmos DB Change Feed
* Azure SignalR Service
* Azure Functions bindings:
    - Http Triggers for fetching SignalR connection Info
    - Azure Blob Trigger
    - Cosmos DB trigger
    - SignalR Service bindings

Usage:
1. Update the Azure Blob connection and container details in app settings to point nonin-3230 day readings container (or) Update the cosmos connection and collection details in app settings if you want ot test on cosmos feed.
2. run the functions project
3. Deploy html page to localhost
4. On page load the client will fetch all existing readings
5. Send a valid allocated nonin message to gateway 
6. The new readings will be rendered on client automatically. 
