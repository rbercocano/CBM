using AutoMapper;
using Charcutarie.Models;
using Charcutarie.Models.ViewModels;
using EF = Charcutarie.Models.Entities;
using Charcutarie.Repository.Contracts;
using Charcutarie.Repository.DbContext;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Charcutarie.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly CharcuterieDbContext context;
        private readonly IMapper mapper;

        public ProductRepository(CharcuterieDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<long> Add(NewProduct model)
        {
            var entity = mapper.Map<EF.Product>(model);
            context.Add(entity);
            var rows = await context.SaveChangesAsync();
            return await Task.FromResult(entity.ProductId);
        }
        public async Task<Product> Update(UpdateProduct model)
        {
            var entity = context.Products.FirstOrDefault(p => p.CorpClientId == model.CorpClientId && p.ProductId == model.ProductId);
            entity.Name = model.Name;
            entity.ActiveForSale = model.ActiveForSale;
            entity.MeasureUnitId = model.MeasureUnitId;
            entity.Price = model.Price;
            context.Update(entity);
            var rows = await context.SaveChangesAsync();
            var result = mapper.Map<Product>(entity);
            return await Task.FromResult(result);
        }
        public async Task<PagedResult<Product>> GetPaged(int corpClientId, int page, int pageSize, string name, bool? active = null)
        {
            var query = context.Products.Include(p => p.MeasureUnit).Where(p => p.CorpClientId == corpClientId).AsQueryable();
            if (!string.IsNullOrEmpty(name))
                query = query.Where(c => c.Name.Contains(name));

            if (active.HasValue)
                query = query.Where(c => c.ActiveForSale == active);

            var count = query.Count();

            var data = await query.OrderBy(p => p.Name).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedResult<Product>
            {
                CurrentPage = page,
                RecordCount = count,
                Data = mapper.Map<IEnumerable<Product>>(data),
                RecordsPerpage = pageSize
            };
        }
        public async Task<Product> Get(int corpClientId, long id)
        {
            var entity = await context.Products.Include(p => p.MeasureUnit).FirstOrDefaultAsync(p => p.ProductId == id && p.CorpClientId == corpClientId);
            var result = mapper.Map<Product>(entity);
            return result;
        }
        public async Task<IEnumerable<Product>> GetAll(int corpClientId)
        {
            var entity = await context.Products.Include(p => p.MeasureUnit).Where(p => p.CorpClientId == corpClientId).OrderBy(p => p.Name).ToListAsync();
            var result = mapper.Map<IEnumerable<Product>>(entity);
            return result;
        }

        public async Task<IEnumerable<Product>> GetRange(int corpClientId, List<long> ids)
        {
            var entity = await context.Products.Include(p => p.MeasureUnit).Where(p => p.CorpClientId == corpClientId && ids.Contains(p.ProductId)).OrderBy(p => p.Name).ToListAsync();
            var result = mapper.Map<IEnumerable<Product>>(entity);
            return result;
        }
    }
}
