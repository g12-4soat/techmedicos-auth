terraform {
  required_version = ">= 0.13"

  required_providers {
    aws = ">= 3.0"
    random = ">= 3.0"
  }

  backend "s3" {
    bucket = "tech-lanches-terraform"
    key    = "tech-lanches-terraform-gateway/terraform.tfstate"
    region = "us-east-1"
  }
}

provider "aws" {  
  region = var.region
}