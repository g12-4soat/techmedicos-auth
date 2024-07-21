﻿using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.Extensions.Options;
using TechMedicosAuth.DTOs;
using TechMedicosAuth.Utils;

namespace TechMedicosAuth.Service;

public class CognitoService : ICognitoService
{
    private readonly AWS.Options.AWSOptions _awsOptions;
    private readonly IAmazonCognitoIdentityProvider _client;
    private readonly IAmazonCognitoIdentityProvider _provider;

    public CognitoService(IOptions<AWS.Options.AWSOptions> awsOptions, IAmazonCognitoIdentityProvider client, IAmazonCognitoIdentityProvider provider)
    {
        _awsOptions = awsOptions.Value;
        _client = client;
        _provider = provider;
    }

    public CognitoService(IOptions<AWS.Options.AWSOptions> awsOptions)
    {
        ArgumentNullException.ThrowIfNull(awsOptions);
        _awsOptions = awsOptions.Value;

        _provider = new AmazonCognitoIdentityProviderClient(RegionEndpoint.GetBySystemName(_awsOptions.Region));
        _client = new AmazonCognitoIdentityProviderClient();
    }

    public async Task<Resultado> SignUp(PacienteDto user)
    {
        if (await UsuarioJaExiste(user))
        {
            var ehUsuarioPadrao = user.Cpf.Equals(_awsOptions.UserTechLanches);
            return ehUsuarioPadrao ? Resultado.Ok() : Resultado.Falha("Usuário já cadastrado. Por favor tente autenticar.");
        }

        try
        {
            var input = new SignUpRequest
            {
                ClientId = _awsOptions.UserPoolClientId,
                Username = user.Cpf,
                Password = user.Senha,
                UserAttributes = new List<AttributeType>
                {
                    new AttributeType {Name = "email", Value = user.Email },
                    new AttributeType {Name = "type_user", Value = "paciente" }
                }
            };

            var signUpResponse = await _client.SignUpAsync(input);

            if (signUpResponse.HttpStatusCode != System.Net.HttpStatusCode.OK)
                return Resultado.Falha("Houve algo de errado ao cadastrar o usuário.");

            var confirmRequest = new AdminConfirmSignUpRequest
            {
                Username = user.Cpf,
                UserPoolId = _awsOptions.UserPoolId
            };

            return await UsuarioConfirmado(confirmRequest);

        }
        catch (NotAuthorizedException)
        {
            return Resultado.Falha<TokenDto>("Usuário não autorizado para cadastro com os dados informadoss.");
        }
    }

    public async Task<Resultado<TokenDto>> SignIn(string userName, string password)
    {
        try
        {
            using var provider = _provider;
            var userPool = new CognitoUserPool(_awsOptions.UserPoolId, _awsOptions.UserPoolClientId, provider);
            var user = new CognitoUser(userName, _awsOptions.UserPoolClientId, userPool, provider);

            var authRequest = new InitiateAdminNoSrpAuthRequest
            {
                Password = password
            };

            var authResponse = await user.StartWithAdminNoSrpAuthAsync(authRequest);

            if (authResponse.AuthenticationResult != null)
                return Resultado.Ok(new TokenDto(authResponse.AuthenticationResult.IdToken, authResponse.AuthenticationResult.AccessToken));

            return Resultado.Falha<TokenDto>("Ocorreu um erro ao fazer loginn.");

        }
        catch (UserNotConfirmedException)
        {
            return Resultado.Falha<TokenDto>("Usuário não confirmado.");
        }
        catch (UserNotFoundException)
        {
            return Resultado.Falha<TokenDto>("Usuário não encontrado com os dados informados.");
        }
        catch (NotAuthorizedException)
        {
            return Resultado.Falha<TokenDto>("Usuário não autorizado com os dados informados.");
        }
    }

    private async Task<bool> UsuarioJaExiste(PacienteDto usuario)
    {
        try
        {
            var adminUser = new AdminGetUserRequest()
            {
                Username = usuario.Cpf,
                UserPoolId = _awsOptions!.UserPoolId
            };

            await _client.AdminGetUserAsync(adminUser);
            return true;
        }
        catch (UserNotFoundException)
        {
            return false;
        }
    }

    protected async Task<Resultado> UsuarioConfirmado(AdminConfirmSignUpRequest confirmRequest)
    {
        try
        {
            await _client.AdminConfirmSignUpAsync(confirmRequest);
            return Resultado.Ok();
        }
        catch (NotAuthorizedException)
        {
            return Resultado.Falha("Não foi possível confirmar o usuárioo.");
        }
    }
}