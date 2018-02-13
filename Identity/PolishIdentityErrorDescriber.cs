using Microsoft.AspNetCore.Identity;

namespace Atut.Identity
{
    public class PolishIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DefaultError() { return new IdentityError { Code = nameof(DefaultError), Description = $"Wystąpił nieznany błąd." }; }
        public override IdentityError ConcurrencyFailure() { return new IdentityError { Code = nameof(ConcurrencyFailure), Description = "Optimistic concurrency failure, object has been modified." }; }
        public override IdentityError PasswordMismatch() { return new IdentityError { Code = nameof(PasswordMismatch), Description = "Niepoprawne hasło." }; }
        public override IdentityError InvalidToken() { return new IdentityError { Code = nameof(InvalidToken), Description = "Niepoprawny token." }; }
        public override IdentityError LoginAlreadyAssociated() { return new IdentityError { Code = nameof(LoginAlreadyAssociated), Description = "Użytkownik z takim loginem już istnieje." }; }
        public override IdentityError InvalidUserName(string userName) { return new IdentityError { Code = nameof(InvalidUserName), Description = $"Użytkownik '{userName}' jest niepoprawny, może zawierać jedynie litery i cyfry." }; }
        public override IdentityError InvalidEmail(string email) { return new IdentityError { Code = nameof(InvalidEmail), Description = $"Email '{email}' jest niepoprawny." }; }
        public override IdentityError DuplicateUserName(string userName) { return new IdentityError { Code = nameof(DuplicateUserName), Description = $"Nazwa użytkownika '{userName}' jest już zajęta." }; }
        public override IdentityError DuplicateEmail(string email) { return new IdentityError { Code = nameof(DuplicateEmail), Description = $"Email '{email}' jest już zajęty." }; }
        public override IdentityError InvalidRoleName(string role) { return new IdentityError { Code = nameof(InvalidRoleName), Description = $"Rola '{role}' jest niepoprawna." }; }
        public override IdentityError DuplicateRoleName(string role) { return new IdentityError { Code = nameof(DuplicateRoleName), Description = $"Rola '{role}' jest już zajęta." }; }
        public override IdentityError UserAlreadyHasPassword() { return new IdentityError { Code = nameof(UserAlreadyHasPassword), Description = "Użytkownik ma już ustawione hasło." }; }
        public override IdentityError UserLockoutNotEnabled() { return new IdentityError { Code = nameof(UserLockoutNotEnabled), Description = "Lockout is not enabled for this user." }; }
        public override IdentityError UserAlreadyInRole(string role) { return new IdentityError { Code = nameof(UserAlreadyInRole), Description = $"Użytkownik ma już przypisaną '{role}'." }; }
        public override IdentityError UserNotInRole(string role) { return new IdentityError { Code = nameof(UserNotInRole), Description = $"Użytkownika nie posiada przypisanej roli '{role}'." }; }
        public override IdentityError PasswordTooShort(int length) { return new IdentityError { Code = nameof(PasswordTooShort), Description = $"Hasło musi zawierać przynajmniej {length} znaków." }; }
        public override IdentityError PasswordRequiresNonAlphanumeric() { return new IdentityError { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "Hasło musi zawierać przynajmniej jeden znak niealfanumeryczny." }; }
        public override IdentityError PasswordRequiresDigit() { return new IdentityError { Code = nameof(PasswordRequiresDigit), Description = "Hasło musi zawierać przynajmniej jedną cyfrę ('0'-'9')." }; }
        public override IdentityError PasswordRequiresLower() { return new IdentityError { Code = nameof(PasswordRequiresLower), Description = "Hasło musi zawierać przynajmniej jedną małą literę ('a'-'z')." }; }
        public override IdentityError PasswordRequiresUpper() { return new IdentityError { Code = nameof(PasswordRequiresUpper), Description = "Hasło musi zawierać przynajmniej jedną wielką literę ('A'-'Z')." }; }
    }
}
