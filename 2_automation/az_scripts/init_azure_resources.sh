#Craete Azure resource group, storage account and container  for terraform state

call az group create --location $(RG_LOCATION) -n $(TF_STATE_STORAGE_ACCOUNT_RG)

call az storage account create -n $(TF_STATE_STORAGE_ACCOUNT_NAME) -g $(TF_STATE_STORAGE_ACCOUNT_RG) --location $(RG_LOCATION) --sku Standard_LRS

call az storage container create -n terraform --account-name $(TF_STATE_STORAGE_ACCOUNT_NAME)

call az storage account keys list -g $(TF_STATE_STORAGE_ACCOUNT_RG) -n $(TF_STATE_STORAGE_ACCOUNT_NAME)