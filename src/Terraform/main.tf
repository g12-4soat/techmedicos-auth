module "lambda" {
  source = "./modules/lambda"
  arn    = data.aws_iam_role.name.arn
}

module "apiGateway" {
  source                 = "./modules/apiGateway"
  arn_lambda_auth        = module.lambda.lambda_arn_auth
  arn_lambda_cadastro    = module.lambda.lambda_arn_cadastro
  arn_lambda_inativacao  = module.lambda.lambda_arn_inativacao
  environment            = var.environment
  nome_lambda_auth       = module.lambda.nome_lambda_auth
  nome_lambda_cadastro   = module.lambda.nome_lambda_cadastro
  nome_lambda_inativacao = module.lambda.nome_lambda_inativacao
}

output "url_eks_gateway" {
  value = module.apiGateway.url_stage
}