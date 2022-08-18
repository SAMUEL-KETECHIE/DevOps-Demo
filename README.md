# DevOps-Demo
A simple project to demonstrate IaC


## Application Environment Variables
```
CERT_CHECK_FROM_DAYS_MORE=15
ASPNETCORE_ENVIRONMENT="Development"
DOTNET_ENVIRONMENT="Development"
EXEC_HOUR=3

```

## Infra Pipeline Variables
```
TF_STATE_STORAGE_ACCOUNT_RG
TF_STATE_STORAGE_ACCOUNT_NAME
TF_STATE_STORAGE_ACCOUNT_KEY

---

RG_LOCATION
RG_NAME
CLUSTER_NAME
ACR_NAME
ENVIRONMENT

```

## Helm Chart Values
```

image.repository=$(IMAGE_REPO),image.tag=$(Release.Artifacts._ssl-checker-ci.BuildId),serviceAccount.name=$(CHART_NAME),nameOverride=$(CHART_NAME),fullnameOverride=$(CHART_NAME),config.ASPNETCORE_ENVIRONMENT=$(ASPNETCORE_ENVIRONMENT),config.DOTNET_ENVIRONMENT=$(DOTNET_ENVIRONMENT),config.CERT_CHECK_FROM_DAYS_MORE=$(CERT_CHECK_FROM_DAYS_MORE),config.EXEC_HOUR=$(EXEC_HOUR),config.DOMAINS=$(DOMAINS),replicaCount=$(REPLICAS)

```