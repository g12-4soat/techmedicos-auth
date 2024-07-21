resource "aws_lambda_function" "tech_lanches_lambda_auth" {
  function_name = "tech-lanches-lambda-auth"
  filename      = "../../auth_lambda.zip"
  handler       = "TechLanchesLambda::TechLanchesLambda.Functions_LambdaAuth_Generated::LambdaAuth"
  runtime       = "dotnet8"
  role          = var.arn
  tags = {
    Name = "tech-lanches-lambda"
  }
  timeout     = 30
  memory_size = 512
}

resource "aws_lambda_function" "tech_lanches_lambda_cadastro" {
  function_name = "tech-lanches-lambda-cadastro"
  filename      = "../../auth_lambda.zip"
  handler       = "TechLanchesLambda::TechLanchesLambda.Functions_LambdaCadastro_Generated::LambdaCadastro"
  runtime       = "dotnet8"
  role          = var.arn
  tags = {
    Name = "tech-lanches-lambda"
  }
  timeout     = 30
  memory_size = 512
}


data "aws_lb" "eks_lb_api_pedido" {
  tags = {
    "kubernetes.io/service-name" = "techlanches/api-pedido-service"
  }
}

data "aws_lb" "eks_lb_api_pagamento" {
  tags = {
    "kubernetes.io/service-name" = "techlanches/api-pagamento-service"
  }
}

data "aws_lb" "eks_lb_api_producao" {
  tags = {
    "kubernetes.io/service-name" = "techlanches/api-producao-service"
  }
}

resource "aws_lambda_function" "tech_lanches_lambda_inativacao" {
  function_name = "tech-lanches-lambda-inativacao"
  filename      = "../../auth_lambda.zip"
  handler       = "TechLanchesLambda::TechLanchesLambda.Functions_LambdaInativacao_Generated::LambdaInativacao"
  runtime       = "dotnet8"
  role          = var.arn
  tags = {
    Name = "tech-lanches-lambda"
  }
  timeout     = 30
  memory_size = 512
   environment {
    variables = {
      PEDIDO_SERVICE    = data.aws_lb.eks_lb_api_pedido.dns_name
      PAGAMENTO_SERVICE  = data.aws_lb.eks_lb_api_pagamento.dns_name
    }
  }
}