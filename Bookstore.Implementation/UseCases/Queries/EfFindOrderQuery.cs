using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookstore.Application;
using Bookstore.Application.Exceptions;
using Bookstore.Application.UseCases.DTO;
using Bookstore.Application.UseCases.Queries;
using Bookstore.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Implementation.UseCases.Queries
{
    public class EfFindOrderQuery : EfUseCase, IFindOrderQuery
    {
        private readonly IApplicationActor _actor;
        public EfFindOrderQuery(BookstoreContext context, IApplicationActor actor) : base(context)
        {
            _actor = actor;
        }
        public int Id => 32;

        public string Name => "Find order";

        public string Description => "";

        public ReadOrdeDto Execute(int search)
        {
            var order = Context.Orders.Include(x => x.OrderItems).FirstOrDefault(x => x.Id == search && x.UserId == _actor.Id);


            if (order == null || !order.IsActive || order.DeletedAt.HasValue)
            {
                throw new EntityNotFoundException(search, nameof(order));
            }


            return new ReadOrdeDto
            {
                Id = order.Id,
                UserId = _actor.Id,
                Username = _actor.Username,
                OrderItems = order.OrderItems.Select(x => new ReadOrderItemDto
                {
                    Id = x.Id,
                    BookPublisherId = x.BookPublisherId,
                    Quantity = x.Quantity
                }).ToList()
            };

        }
    }
}
