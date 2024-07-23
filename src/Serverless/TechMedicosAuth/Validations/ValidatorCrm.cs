using System.Text.RegularExpressions;

namespace TechMedicosAuth.Validations;

public static class ValidatorCrm
{
    public static bool Validar(string crm)
    {
        if (string.IsNullOrWhiteSpace(crm))
            return false;

        crm = LimparCrm(crm);

        // Padrão para validar CRM no Brasil
        string pattern = @"^\d+[A-Za-z]+$";

        return Regex.IsMatch(crm, pattern, RegexOptions.None, TimeSpan.FromSeconds(30));
    }

    public static string LimparCrm(string crm)
    {
        try
        {
            return Regex.Replace(crm.Trim(), @"[^\w]", "", RegexOptions.None, TimeSpan.FromSeconds(30));
        }
        catch (Exception)
        {
            return null;
        }
    }
}