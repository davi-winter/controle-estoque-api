using InventoryControl.Domain.Interfaces.Repositories;
using System.Text.RegularExpressions;

namespace InventoryControl.Application.Validations
{
    public class UserValidation
    {
        private readonly IUserRepository _repository;

        public UserValidation(IUserRepository repository)
            => _repository = repository;

        public bool IsUsernameUnique(Guid id, string username)
        {
            var user = _repository.GetByUsernameAsync(id, username).Result;
            return user == null;
        }

        public bool IsEmailUnique(string email)
        {
            var user = _repository.GetByEmailAsync(email).Result;
            return user == null;
        }

        public bool IsValidEmailFormat(string email)
        {
            var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }

        public bool IsPasswordValid(string password)
        {
            // Mínimo de 8 caracteres, uma letra maiúscula, uma minúscula, um número e um caractere especial
            var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$");

            return regex.IsMatch(password);
        }
    }
}
