using AutoMapper;
using Charcutarie.Models.ViewModels;
using Charcutarie.Repository.Contracts;
using Charcutarie.Repository.DbContext;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Repository
{
    public class OrderItemStatusRepository : IOrderItemStatusRepository
    {
        private readonly CharcuterieDbContext context;
        private readonly IMapper mapper;

        public OrderItemStatusRepository(CharcuterieDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<OrderItemStatus>> GetAll()
        {
            var data = await context.OrderItemStatus.ToListAsync();
            var result = mapper.Map<IEnumerable<OrderItemStatus>>(data);
            return result;
        }
    }
}
