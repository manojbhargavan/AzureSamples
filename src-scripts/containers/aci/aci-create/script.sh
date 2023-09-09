az group create --name my-acr-rg --location eastus
az acr create --resource-group my-acr-rg --name myacrregaz2042023 --location eastus --sku Basic
az acr show --name myacrregaz2042023 --query loginServer -o tsv

sudo az acr login --name myacrregaz2042023
sudo docker build . -t myacrregaz2042023.azurecr.io/aci-tutorial-app:v1
sudo docker push myacrregaz2042023.azurecr.io/aci-tutorial-app:v1

az ad sp create-for-rbac --name myacisp --role acrpull --scope "/subscriptions/***/resourceGroups/my-acr-rg/providers/Microsoft.ContainerRegistry/registries/myacrregaz2042023"

{
  "appId": "0fa4db85-***,
  "displayName": "myacisp",
  "password": "ghO8Q~***,
  "tenant": "e37e357b-b501-4093-9e9d-663258b47091"
}

az container create --resource-group my-acr-rg --name mytutaciaz2042023 --location eastus --cpu 1 --memory 1 --dns-name-label mytutaciaz2042023 --ip-address Public --os-type Linux --image myacrregaz2042023.azurecr.io/aci-tutorial-app:v1 --registry-login-server myacrregaz2042023.azurecr.io --registry-username "0fa4db85-***" --registry-password "ghO8Q~***" --ports 80

az container logs --name mytutaciaz2042023 --resource-group my-acr-rg

az group delete --name my-acr-rg