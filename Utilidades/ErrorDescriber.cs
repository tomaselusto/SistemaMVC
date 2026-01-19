using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Utilidades
{
    public class ErrorDescriber :IdentityErrorDescriber
    {
        public override IdentityError PasswordRequiresLower()
        {
            return new IdentityError()
            {
                Code = nameof(PasswordRequiresLower),
                Description = "El password debe tener al menos una letra minúscula"
            };
        }
    }
}
