﻿using Bookstore.Application;
using Bookstore.Application.Exceptions;
using Bookstore.Application.UseCases.Commands;
using Bookstore.Application.UseCases.DTO;
using Bookstore.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Implementation.UseCases.Commands
{
    public class EfDeleteBookCommand: EfUseCase, IDeleteBookCommand
    {
        private readonly IApplicationActor _actor;

        public EfDeleteBookCommand(
             BookstoreContext context,
            IApplicationActor actor
           ) : base(context)
        {

            _actor = actor;

        }

        public int Id => 19;

        public string Name => "Book delete";

        public string Description => "";

        public void Execute(DeleteEntityDto request)
        {
            var book = Context.Books.Include(x => x.BookPublishers).FirstOrDefault(x => x.Id == request.Id);

            if (book == null || !book.IsActive || book.DeletedAt.HasValue)
            {
                throw new EntityNotFoundException(request.Id, "book");
            }

            if (book.BookPublishers.Any())
            {
                throw new ConflictException(request.Id, "book", "Book cannot be deleted because it has been published");
            }

            if (Context.OrderItems.Any(x=>x.BookPublisher.BookId==book.Id && x.IsActive))
            {
                throw new ConflictException(request.Id, "book", "Book cannot be deleted because it is part of an user order");
            }

           

            book.DeletedAt = DateTime.UtcNow;
            book.IsActive = false;
            book.DeletedBy = _actor.Username;
            Context.SaveChanges();
        }
    }
}
