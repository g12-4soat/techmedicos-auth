using Amazon.CognitoIdentityProvider.Model;
using TechMedicosAuth.DTOs;

namespace TechMedicosAuthTest.BDDTests.Fixtures
{
    public class SignInFixture : IDisposable
    {
        private const string MENSAGEM_USUARIO_NAO_CONFIRMADO = "Usuário não confirmado.";
        private const string MENSAGEM_USUARIO_NAO_AUTORIZADO = "Usuário não autorizado com os dados informados.";
        private const string MENSAGEM_USUARIO_NAO_ENCONTRADO = "Usuário não encontrado com os dados informados.";
        private const string MENSAGEM_USUARIO_INVALIDO = "Ocorreu um erro ao fazer login.";

        public SignInFixture() { }

        public PacienteDto GerarUsuario()
        {
            return new PacienteDto { Cpf = "958.315.760-00", Email = "test@example.com", Senha = "tzHWCAfzR7" };
        }

        public PacienteDto GerarUsuarioTechLanches()
        {
            return new PacienteDto { Cpf = "techlanches", Email = "techlanches@example.com", Senha = "tzHWCAfzR7" };
        }

        public TokenDto GerarToken()
        {
            var authResultType = new AuthenticationResultType()
            {
                IdToken = "lTgRzbKewhYrLUCTE7CCsWJOPP7avkKXWLcwhLja8p9IGjmsiXfy6LeJft5smCHH",
                AccessToken = "AtjOELlJfZQmSoPKYTvJutLX9iA5rRarpe9Oy9sd0lbyR2tAH3RWDr2zSjcCuAIl"
            };

            return new TokenDto(authResultType.IdToken, authResultType.AccessToken);
        }

        public string ObterMensagemFalha(string nomeMensagem)
        {
            switch (nomeMensagem.ToLower())
            {
                case "usuario_nao_confirmado": return MENSAGEM_USUARIO_NAO_CONFIRMADO;
                case "usuario_nao_autorizado": return MENSAGEM_USUARIO_NAO_AUTORIZADO;
                case "usuario_nao_encontrado": return MENSAGEM_USUARIO_NAO_ENCONTRADO;
                case "usuario_invalido": return MENSAGEM_USUARIO_INVALIDO;
                default: return MENSAGEM_USUARIO_INVALIDO;
            }
        }

        public void Dispose()
        {
        }
    }
}
