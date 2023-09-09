az group create --name my-aci-grp-rg --location eastus

az container create --resource-group my-aci-grp-rg --file deploy-aci.yaml

az container show --resource-group my-aci-grp-rg --name myContainerGroup --output table

az container delete --resource-group my-aci-grp-rg --name myContainerGroup

az group delete --name my-aci-grp-rg