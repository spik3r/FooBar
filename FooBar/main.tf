# Configure the Azure provider
terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.0.0"
    }
  }
  required_version = ">= 0.14.9"
}

provider "azurerm" {
  features {}
}

# Create a Resource Group
resource "azurerm_resource_group" "foobar" {
  name     = "foobar-resource-group"
  location = "Australia East"
}

# Create an Azure SQL Server
resource "azurerm_mssql_server" "foobar" {
  name                         = "foobarsqlserver"
  resource_group_name          = azurerm_resource_group.foobar.name
  location                     = azurerm_resource_group.foobar.location
  administrator_login          = "foobaradmin"
  administrator_login_password = "YourStrong!Passw0rd"
  version                      = "12.0"
}

# Create an Azure SQL Database
resource "azurerm_mssql_database" "foobar" {
  name                = "foobardatabase"
  server_id           = azurerm_mssql_server.foobar.id
  sku_name            = "S0"
}

# Allow access from a fixed public IP
resource "azurerm_mssql_firewall_rule" "allow_all_ip" {
  name                = "AllowAllIP"
  start_ip_address    = "0.0.0.0"  # Open to all for simplicity; refine as needed
  end_ip_address      = "255.255.255.255"  # Open to all for simplicity; refine as needed
  server_id           = azurerm_mssql_server.foobar.id
}

# Allow access from your local IP address
# resource "azurerm_mssql_firewall_rule" "allow_my_ip" {
#   name                = "AllowMyIP"
#   start_ip_address    = "101.182.140.219"  # Replace with your local IP
#   end_ip_address      = "101.182.140.219"   # Replace with your local IP
#   server_id           = azurerm_mssql_server.foobar.id
# }

# Create an Azure Container Group for the Docker Compose
resource "azurerm_container_group" "foobar" {
  name                = "foobarcontainergroup"
  location            = azurerm_resource_group.foobar.location
  resource_group_name = azurerm_resource_group.foobar.name
  os_type             = "Linux"

  container {
    name   = "webapp"
    image  = "kaiftait/foobar:latest"  # Change to your Docker Hub image
    cpu    = "0.5"
    memory = "1.5"

    ports {
      port     = 5056
      protocol = "TCP"
    }

    environment_variables = {
      ASPNETCORE_ENVIRONMENT = "Development"
      ConnectionStrings__AzureConnection = "Server=tcp:${azurerm_mssql_server.foobar.name}.database.windows.net,1433;Initial Catalog=${azurerm_mssql_database.foobar.name};Persist Security Info=False;User ID=foobaradmin;Password=YourStrong!Passw0rd;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
    }
  }

  tags = {
    environment = "development"
  }
}
