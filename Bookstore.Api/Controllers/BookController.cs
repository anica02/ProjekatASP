using FluentValidation.Results;
using Bookstore.Application.UseCases.DTO;
using Bookstore.Application.UseCases.Commands;
using Bookstore.DataAccess;
using Bookstore.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Bookstore.API.DTO;
using Bookstore.Application.UseCaseHandiling;
using Bookstore.Application.UseCases.Queries;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bookstore.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
 
    public class BookController : ControllerBase
    {

        private ICommandHandler _commandHandler;
        private IQueryHandler _queryHandler;
        public BookController(ICommandHandler commandHandler, IQueryHandler queryHandler)
        {
            _queryHandler = queryHandler;
            _commandHandler = commandHandler;

        }
      

        //// GET: api/<BookController>
        //[HttpGet]
        //public IActionResult Get([FromQuery] BookSearch search)
        //{


        //        var query = _context.Books.AsQueryable();

        //        if (!string.IsNullOrEmpty(search.Name))
        //        {
        //            query = query.Where(x => x.Name.Contains(search.Name));
        //        }

        //        if (!string.IsNullOrEmpty(search.AuthorFirstName))
        //        {
        //            query = query.Where(x => x.BookAuthors.Any(b => b.Author.FirstName.ToLower() == search.AuthorFirstName.ToLower()));
        //        }

        //        if (!string.IsNullOrEmpty(search.AuthorLastName))
        //        {
        //            query = query.Where(x => x.BookAuthors.Any(b => b.Author.LastName.ToLower() == search.AuthorLastName.ToLower()));
        //        }

        //        if (!string.IsNullOrEmpty(search.AuthorPseudonym))
        //        {
        //            query = query.Where(x => x.BookAuthors.Any(b => b.Author.Pseudonym.ToLower() == search.AuthorPseudonym.ToLower()));
        //        }

        //        if (!string.IsNullOrEmpty(search.Genre))
        //        {
        //            query = query.Where(x => x.BookGenres.Any(b => b.Genre.Name.Contains(search.Genre)));
        //        }
        //        if (!string.IsNullOrEmpty(search.Publisher))
        //        {
        //            query = query.Where(x => x.BookPublishers.Any(b => b.Publisher.Name.ToLower() == search.Publisher.ToLower()));
        //        }

        //        if (search.WritinFromYear.HasValue && search.WritinToYear.HasValue)
        //        {
        //            query = query.Where(x => x.Year >= search.WritinFromYear.Value && x.Year <= search.WritinToYear.Value);
        //        }

        //        if (search.PublicationFromYear.HasValue && search.PublicationToYear.HasValue)
        //        {
        //            query = query.Where(x => x.BookPublishers.Any(b => b.Year >= search.PublicationFromYear.Value && b.Year <= search.PublicationToYear.Value));
        //        }

        //        IEnumerable<ReadBookDto> books = query.Select(x => new ReadBookDto
        //        {
        //            Id = x.Id,
        //            Name = x.Name,
        //            Description = x.Description,
        //            Code = x.Code,
        //            Year = x.Year,
        //            BookAuthors = x.BookAuthors.Select(a => new AuthorDto
        //            {
        //                Id = a.Author.Id,
        //                FirstName = a.Author.FirstName,
        //                LastName = a.Author.LastName,
        //                Pseudonym = a.Author.Pseudonym != null ? a.Author.Pseudonym : "none",
        //                Country = a.Author.Country,
        //                DateOfBirth = a.Author.DateOfBirth
        //            }),
        //            BookPublishers = x.BookPublishers.Select(p => new PublisherDto
        //            {
        //                Id=p.Id,
        //                PublisherId = p.PublisherId,
        //                PublisherName = p.Publisher.Name,
        //                NumberOfPages = p.NumberOfPages,
        //                BookCover = p.BookCover,
        //                BookFormat = p.BookFormat,
        //                BookWritingSystem = p.BookWritingSystem,
        //                Year = p.Year,
        //                Price = p.Prices.Where(x => x.IsActive).OrderByDescending(x => x.Id).Select(x => x.BookPrice).First(),
        //                Discounts = p.Discounts.Where(x => x.IsActive && x.StartsFrom <= DateTime.UtcNow && x.EndsAt >= DateTime.UtcNow).Select(d => new PriceDiscountDto
        //                {
        //                    Id=d.Id,
        //                    DiscountPercentage = d.DiscountPercentage,
        //                    StartsFrom = d.StartsFrom,
        //                    EndsAt = d.EndsAt
        //                }),
        //                Image=new ImageDto
        //                {
        //                    Id=p.Id,
        //                    Path=p.Image.Path,
        //                    Size=p.Image.Size
        //                }
        //            }),
        //            BookGenres = x.BookGenres.Select(g => new GenreDto
        //            {
        //                Id = g.Genre.Id,
        //                Name = g.Genre.Name,


        //            })
        //        }).ToList();


        //        return Ok(books);


        //}


        // GET api/<BookController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id, [FromServices] IFindBookQuery query)
        {
            return Ok(_queryHandler.HandleQuery(query, id));
        }

        // POST api/<BookController>
        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody] CreateBookDto dto, [FromServices] ICreateBookCommand command)
        {

            _commandHandler.HandleCommand(command, dto);
            return StatusCode(201);
        }



        // PUT api/<BookController>/5
        [HttpPut("{id}")]
        [Authorize]

        public IActionResult Put(int id, [FromBody] UpdateBookDto dto, [FromServices] IUpdateBookCommand command)
        {

            dto.Id = id;
            _commandHandler.HandleCommand(command, dto);
            return NoContent();
        }

        // DELETE api/<BookController>/5
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(int id, [FromBody] DeleteEntityDto dto, [FromServices] IDeleteBookCommand command)
        {

            dto.Id = id;
            _commandHandler.HandleCommand(command, dto);
            return NoContent();

        }




    }
}
