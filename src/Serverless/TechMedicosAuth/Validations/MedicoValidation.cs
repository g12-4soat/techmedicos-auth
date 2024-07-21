using FluentValidation;
using TechMedicosAuth.DTOs;

namespace TechMedicosAuth.Validations;

public class MedicoValidation : AbstractValidator<MedicoDto>
{
    public MedicoValidation()
    {
        RuleFor(x => x.Crm)
           .NotEmpty().WithMessage("Crm deve ser informado.")
           .NotNull().WithMessage("Crm deve ser informado.");

        When(x => !string.IsNullOrEmpty(x.Crm) && !string.IsNullOrWhiteSpace(x.Crm), () =>
        {
            RuleFor(x => x.Crm)
            .Must(ValidatorCrm.Validar).WithMessage("O crm informado está inválido.");
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