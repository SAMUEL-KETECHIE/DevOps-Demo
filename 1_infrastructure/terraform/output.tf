
# Azure AKS Versions Datasource
output "versions" {
  value = data.azurerm_kubernetes_service_versions.akscurrentversion.versions
}

output "latest_version" {
  value = data.azurerm_kubernetes_service_versions.akscurrentversion.latest_version
}

output "aks_cluster_kubernetes_version" {
  value = azurerm_kubernetes_cluster.aks.kubernetes_version
}

output "aks_id" {
  value = azurerm_kubernetes_cluster.aks.id
}

#Container registry's ID
output "acr_id" {
  value = azurerm_container_registry.acr.id
}

#Container registry's login server
output "acr_login_server" {
  value = azurerm_container_registry.acr.login_server
}

