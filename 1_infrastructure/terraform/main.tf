
#Create a resource group
resource "azurerm_resource_group" "rg" {
  name     = var.resource_group_name
  location = var.resource_group_location
}

#Create ACR
resource "azurerm_container_registry" "acr" {
  name                = var.container_registry_name
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  sku                 = "Standard"
}


# Datasource to get Azure AKS latest Version
data "azurerm_kubernetes_service_versions" "akscurrentversion" {
  location        = azurerm_resource_group.rg.location
  include_preview = false
}

#Create AKS Cluster
resource "azurerm_kubernetes_cluster" "aks" {
  name                = var.kubernetes_cluster_name
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  dns_prefix          = var.dns_prefix
  kubernetes_version  = data.azurerm_kubernetes_service_versions.akscurrentversion.latest_version

  default_node_pool {
    name                 = "linuxpool"
    max_count            = var.max_node_count
    min_count            = var.min_node_count
    vm_size              = var.linux_vmss_size
    max_pods             = var.max_pods
    type                 = "VirtualMachineScaleSets"
    enable_auto_scaling  = true
    orchestrator_version = data.azurerm_kubernetes_service_versions.akscurrentversion.latest_version
    os_disk_size_gb      = 30

    node_labels = {
      "nodepool-type" = "system"
      "environment"   = var.environment
      "nodepoolos"    = "linux"
      "app"           = "all-apps"
    }
    tags = {
      "nodepool-type" = "system"
      "environment"   = var.environment
      "nodepoolos"    = "linux"
      "app"           = "all-apps"
    }
  }

  identity {
    type = "SystemAssigned"
  }

  # Linux Profile
  #   linux_profile {
  #     admin_username = var.admin_username
  #     ssh_key {
  #       key_data = file(var.ssh_key)
  #     }
  #   }


  #Network profile: azure= Azure CNI, kubenet= KubeNet
  network_profile {
    network_plugin    = "kubenet"
    load_balancer_sku = "standard"
  }

  role_based_access_control_enabled = true


  tags = {
    Environment = var.environment
  }

}

#Assign Image pull role to the kubernetes kubelet identity
resource "azurerm_role_assignment" "ra" {
  principal_id                     = azurerm_kubernetes_cluster.aks.kubelet_identity[0].object_id
  role_definition_name             = "AcrPull"
  scope                            = azurerm_container_registry.acr.id
  skip_service_principal_aad_check = true
}