using Microsoft.AspNetCore.Identity;

namespace ticket_selling_backend.Services;

// Traduce los mensajes de error de Identity al español
public class SpanishIdentityErrorDescriber : IdentityErrorDescriber
{
    public override IdentityError PasswordRequiresDigit() =>
        new() { Code = nameof(PasswordRequiresDigit), Description = "La contrasena debe contener al menos un numero (0-9)" };

    public override IdentityError PasswordRequiresUpper() =>
        new() { Code = nameof(PasswordRequiresUpper), Description = "La contrasena debe contener al menos una letra mayuscula (A-Z)" };

    public override IdentityError PasswordRequiresLower() =>
        new() { Code = nameof(PasswordRequiresLower), Description = "La contrasena debe contener al menos una letra minuscula (a-z)" };

    public override IdentityError PasswordRequiresNonAlphanumeric() =>
        new() { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "La contrasena debe contener al menos un caracter especial" };

    public override IdentityError PasswordTooShort(int length) =>
        new() { Code = nameof(PasswordTooShort), Description = $"La contrasena debe tener al menos {length} caracteres" };

    public override IdentityError DuplicateEmail(string email) =>
        new() { Code = nameof(DuplicateEmail), Description = $"El correo '{email}' ya esta registrado" };

    public override IdentityError DuplicateUserName(string userName) =>
        new() { Code = nameof(DuplicateUserName), Description = $"El usuario '{userName}' ya existe" };

    public override IdentityError InvalidEmail(string? email) =>
        new() { Code = nameof(InvalidEmail), Description = $"El correo '{email}' no es valido" };

    public override IdentityError DefaultError() =>
        new() { Code = nameof(DefaultError), Description = "Ocurrio un error inesperado" };
}
