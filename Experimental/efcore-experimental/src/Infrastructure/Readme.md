# Entity Framework Commands

## Adding migrations

    dotnet ef migrations add -p  ./src/Infrastructure/Tsmoreland.EntityFramework.Core.Experimental.Infrastructure.csproj -c PeopleDbContext {Migration Name}

-c content is optional, unless the project contains more than one DbContext implementation.

This does assume a ```DesignTimeDbContextFactory<PeopleDbContext>``` implementation exists, otherwise
a ```-s {start up project}``` is required to specify the application project

## Optmize

    dotnet ef dbcontext optimize -p ./src/Infrastructure/Tsmoreland.EntityFramework.Core.Experimental.Infrastructure.csproj    

This will generated an optimized db context model with generated code which avoids the use of
reflection
