
using AutoMapper;
using Core.Application.Interfaces;
using Core.Domain.Entities;
using Core.Domain.Entities.Application;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Infrastructure.Persistence.Repositories;

public class ContactRepository : EfRepository<Contact>, IContactRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public ContactRepository(ApplicationDbContext dbContext, IMapper mapper)
        : base(dbContext)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Contact> GetFullContact(int id)
    {
        //return await _dbContext.Contacts.Where(c => c.Id == id)
        //    .Include(c => c.Addresses)
        //    .AsNoTracking()
        //    .FirstAsync();

        var query = from c in _dbContext.Contacts
                    where c.Id == id
                    join a in _dbContext.Addresses
                    on c.Id equals a.ContactId
                    select new { c, a };

        var result = await query.ToListAsync();

        return result.First().c;
    }

    public async override Task UpdateAsync(Contact entity, CancellationToken cancellationToken = default)
    {
        var existingEntity = await _dbContext.Contacts.FindAsync(entity.Id);
        entity.CreatedBy=existingEntity.CreatedBy;
        entity.DateCreated=existingEntity.DateCreated;
        _dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
        await base.UpdateAsync(existingEntity, cancellationToken);
    }

    public async Task DeleteAsync(int id)
    {
        var existingEntity = await _dbContext.Contacts.FindAsync(id);
        await base.DeleteAsync(existingEntity);
    }
}
