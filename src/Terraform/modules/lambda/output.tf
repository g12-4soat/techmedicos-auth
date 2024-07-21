output "lambda_arn_auth" {
  description = "ARN da lambda"
  value       = aws_lambda_function.tech_lanches_lambda_auth.invoke_arn
}

output "lambda_arn_cadastro" {
  description = "ARN da lambda"
  value       = aws_lambda_function.tech_lanches_lambda_cadastro.invoke_arn
}

output "lambda_arn_inativacao" {
  description = "ARN da lambda"
  value       = aws_lambda_function.tech_lanches_lambda_inativacao.invoke_arn
}

output "nome_lambda_auth" {
  description = "Nome da Lambda Auth"
  value       = aws_lambda_function.tech_lanches_lambda_auth.function_name
}

output "nome_lambda_cadastro" {
  description = "Nome da Lambda Cadastro"
  value       = aws_lambda_function.tech_lanches_lambda_cadastro.function_name
}

output "nome_lambda_inativacao" {
  description = "Nome da Lambda Inativação"
  value       = aws_lambda_function.tech_lanches_lambda_inativacao.function_name
}