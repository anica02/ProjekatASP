﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Application.UseCases.DTO
{
    public class CreateAuthorDto
    {
        public string? Pseudonym { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Country { get; set; }
    }

    public class UpdateAuthorDto: UpdateEntityDto
    {
   
        public string? Pseudonym { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
