- Create a ACR with whatever name, let call it myacraz20456rt
- Build docker image:
    > docker build -t myacraz20456rt.azurecr.io/ipcheck:v1 .
- Push the docker image to acr
    > az acr login -n myacraz20456rt
    > docker push myacraz20456rt.azurecr.io/ipcheck:v1
- Reference https://microsoftlearning.github.io/AZ-204-DevelopingSolutionsforMicrosoftAzure/Instructions/Labs/AZ-204_lab_05.html
