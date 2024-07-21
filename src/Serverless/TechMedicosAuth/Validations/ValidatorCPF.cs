﻿using System.Text.RegularExpressions;

namespace TechMedicosAuth.Validations;

public static class ValidatorCPF
{
    public static bool Validar(string cpf)
    {
        // Remove caracteres não numéricos
        string cpfLimpo = LimparCpf(cpf).Trim();

        // Verifica se o CPF tem 11 dígitos
        if (cpfLimpo.Length != 11)
        {
            return false;
        }

        // Calcula o primeiro dígito verificador
        int sum = 0;
        for (int i = 0; i < 9; i++)
        {
            sum += int.Parse(cpfLimpo[i].ToString()) * (10 - i);
        }
        int remainder = sum % 11;
        int firstDigit = remainder < 2 ? 0 : 11 - remainder;

        // Verifica o primeiro dígito verificador
        if (int.Parse(cpfLimpo[9].ToString()) != firstDigit)
        {
            return false;
        }

        // Calcula o segundo dígito verificador
        sum = 0;
        for (int i = 0; i < 10; i++)
        {
            sum += int.Parse(cpfLimpo[i].ToString()) * (11 - i);
        }
        remainder = sum % 11;
        int secondDigit = remainder < 2 ? 0 : 11 - remainder;

        // Verifica o segundo dígito verificador
        return int.Parse(cpfLimpo[10].ToString()) == secondDigit;
    }

    public static string LimparCpf(string cpf)
    {
        try
        {
            return Regex.Replace(cpf.Trim(), @"[^\d]", "", RegexOptions.None, TimeSpan.FromSeconds(30));
        }
        catch (Exception)
        {
            return null;
        }
    }
}
