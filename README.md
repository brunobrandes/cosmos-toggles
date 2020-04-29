# Cosmos Toggles
.NET tools to allowing teams to modify system behavior without changing code with Azure Cosmos DB.

### Introduction

>Feature Toggles (often also refered to as Feature Flags) are a powerful technique, allowing teams to modify 
system behavior without changing code. - Martin Fowler

Cosmos Toggles is a tool to allows that applications change behavior without new deployment through flags configuration 
with the [Azure Cosmos DB SQL API](https://bit.ly/3cEQKuP), a Microsoft's globally distributed with guaranteed speed and performance.

[![Global scale with Azure Cosmos DB](https://i.imgur.com/g8zAuTh.png)](https://docs.microsoft.com/en-us/azure/cosmos-db/introduction)

In March 6th, 2020 Microsoft pronounce new [Free Tier](https://bit.ly/3atWXbB) that allows get the first 400 RU/s throughput and 5 GB storage 100% free. 

["Beautiful docs for API reference - GitBook"](https://bruno-brandes.gitbook.io/cosmos-toggles/)

### Project Architecture

The Onion Architecture was created [by Jeffrey Palermo in 2008](https://jeffreypalermo.com/2008/07/the-onion-architecture-part-1/) and this project is based in this architectural pattern.

### Build and Test
How to build, explore and test application

#### Emulator
Use the **Azure Cosmos DB Emulator** for local development and testing.  You can download and install the Azure Cosmos Emulator from the [Microsoft Download Center](https://aka.ms/cosmosdb-emulator)

#### Build
Run **Cosmos.Toggles.Ui.Api.csproj** and explore the swagger to exemplify all application flow. Execute command in the *cosmos-toggles/src* project path:
```powershell
dotnet run --project ./Ui/Cosmos.Toggles.Ui.Api/Cosmos.Toggles.Ui.Api.csproj
```

#### Test
Download the [free Postman app to get started](https://www.postman.com/downloads/)

>Postman is a collaboration platform for API development. Postman's features simplify each step of building an API and streamline collaboration so you can create better APIs—faster.).

Open Postman and import Cosmos Toggles Collection from link:
```powershell
https://www.getpostman.com/collections/80a0e2627584345149ed
```

Run Postmand collection.

### Contributing

This project welcomes contributions and suggestions. Most contributions require you to agree to a MIT License.

When you submit a pull request, garantehave a unit test to coverage new feature and increse the postman collection with tests.

Looking forward to your contribution 🤓

### License

This software is open source, licensed under the Apache License. </br>
See [LICENSE.me](https://github.com/brunobrandes/cosmos-toggles/blob/master/LICENSE.me) for details.
