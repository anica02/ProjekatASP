﻿using Bookstore.Application;
using Bookstore.Application.Exceptions;
using Bookstore.Application.UseCases.DTO;
using Bookstore.Application.UseCases.Queries;
using Bookstore.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Implementation.UseCases.Queries
{
    public class EfFindCartQuery: EfUseCase, IFindUserCartQuery
    {
        private readonly IApplicationActor _actor;
        public EfFindCartQuery(BookstoreContext context, IApplicationActor actor) : base(context)
        {
            _actor = actor;
        }
        public int Id => 31;

        public string Name => "Find cart";

        public string Description => "";

        public ReadCartDto Execute(int search)
        {
            var cart = Context.Carts.Include(x=>x.CartItems).FirstOrDefault(x => x.Id == search && x.UserId==_actor.Id);


            if (cart == null || !cart.IsActive || cart.DeletedAt.HasValue)
            {
                throw new EntityNotFoundException(search, nameof(cart));
            }


            return new ReadCartDto
            {
               Id=cart.Id,
               UserId=_actor.Id,
               Username=_actor.Username,
               CartItems= cart.CartItems.Select(x => new ReadCartItemDto
               {
                  Id=x.Id,
                  BookPublisherId=x.BookPublisherId,
                  Quantity=x.Quantity
               }).ToList()
            };

        }
    }
}
