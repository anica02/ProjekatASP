﻿

using Bookstore.Application.UseCases.DTO;
using Bookstore.DataAccess;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bookstore.Implementation.Validators
{
    public class UpdateUserValidator:AbstractValidator<UpdateUserDto>
    {
      
        public UpdateUserValidator(BookstoreContext context)
        {
         
            RuleLevelCascadeMode = CascadeMode.Stop;
            Regex regexN = new Regex(@"^[A-ZĆČĐŽŠ]{1}[a-zćčđžš]{2,15}(\s[A-ZČĆŠĐŽ]{1}[a-zčćšđž]{2,15})*$");
            Regex regexU = new Regex(@"^[A-Za-z][A-Za-z0-9_]{7,29}$");

            RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required").Matches(regexU).WithMessage("Username should start with an alphabet. All other characters can be alphabets, numbers or an underscore. Length from 7-29 characters");

            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required").Matches(regexN);
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required").Matches(regexN);

            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required").EmailAddress();
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is reguired").MinimumLength(8).WithMessage("Password must be at least 8 characters long");
            RuleFor(x => x.RoleId).NotEmpty().WithMessage("Role id is required").Must(x => context.Roles.Any(r => r.Id == x && r.IsActive)).WithMessage("Role id does not exits");
        }
    }
}
