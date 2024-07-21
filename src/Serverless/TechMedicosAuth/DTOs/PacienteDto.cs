namespace TechMedicosAuth.DTOs;

public record class PacienteDto(string Cpf = "", string Email = "", string Senha = "");