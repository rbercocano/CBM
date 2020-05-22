using AutoMapper;
using Charcutarie.Models.ViewModels;
using Charcutarie.Repository.Contracts;
using Charcutarie.Repository.DbContext;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Repository
{
    public class OrderStatusRepository : IOrderStatusRepository
    {
        private readonly CharcuterieDbContext context;
        private readonly IMapper mapper;

        public OrderStatusRepository(CharcuterieDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<OrderStatus>> GetAll()
        {
            var data = await context.OrderStatus.ToListAsync();
            var result = mapper.Map<IEnumerable<OrderStatus>>(data);
            return result;
        }
    }
}
