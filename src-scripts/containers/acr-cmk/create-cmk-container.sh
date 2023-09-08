# Create rg
az group create --name my-container-rg --location eastus

# Create UAMI
az identity create --resource-group my-container-rg --name uami-acr-cmk
$identityId = $(az identity show --resource-group my-container-rg --name uami-acr-cmk --query 'id' -o tsv)
$identityPrincipalId = $(az identity show --resource-group my-container-rg --name uami-acr-cmk --query principalId -o tsv)

# Create Key Vault
az keyvault create --resource-group my-container-rg --name mykvaz2042023 --enable-purge-protection
$keyVaultId = $(az keyvault show --name mykvaz2042023 --resource-group my-container-rg --query id -o tsv)

# Permission
 az keyvault set-policy --resource-group my-container-rg --name mykvaz2042023 --object-id $identityPrincipalId --key-permission get unwrapKey wrapKey

# Create Key
az keyvault key create --name acr-key --vault-name mykvaz2042023
$keyId = $(az keyvault key show --name acr-key --vault-name mykvaz2042023 --query 'key.kid' -o tsv)

# Create ACR with cmk
az acr create --name myacraz2042023 --resource-group my-container-rg --location eastus --sku Premium --identity $identityId --key-encryption-key $keyId