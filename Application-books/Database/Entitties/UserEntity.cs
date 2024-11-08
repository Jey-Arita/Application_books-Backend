using Application_books.Database.Entitties;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application_books.Database.Entities
{
    public class UserEntity : IdentityUser // `Id` será de tipo `string`, heredado de `IdentityUser`
    {
        [StringLength(70, MinimumLength = 3)]
        [Column("first_name")]
        [Required]
        public string FirstName { get; set; }

        [StringLength(70, MinimumLength = 3)]
        [Column("last_name")]
        [Required]
        public string LastName { get; set; }

        [Column("refresh_token")]
        [StringLength(450)]
        public string RefreshToken { get; set; }

        [Column("refresh_token_expire")]
        public DateTime RefreshTokenExpire { get; set; }

        public ICollection<MembresiaEntity> Membresia { get; set; } = new List<MembresiaEntity>();
    }
}
