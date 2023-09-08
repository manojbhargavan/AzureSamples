# Create the holding resource group
az group create --name my-container-rg --location eastus

# Create the container registry
az acr create --name myacraz20423 --resource-group my-container-rg --location eastus --sku Basic

# Login to the portal and enable admin
# sudo -i and az login --use-device-login
# az account set --subscription ***
# Login to the registry
az acr login --name myacraz20423

# Pull a sample image and tag
docker pull docker/getting-started
docker tag docker/getting-started myacraz20423.azurecr.io/docker-getting-started:v1

# Push to ACR
docker push myacraz20423.azurecr.io/docker-getting-started:v1

# Remove image from the local 
docker rmi myacraz20423.azurecr.io/docker-getting-started:v1

# List containers in acr registry
az acr repository list --name myacraz20423
az acr repository list --name myacraz20423 -o table

az acr repository show-tags --name myacraz20423 --repository docker-getting-started -o table

# Run it locally
docker run -d -p 80:80 myacraz20423.azurecr.io/docker-getting-started:v1

# Cleanup
az group delete --name my-container-rg