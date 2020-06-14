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
using Microsoft.Data.SqlClient;
using System.Text;

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
        public async Task<IEnumerable<ProductionCostProfit>> GetProductionCostProfit(int corpClientId)
        {
            var sqlParams = new List<SqlParameter>();
            sqlParams.Add(new SqlParameter("@corpClientId", corpClientId));
            var query = new StringBuilder(@"SELECT 
	                                            P2.ProductId,
	                                            P2.Name AS Product,
	                                            Cost,
	                                            Price - Cost AS Profit,
	                                            ROUND((Price - Cost)/ Cost * 100,2) as ProfitPercentage
                                            FROM 
                                            (SELECT 
		                                            X.ProductId,
		                                            ROUND(SUM(X.Cost),2) AS Cost
                                            FROM 
                                            (SELECT
	                                            P.ProductId,
	                                            CASE 
		                                            WHEN (R.MeasureUnitId = 5 OR  R.MeasureUnitId = 6)
			                                            THEN R.Price / dbo.ConvertMeasure(R.MeasureUnitId,1,5) 
		                                            ELSE R.Price / dbo.ConvertMeasure(R.MeasureUnitId,1,1)
	                                            END * (dbo.ConvertMeasure(P.MeasureUnitId,1,1) / (1-(CAST(D.WeightLossPercentage as DECIMAL(4,2))/100))) * I.Percentage/100 AS Cost
	                                            FROM Product P
	                                            JOIN DataSheet D ON P.ProductId = D.ProductId
	                                            JOIN DataSheetItem I ON D.DataSheetId = I.DataSheetId
	                                            JOIN RawMaterial R ON I.RawMaterialId = R.RawMaterialId
	                                            WHERE P.CorpClientId = @corpClientId
	                                            ) X GROUP BY X.ProductId)
	                                             X2 
                                            JOIN Product P2 ON X2.ProductId = P2.ProductId");
            var data = await context.ProductionCostProfits.FromSqlRaw(query.ToString(), sqlParams.ToArray()).FirstOrDefaultAsync();
            return mapper.Map<IEnumerable<ProductionCostProfit>>(data);
        }
    }
}
