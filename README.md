# Library

## Running the project:

```
dotnet run .\Library.csproj
```

Open API specification (Swagger) will be available on address ```localhost:5000/index.json```

## Running the tests(run the command in the root where .sln file is available):

```
dotnet test
```

## NOTE: 
In memory database is used. If you want to test ```GET/PUT``` endpoints - first you need to create resource using the ```POST``` endpoint. Resource will be available during the runtime until
application is stopped
