using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TechMedicosAuth.AWS.Options;
using TechMedicosAuth.DTOs;
using TechMedicosAuth.Service;
using TechMedicosAuth.Utils;
using TechMedicosAuth.Validations;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace TechMedicosAuth;

public class Functions
{
    public Functions()
    {
    }

    [LambdaFunction(Policies = "AWSLambdaBasicExecutionRole", MemorySize = 512, Timeout = 30)]
    [RestApi(LambdaHttpMethod.Post, "/auth")]
    public async Task<APIGatewayProxyResponse> TechMedicosAuth(APIGatewayProxyRequest request,
                                                  ILambdaContext context,
                                                  [FromServices] ICognitoService cognitoService,
                                                  [FromServices] IOptions<AWSOptions> awsOptions)
    {
        try
        {
            context.Logger.LogInformation("Handling the 'TechMedicosAuth' Request");
            ArgumentNullException.ThrowIfNull(awsOptions);

            var resLogin = Resultado.Falha<TokenDto>("Falha ao obter token");

            if (request.Body.ToLower().Contains("cpf"))
            {
                var paciente = ObterPaciente(request, awsOptions.Value);
                resLogin = await cognitoService.SignIn(paciente.Value.Cpf, paciente.Value.Senha);
            }
            else
            {
                var medico = ObterMedico(request, awsOptions.Value);
                resLogin = await cognitoService.SignIn(medico.Value.Crm, medico.Value.Senha);
            }

            if (!resLogin.Sucesso)
            {
                return Response.BadRequest(resLogin.Notificacoes);
            }

            var tokenResult = resLogin.Value;
            return !string.IsNullOrEmpty(tokenResult.AccessToken) ? Response.Ok(tokenResult) : Response.BadRequest("Não possui token"); 
        }
        catch (Exception ex)
        {
            Console.WriteLine("Auth Lambda response error: " + ex.Message);
            throw new Exception(ex.Message);
        }
    }

    [LambdaFunction(Policies = "AWSLambdaBasicExecutionRole", MemorySize = 512, Timeout = 30)]
    [RestApi(LambdaHttpMethod.Post, "/cadastro")]
    public async Task<APIGatewayProxyResponse> LambdaCadastro(APIGatewayProxyRequest request,
                                                  ILambdaContext context,
                                                  [FromServices] ICognitoService cognitoService,
                                                  [FromServices] IOptions<AWSOptions> awsOptions)
    {
        try
        {
            context.Logger.LogInformation("Handling the 'LambdaCadastro' Request");

            ArgumentNullException.ThrowIfNull(awsOptions);

            var usuario = ObterPaciente(request, awsOptions.Value);

            var resultadoValidacaoUsuario = new PacienteValidation().Validate(usuario.Value);
            if(!resultadoValidacaoUsuario.IsValid)
            {
                return Response.BadRequest(resultadoValidacaoUsuario.Errors.Select(x => new NotificacaoDto(x.ErrorMessage)).ToList());
            }
            
            var resultadoCadastroUsuario = await cognitoService.SignUp(usuario.Value);
            if (!resultadoCadastroUsuario.Sucesso)
            {
                return Response.BadRequest(resultadoCadastroUsuario.Notificacoes);
            }

            var resultadoLogin = await cognitoService.SignIn(usuario.Value.Cpf, usuario.Value.Senha);
            if (!resultadoLogin.Sucesso)
            {
                return Response.BadRequest(resultadoLogin.Notificacoes);
            }

            var tokenResult = resultadoLogin.Value;
            return !string.IsNullOrEmpty(tokenResult.AccessToken) ? Response.Ok(tokenResult) : Response.BadRequest("Não possui token");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Cadastro Lambda response error: " + ex.Message);
            throw new Exception(ex.Message);
        }
    }

    private Resultado<PacienteDto> ObterPaciente(APIGatewayProxyRequest request, AWSOptions awsOptions)
    {
        var pacienteDes = JsonConvert.DeserializeObject<PacienteDto>(request.Body) ?? new PacienteDto();
        ArgumentNullException.ThrowIfNull(pacienteDes);

        if (!CampoFoiInformado(pacienteDes.Cpf))
            return Resultado.Falha<PacienteDto>("Cpf inválido!");

        string cpf = pacienteDes.Cpf ?? string.Empty;
        string cpfLimpo = ValidatorCPF.LimparCpf(cpf);
        string email = pacienteDes.Email ?? string.Empty;
        string senha = pacienteDes.Senha ?? string.Empty;
        var paciente = new PacienteDto(string.IsNullOrEmpty(cpfLimpo) ? cpf : cpfLimpo, email, senha);
        return Resultado.Ok(paciente); 
    }

    private Resultado<MedicoDto> ObterMedico(APIGatewayProxyRequest request, AWSOptions awsOptions)
    {
        var medicoDes = JsonConvert.DeserializeObject<MedicoDto>(request.Body) ?? new MedicoDto();
        ArgumentNullException.ThrowIfNull(medicoDes);

        if (!CampoFoiInformado(medicoDes.Crm))
            return Resultado.Falha<MedicoDto>("Crm inválido!");

        string crm = medicoDes.Crm ?? string.Empty;
        string senha = medicoDes.Senha ?? string.Empty;
        var medico = new MedicoDto(crm, senha);
        return Resultado.Ok(medico);
    }

    private bool CampoFoiInformado(string campo)
    {
        return !string.IsNullOrEmpty(campo) && !string.IsNullOrWhiteSpace(campo);
    }
}