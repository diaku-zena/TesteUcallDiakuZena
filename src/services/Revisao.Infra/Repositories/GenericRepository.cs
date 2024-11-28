using Microsoft.EntityFrameworkCore;
using Revisao.Domain.Interfaces;
using Revisao.Domain.Shared.Entities;
using Revisao.Infra.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revisao.Infra.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly DbSet<T> _dbSet;
        private readonly RevisaoContext _context;

        public GenericRepository(RevisaoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        public async Task<T> AddAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _dbSet.Remove(entity);
            var changes = await _context.SaveChangesAsync();
            return changes > 0; // Retorna true se alterações foram feitas no banco.
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            var entity = await _dbSet.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Entity with ID {id} was not found.");
            }
            return entity;
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _dbSet.Update(entity);
            var changes = await _context.SaveChangesAsync();
            return changes > 0; // Retorna true se alterações foram feitas no banco.
        }

    }
}
