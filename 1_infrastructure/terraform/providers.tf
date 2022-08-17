terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "3.18.0"
    }
  }

  required_version = ">= 1.2.0"

  #backend for storing/saving tfstate file
  backend "azurerm" {
    resource_group_name  = "__TF_STATE_STORAGE_ACCOUNT_RG__"
    storage_account_name = "__TF_STATE_STORAGE_ACCOUNT_NAME__"
    access_key           = "__TF_STATE_STORAGE_ACCOUNT_KEY__"
    container_name       = "terraform"
    key                  = "terraform.tfstate"
  }

}

#Configure Azure Provider
provider "azurerm" {
  features {}
}