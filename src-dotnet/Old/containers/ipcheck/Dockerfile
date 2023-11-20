 # Start using the .NET 6 SDK container image
 FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

 # Change current working directory
 WORKDIR /app

 # Copy existing files from host machine
 COPY . ./

 # Publish application to the "out" folder
 RUN dotnet publish --configuration Release --output out

 # Start container by running application DLL
 ENTRYPOINT ["dotnet", "out/ipcheck.dll"]