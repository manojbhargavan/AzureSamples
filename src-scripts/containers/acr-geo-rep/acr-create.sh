# Create Group
az group create --name my-container-rg --location eastus

# Create ACR
az acr create --name myacraz2042023 --resource-group my-container-rg --location eastus --sku Premium

# Enable Admin
az acr update --name myacraz2042023 --admin-enabled true

# Create Replica
az acr replication create --registry myacraz2042023 --resource-group my-container-rg --name southindiareplica --location southindia --region-endpoint-enabled true

# Login to ACR
az acr login --name myacraz2042023

# Build and Tag
docker build . -f ./AcrHelloworld/Dockerfile -t myacraz2042023.azurecr.io/acr-helloworld:v1

# Push to ACR
docker push myacraz2042023.azurecr.io/acr-helloworld:v1