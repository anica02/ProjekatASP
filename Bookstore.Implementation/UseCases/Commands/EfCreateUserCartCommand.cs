using Bookstore.Application;
using Bookstore.Application.Exceptions;
using Bookstore.Application.UseCases.Commands;
using Bookstore.Application.UseCases.DTO;
using Bookstore.DataAccess;
using Bookstore.Domain.Entities;
using Bookstore.Implementation.Validators;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Implementation.UseCases.Commands
{
    public class EfCreateUserCartCommand: EfUseCase , ICreateUserCartCommand
    {
        private readonly IApplicationActor _actor;
        private readonly CreateUserCartValidator _validator;
        public EfCreateUserCartCommand(BookstoreContext context, IApplicationActor actor,
            CreateUserCartValidator validator):base(context)
        {
            _actor = actor;
            _validator = validator;
        }
        public int Id => 14;

        public string Name => "Cart create";

        public string Description => "";

        public void Execute(CreateUserCartDto request)
        {
            var userHasCart = Context.Carts.Any(x => x.UserId == _actor.Id && x.IsActive);
            if (userHasCart)
            {
                throw new ConflictExceptionCreating("cart", $"It is not possible to create cart because there is already a cart for the given user, you can edit the existing one");
            }

            _validator.ValidateAndThrow(request);

          
        

            if (!request.CartItems.Any())
            {
                throw new ConflictExceptionCreating("cart", $"No items were added to the cart");
            }
            else
            {
                Cart cart = new Cart();
                cart.UserId = _actor.Id;
                Context.Carts.Add(cart);
                Context.SaveChanges();

                List<CartItemDto> itemsDb = new List<CartItemDto>();

                foreach (var item in request.CartItems)
                {
                    if (!itemsDb.Any())
                    {
                        itemsDb.Add(item);
                    }
                    else
                    {
                        foreach (var itemDb in itemsDb)
                        {
                            if (itemDb.BookPublisherId == item.BookPublisherId)
                            {
                                itemDb.Quantity += item.Quantity;
                            }

                        }
                    }


                }
                List<CartItem> cartItems = new List<CartItem>();

                foreach (var items in itemsDb)
                {
                   
                    CartItem item = new CartItem();
                    item.BookPublisherId = items.BookPublisherId;

                    item.Quantity = items.Quantity;
                    item.CartId = cart.Id;
                    cartItems.Add(item);

                }
                Context.CartItems.AddRange(cartItems);

              

                Context.SaveChanges();
            }
         
          
           


        }
    }
}
