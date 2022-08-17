terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "3.18.0"
    }
  }

  required_version = ">= 1.2.0"

  backend "azurerm" {
    resource_group_name  = "test-rg"
    storage_account_name = "__storateaccountname__"
    access_key="__storageaccountkey__"
    container_name       = "terraform"
    key                  = "terraform.tfstate"
  }

}

#Configure Azure Provider
provider "azurerm" {
  features {}
}