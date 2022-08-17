# Fetch storage account key and set it to a variable. Use it for authenticating the backend for TF 

$key=(Get-AzureRmStorageAccountKey -ResourceGroupName $(TF_STATE_STORAGE_ACCOUNT_RG) -AccountName $(TF_STATE_STORAGE_ACCOUNT_NAME)).Value[0]

Write-Host "##vso[task.setvariable variable=TF_STATE_STORAGE_ACCOUNT_KEY]$key"

