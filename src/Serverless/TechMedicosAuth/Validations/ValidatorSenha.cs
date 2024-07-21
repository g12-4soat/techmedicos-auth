using System.Text.RegularExpressions;

namespace TechMedicosAuth.Validations;

public static class ValidatorSenha
{
    public static bool Validar(string senha)
    {
        if (string.IsNullOrWhiteSpace(senha))
            return false;

        // Padrão para validar senha com complexidade do Amazon Cognito
        string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[A-Za-z\d@$!%*?&]{8,}$";

        return Regex.IsMatch(senha, pattern, RegexOptions.None, TimeSpan.FromSeconds(30));
    }
}