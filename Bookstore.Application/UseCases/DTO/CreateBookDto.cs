using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Application.UseCases.DTO
{
    public class CreateBookDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Code { get; set; }
        public IEnumerable<CreateBookAuthorDto> BookAuthors { get; set; } = new List<CreateBookAuthorDto>();
        public IEnumerable<CreateBookGenreDto> BookGenres { get; set; } =new List<CreateBookGenreDto>();

        public CreateBookPublisherDto BookPublisher { get; set; }

    }

    public class CreateBookAuthorDto { 
        public int? AuthorId { get; set; }
    }

    public class CreateBookGenreDto
    {
        public int? GenreId { get; set; }
    }

    public class CreateBookPriceDto
    {
        public int? BookPublisherId { get; set; }
        public decimal BookPrice { get; set; }
    }

    public class UpdateBookDto: UpdateEntityDto
    {
     
        public string Name { get; set; }
        public string Description { get; set; }
        public int Code { get; set; }
        public int Year { get; set; }
        public IEnumerable<CreateBookAuthorDto> BookAuthors { get; set; } = new List<CreateBookAuthorDto>();
        public IEnumerable<CreateBookGenreDto> BookGenres { get; set; } = new List<CreateBookGenreDto>();
       
      
    }



}
