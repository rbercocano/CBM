using AutoMapper;
using Charcutarie.Models;
using Charcutarie.Models.ViewModels;
using Charcutarie.Repository.Contracts;
using Charcutarie.Repository.DbContext;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Charcutarie.Repository
{
    public class CorpClientRepository : ICorpClientRepository
    {
        private readonly CharcuterieDbContext context;
        private readonly IMapper mapper;

        public CorpClientRepository(CharcuterieDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<CorpClient> Add(NewCorpClient model)
        {
            var @params = new[]
                {
                   new SqlParameter("@p0", SqlDbType.VarChar) {Direction = ParameterDirection.Input, Size = 100, Value= string.IsNullOrEmpty(model.Name) ? DBNull.Value :(object) model.Name},
                   new SqlParameter("@p1", SqlDbType.VarChar) {Direction = ParameterDirection.Input, Size = 100, Value = string.IsNullOrEmpty(model.DBAName) ? DBNull.Value :(object) model.DBAName},
                   new SqlParameter("@p2", SqlDbType.Bit) {Direction = ParameterDirection.Input, Value =model.Active},
                   new SqlParameter("@p3", SqlDbType.VarChar) {Direction = ParameterDirection.Input, Size =200, Value = string.IsNullOrEmpty(model.Email) ? DBNull.Value : (object)model.Email},
                   new SqlParameter("@p4", SqlDbType.VarChar) {Direction = ParameterDirection.Input, Size = 20, Value = string.IsNullOrEmpty(model.Mobile) ? DBNull.Value : (object)model.Mobile},
                   new SqlParameter("@p5", SqlDbType.VarChar) {Direction = ParameterDirection.Input, Size = 4, Value = string.IsNullOrEmpty(model.Currency) ? DBNull.Value : (object)model.Currency},
                   new SqlParameter("@p6", SqlDbType.Int) {Direction = ParameterDirection.Input, Value = model.CustomerTypeId},
                   new SqlParameter("@p7", SqlDbType.VarChar) {Direction = ParameterDirection.Input, Size = 14, Value = string.IsNullOrEmpty(model.CPF) ? DBNull.Value : (object)model.CPF},
                   new SqlParameter("@p8", SqlDbType.VarChar) {Direction = ParameterDirection.Input, Size = 18, Value = string.IsNullOrEmpty(model.CNPJ) ? DBNull.Value :(object) model.CNPJ},
                   new SqlParameter("@p9", SqlDbType.DateTimeOffset) {Direction = ParameterDirection.Input, Value = model.LicenseExpirationDate.HasValue ? (object)model.LicenseExpirationDate.Value: DBNull.Value},
                   new SqlParameter("@ID", SqlDbType.Int) {Direction = ParameterDirection.Output}
                 };
            var id = await context.Database.ExecuteSqlRawAsync("exec @ID = AddCorpClient @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9", @params);
            return await Get(Convert.ToInt32(@params[10].Value));
        }
        public async Task<CorpClient> GetByCpf(string cpf)
        {
            var entity = await context.CorpClients.FirstOrDefaultAsync(c => c.CPF == cpf);
            var result = mapper.Map<CorpClient>(entity);
            return result;

        }
        public async Task<CorpClient> GetByCnpj(string cnpj)
        {
            var entity = await context.CorpClients.FirstOrDefaultAsync(c => c.CNPJ == cnpj);
            var result = mapper.Map<CorpClient>(entity);
            return result;

        }
        public async Task<CorpClient> Update(UpdateCorpClient model)
        {
            var entity = context.CorpClients.Find(model.CorpClientId);
            entity.DBAName = model.DbaName;
            entity.Currency = model.Currency;
            entity.Name = model.Name;
            entity.CustomerTypeId = model.CustomerTypeId;
            entity.Mobile = model.Mobile;
            entity.Email = model.Email;
            if (model.CustomerTypeId == 1)
            {
                entity.CPF = model.SocialIdentifier;
                entity.CNPJ = null;
            }
            else
            {
                entity.CNPJ = model.SocialIdentifier;
                entity.CPF = null;
            }
            context.Update(entity);
            var rows = await context.SaveChangesAsync();
            return mapper.Map<CorpClient>(entity);
        }
        public async Task<PagedResult<CorpClient>> GetPaged(int page, int pageSize, string filter, bool? active = null)
        {
            var query = context.CorpClients.AsQueryable();
            if (!string.IsNullOrEmpty(filter))
                query = query.Where(c => c.Name.Contains(filter) || c.DBAName.Contains(filter));

            if (active.HasValue)
                query = query.Where(c => c.Active == active);

            var count = query.Count();

            var data = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedResult<CorpClient>
            {
                CurrentPage = page,
                RecordCount = count,
                Data = mapper.Map<IEnumerable<CorpClient>>(data),
                RecordsPerpage = pageSize
            };
        }
        public async Task<CorpClient> Get(int id)
        {
            var entity = await context.CorpClients.FirstOrDefaultAsync(c => c.CorpClientId == id);
            var result = mapper.Map<CorpClient>(entity);
            return result;
        }
        public async Task<IEnumerable<CorpClient>> GetActives()
        {
            var data = await context.CorpClients.Where(c => c.Active).ToListAsync();
            return mapper.Map<IEnumerable<CorpClient>>(data);
        }
    }
}
