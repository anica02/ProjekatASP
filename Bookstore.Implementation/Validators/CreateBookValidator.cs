
using Bookstore.DataAccess;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bookstore.Application.UseCases.DTO;

namespace Bookstore.Implementation.Validators
{
    public class CreateBookValidator:AbstractValidator<CreateBookDto>
    {
       
        public CreateBookValidator(BookstoreContext context)
        {
           
            RuleLevelCascadeMode = CascadeMode.Stop;
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");
            RuleFor(x => x.Code).NotEmpty().WithMessage("Code is required").Must(x => !context.Books.Any(b => b.Code == x)).WithMessage("Code already exist");

            RuleForEach(x => x.BookAuthors).SetValidator(new BookAuthorValidator(context));

            RuleForEach(x => x.BookGenres).SetValidator(new BookGenreValidator(context));
            RuleFor(x => x.BookPublisher).SetValidator(new CreateBookPublisherValidator(context));
            
        }
    }


    public class BookAuthorValidator : AbstractValidator<CreateBookAuthorDto>
    {
      
        public BookAuthorValidator(BookstoreContext context)
        {
       

            RuleFor(x => x.AuthorId).NotEmpty().WithMessage("Author id is required").Must(x => context.Authors.Any(a => a.Id == x && a.IsActive)).WithMessage("Author id does not exist");

        }
    }

    public class BookGenreValidator : AbstractValidator<CreateBookGenreDto>
    {
       
        public BookGenreValidator(BookstoreContext context)
        {

            RuleFor(x => x.GenreId).NotEmpty().WithMessage("Genre id is required").Must(x => context.Genres.Any(g => g.Id == x && g.IsActive)).WithMessage("Genre id does not exist");

        }
    }
    
   
}
