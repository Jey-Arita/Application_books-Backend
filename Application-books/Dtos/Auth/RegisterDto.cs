﻿using System.ComponentModel.DataAnnotations;

namespace Application_books.Dtos.Auth
{
    public class RegisterDto
    {
        [StringLength(70, MinimumLength = 3,
            ErrorMessage = "Los {0} no puede tener mas de {1} y menos de {2} caracteres")]
        [Display(Name = "Nombres")]
        [Required(ErrorMessage = "Los {0} es requerido")]
        public string FirstName { get; set; }

        [StringLength(70, MinimumLength = 3,
            ErrorMessage = "Los {0} no puede tener mas de {1} y menos de {2} caracteres")]
        [Display(Name = "Apellidos")]
        [Required(ErrorMessage = "Los {0} es requerido")]
        public string LastName { get; set; }

        [Display(Name = "Correo Electrónico")]
        [Required(ErrorMessage = "EL campo {0} es requerido.")]
        [EmailAddress(ErrorMessage = "El campo {0} no es valido.")]
        public string Email { get; set; }

        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        // 1 mayuscula, 1 minuscula, 1 caracter especial, 1 numero, sea mayor a 8 caracteres
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "La contraseña debe ser segura y contener al menos 8 caracteres, incluyendo minúsculas, mayúsculas, números y caracteres especiales.")]
        public string Password { get; set; }

        [Display(Name = "Confirmar contraseña")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Compare(nameof(Password), ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmPassword { get; set; }
    }
}
