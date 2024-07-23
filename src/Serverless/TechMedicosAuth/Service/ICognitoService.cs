using Amazon.Runtime.Internal;
using TechMedicosAuth.DTOs;
using TechMedicosAuth.Utils;

namespace TechMedicosAuth.Service
{
    public interface ICognitoService
    {
        Task<Resultado> SignUp(PacienteDto usuario);
        Task<Resultado<TokenDto>> SignIn(string userName, string password);
    }
}
