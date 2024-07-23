using TechMedicosAuth.Validations;
using TechMedicosAuthTest.UnitTests.Fixtures;

namespace TechMedicosAuthTest.UnitTests.Utils
{
    [Trait("Utils", "ValidatorSenha")]
    public class ValidatorSenhaTest : IClassFixture<UtilsFixture>
    {
        private readonly UtilsFixture _utilsFixture;

        public ValidatorSenhaTest(UtilsFixture utilsFixture)
        {
            _utilsFixture = utilsFixture;
        }

        [Fact(DisplayName = "Validador de senha efetuado com sucesso")]
        public void ValidatorSenha_Validar_DeveRetornarSenhaComSucesso()
        {
            // Arrange
            string senha = _utilsFixture.GerarSenhaValido();

            //// Act
            var res = ValidatorSenha.Validar(senha);

            //// Assert
            Assert.True(res);
            Assert.IsType<bool>(res);
        }

        [Fact(DisplayName = "Validador de senha inválido efetuado com falha")]
        public void ValidatorSenha_ValidarSenhaInvalido_DeveRetornarSenhaComFalha()
        {
            // Arrange
            string senha = _utilsFixture.GerarSenhaInvalido();

            //// Act
            var res = ValidatorTelefone.Validar(senha);

            //// Assert
            Assert.False(res);
            Assert.IsType<bool>(res);
        }

        [Fact(DisplayName = "Validador de senha vazio efetuado com falha")]
        public void ValidatorSenha_ValidarSenhaVazio_DeveRetornarSenhaComFalha()
        {
            // Arrange
            string fakeSenha = "";

            //// Act
            var res = ValidatorTelefone.Validar(fakeSenha);

            //// Assert
            Assert.False(res);
            Assert.IsType<bool>(res);
        }
    }
}