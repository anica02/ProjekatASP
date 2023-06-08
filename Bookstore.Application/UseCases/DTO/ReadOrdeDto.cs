using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Application.UseCases.DTO
{
    public class ReadOrdeDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Address { get; set; }
        public string PaymentMethod { get; set; }
        public string DeliveryMethod { get; set; }

        public IEnumerable<ReadOrderItemDto> OrderItems { get; set; } = new List<ReadOrderItemDto>();
    }

    public class ReadOrderItemDto
    {
        public int Id { get; set; }
        public int BookPublisherId { get; set; }
        public int Quantity { get; set; }

    }
}
