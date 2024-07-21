using System.Text.RegularExpressions;

namespace TechMedicosAuth.Validations;

public static class ValidatorCrm
{
    public static bool Validar(string crm)
    {
        if (string.IsNullOrWhiteSpace(crm))
            return false;

        // Padrão para validar CRM no Brasil: 7 dígitos seguidos de um hífen e 2 letras maiúsculas
        string pattern = @"^\d{7}-[A-Z]{2}$";

        return Regex.IsMatch(crm, pattern, RegexOptions.None, TimeSpan.FromSeconds(30));
    }
}
