REM Create Group
az group create --name my-container-rg --location eastus

REM Create ACR
az acr create --name myacraz2042023 --resource-group my-container-rg --location eastus --sku Premium

REM Enable Admin
az acr update --name myacraz2042023 --admin-enabled true

REM Create Replica
az acr replication create --registry myacraz2042023 --resource-group my-container-rg --name southindiareplica --location southindia --region-endpoint-enabled true

REM Login to ACR
az acr login --name myacraz2042023

REM Build and Tag
docker build . -f ./AcrHelloworld/Dockerfile -t myacraz2042023.azurecr.io/acr-helloworld:v1

REM Push to ACR
docker push myacraz2042023.azurecr.io/acr-helloworld:v1