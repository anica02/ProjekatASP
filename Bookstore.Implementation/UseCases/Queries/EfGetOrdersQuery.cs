using Bookstore.Application.Exceptions;
using Bookstore.Application.UseCases.DTO;
using Bookstore.Application.UseCases.Queries;
using Bookstore.Application.UseCases.Queries.Searches;
using Bookstore.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Implementation.UseCases.Queries
{
    public class EfGetOrdersQuery : EfUseCase, IGetOrdersQuery
    {
        public EfGetOrdersQuery(BookstoreContext context) : base(context)
        {

        }

        public int Id => 41;

        public string Name => "Search orders";

        public string Description => "";

        public IEnumerable<ReadOrdeDto> Execute(CartSearch search)
        {

          
            var query = Context.Orders.AsQueryable();

            if (query == null)
            {
                throw new NotFoundException("orders");

            }

            if (search.UserId.HasValue)
            {
                query = query.Where(x => x.UserId == search.UserId);
            }

            if (search.UserId.HasValue)
            {
                query = query.Where(x => x.OrderItems.Any(i => i.BookPublisherId == search.BookPublisherId));
            }




            IEnumerable<ReadOrdeDto> result = query.Select(p => new ReadOrdeDto
            {
                Id = p.Id,
                UserId = p.User.Id,
                Username = p.User.Username,
                OrderItems = p.OrderItems.Select(x => new ReadOrderItemDto
                {
                    Id = x.Id,
                    BookPublisherId = x.BookPublisherId,
                    Quantity = x.Quantity
                }).ToList()

            }).ToList();

            return result;
        }
    }
}
