variable "resource_group_location" {
  type        = string
  description = "resource location"
  default     = "East Us"
}

variable "resource_group_name" {
  type        = string
  description = "resource group name"
}

variable "linux_vmss_size" {
  type        = string
  description = "Linux pool Virtual Machine size name"
}


variable "kubernetes_cluster_name" {
  type        = string
  description = "kubernetes cluster name"
}


variable "kubernetes_version" {
  type        = string
  description = "k8's version"
  default     = "1.23.9"
}

variable "container_registry_name" {
  type        = string
  description = "container registry name"
}

variable "max_pods" {
  type        = number
  description = "maximum number of pods to be scheduled on a node"
}

variable "max_node_count" {
  type        = number
  description = "maximum number of nodes for the pools"
}

variable "min_node_count" {
  type        = number
  description = "minimum number of nodes for the pools"
}

variable "admin_username" {
  type        = string
  description = "admin username for linux and windows profiles"
  default     = "testuser"
}


#export your ssh key into a TF var e.g. export TF_VAR_ssh_key=$( cat ~/.ssh/id_rsa.pub) NOTE the prefix TF_VAR_
variable "ssh_key" {
  description = "ssh_key for admin_username"
}

variable "environment" {
  type        = string
  description = "infrastructure environment"
  default     = "Development"
}


variable "dns_prefix" {
  type        = string
  description = "dns prefix for kubernetes cluster"
  default     = "test-ask"
}