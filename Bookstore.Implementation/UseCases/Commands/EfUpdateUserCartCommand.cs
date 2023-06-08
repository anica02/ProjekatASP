using Bookstore.Application;
using Bookstore.Application.Exceptions;
using Bookstore.Application.UseCases.Commands;
using Bookstore.Application.UseCases.DTO;
using Bookstore.DataAccess;
using Bookstore.Domain.Entities;
using Bookstore.Implementation.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Implementation.UseCases.Commands
{
    public class EfUpdateUserCartCommand : EfUseCase, IUpdateUserCartCommand
    {
        private readonly IApplicationActor _actor;
        private readonly CreateUserCartValidator _validator;

        public EfUpdateUserCartCommand(
            BookstoreContext context,
            IApplicationActor actor,
            CreateUserCartValidator validator
            ):base(context)
        {
            _actor = actor;
            _validator = validator;
        }
        public int Id => 15;

        public string Name => "Cart edit";

        public string Description => "";

        public void Execute(CreateUserCartDto request)
        {
            var cart = Context.Carts.Include(x=>x.CartItems).FirstOrDefault(x => x.Id == request.Id.Value && x.IsActive && x.UserId==_actor.Id);

            if (cart == null || cart.DeletedAt.HasValue)
            {
                throw new EntityNotFoundException(request.Id.Value, "cart");

            }

            _validator.ValidateAndThrow(request);
           

            if (!request.CartItems.Any())
            {
                var remove = Context.CartItems.Where(x => x.CartId == cart.Id);
                Context.CartItems.RemoveRange(remove);

                Context.Carts.Remove(cart);
            }
            else
            {
               

                var removeItems = cart.CartItems.Where(x => !request.CartItems.Any(b => b.BookPublisherId == x.BookPublisherId));
                Context.CartItems.RemoveRange(removeItems);

                var cartItems = Context.CartItems.Where(x => x.CartId == cart.Id);

                foreach (var item in request.CartItems)
                {
                    foreach (var itemInDb in cartItems)
                    {
                        if (item.BookPublisherId == itemInDb.BookPublisherId)
                        {
                            if (item.Quantity == itemInDb.Quantity)
                            {
                                continue;
                            }
                            else
                            {
                                itemInDb.Quantity = item.Quantity;
                                itemInDb.ModifiedAt = DateTime.UtcNow;
                                itemInDb.ModifiedBy = _actor.Username;
                                Context.Entry(itemInDb).State = EntityState.Modified;
                            }
                        }
                        else
                        {
                            CartItem newItem = new CartItem();
                            newItem.BookPublisherId = item.BookPublisherId;
                            newItem.Quantity = item.Quantity;
                            newItem.CartId = cart.Id;
                            Context.CartItems.Add(newItem);
                           
                        }
                    }
                        
                }

            }
            cart.ModifiedAt = DateTime.UtcNow;
            cart.ModifiedBy = _actor.Username;
            Context.Entry(cart).State = EntityState.Modified;

            Context.SaveChanges();
            
        }
    }
}
