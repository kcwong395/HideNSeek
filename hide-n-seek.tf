terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "=2.75.0"
    }
  }
}

provider "azurerm" {
    features {}
}

resource "azurerm_resource_group" "hide_n_seek" {
    name        = "HideNSeek"
    location    = "East US"
}

resource "azurerm_container_group" "hide_n_seek_containers" {
  name                = "HideNSeekContainer"
  location            = azurerm_resource_group.hide_n_seek.location
  resource_group_name = azurerm_resource_group.hide_n_seek.name
  ip_address_type     = "public"
  os_type             = "Linux"

  container {
    name   = "backend"
    image  = "kcwong395/hidenseek_hns-backend:latest"
    cpu    = "0.5"
    memory = "0.5"
  }

  container {
    name   = "frontend"
    image  = "kcwong395/hidenseek_hns-frontend:latest"
    cpu    = "0.5"
    memory = "0.5"

    ports {
      port     = 80
      protocol = "TCP"
    }
  }
}