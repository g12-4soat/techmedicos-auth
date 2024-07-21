using FluentValidation;
using TechMedicosAuth.DTOs;

namespace TechMedicosAuth.Validations;

public class PacienteValidation : AbstractValidator<PacienteDto>
{
    public PacienteValidation()
    {
        RuleFor(x => x.Cpf)
           .NotEmpty().WithMessage("CPF deve ser informado.")
           .NotNull().WithMessage("CPF deve ser informado.");

        When(x => !string.IsNullOrEmpty(x.Cpf) && !string.IsNullOrWhiteSpace(x.Cpf), () =>
        {
            RuleFor(x => x.Cpf)
            .Must(ValidatorCPF.Validar).WithMessage("O CPF informado está inválido.");
        });

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email deve ser informado.")
            .NotNull().WithMessage("Email deve ser informado.");

        When(x => !string.IsNullOrEmpty(x.Email) && !string.IsNullOrWhiteSpace(x.Email), () =>
        {
            RuleFor(x => x.Email)
            .Must(ValidatorEmail.Validar).WithMessage("O Email informado está inválido.");
        });

        RuleFor(x => x.Senha)
           .NotEmpty().WithMessage("Senha deve ser informado.")
           .NotNull().WithMessage("Senha deve ser informado.");

        When(x => !string.IsNullOrEmpty(x.Senha) && !string.IsNullOrWhiteSpace(x.Senha), () =>
        {
            RuleFor(x => x.Senha)
            .Must(ValidatorSenha.Validar).WithMessage("Senha informada está inválido.");
        });
    }
}