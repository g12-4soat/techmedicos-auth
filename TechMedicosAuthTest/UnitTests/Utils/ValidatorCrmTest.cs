using TechMedicosAuth.Validations;
using TechMedicosAuthTest.UnitTests.Fixtures;

namespace TechMedicosAuthTest.UnitTests.Utils
{
    [Trait("Utils", "ValidatorCrm")]
    public class ValidatorCrmTest : IClassFixture<UtilsFixture>
    {
        private readonly UtilsFixture _utilsFixture;

        public ValidatorCrmTest(UtilsFixture utilsFixture)
        {
            _utilsFixture = utilsFixture;
        }

        [Fact(DisplayName = "Validador de crm efetuado com sucesso")]
        public void ValidatorCrm_Validar_DeveRetornarCrmComSucesso()
        {
            // Arrange
            string crm = _utilsFixture.GerarCrmValido();

            //// Act
            var res = ValidatorCrm.Validar(crm);

            //// Assert
            Assert.True(res);
            Assert.IsType<bool>(res);
        }

        [Fact(DisplayName = "Validador de crm inválido efetuado com falha")]
        public void ValidatorCrm_ValidarCrmInvalido_DeveRetornarCrmComFalha()
        {
            // Arrange
            string crm = _utilsFixture.GerarCrmInvalido();

            //// Act
            var res = ValidatorCrm.Validar(crm);

            //// Assert
            Assert.False(res);
            Assert.IsType<bool>(res);
        }

        [Fact(DisplayName = "Validador de crm vazio efetuado com falha")]
        public void ValidatorCrm_ValidarCrmVazio_DeveRetornarCrmComFalha()
        {
            // Arrange
            string fakeCrm = "";

            //// Act
            var res = ValidatorCrm.Validar(fakeCrm);

            //// Assert
            Assert.False(res);
            Assert.IsType<bool>(res);
        }

        [Fact(DisplayName = "Limpar Crm efetuado com sucesso")]
        public void LimparCrm_Limpar_DeveRetornarCrmLimpoComSucesso()
        {
            // Arrange
            string crm = _utilsFixture.GerarCrmValido();
            string crmLimpo = _utilsFixture.GerarCrmValidoELimpo();

            //// Act
            var res = ValidatorCrm.LimparCrm(crm);

            //// Assert
            Assert.Equal(res, crmLimpo);
            Assert.IsType<string>(res);
            Assert.NotEmpty(res);
            Assert.NotNull(res);
        }

        [Fact(DisplayName = "Limpar Crm efetuado com falha")]
        public void LimparCrm_Limpar_DeveRetornarCrmLimpoComFalha()
        {
            // Arrange
            string fakeCrm = null;

            // Act
            var res = ValidatorCrm.LimparCrm(fakeCrm);

            // Assert
            Assert.Equal(res, fakeCrm);
            Assert.Null(res);
        }
    }
}