using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models;

namespace UserManagement.Data;

public class DataContext : DbContext, IDataContext
{
    public DataContext() => Database.EnsureCreated();

    public DbSet<User>? Users { get; set; }

    public async Task CreateAsync<TEntity>(TEntity entity) where TEntity : class
    {
        await base.AddAsync(entity);
        await SaveChangesAsync();
    }

    public async Task DeleteAsync<TEntity>(object id) where TEntity : class
    {
        var entity = await GetByIDAsync<TEntity>(id);
        base.Remove(entity);
        await SaveChangesAsync();
    }

    public Task<List<TEntity>> GetAllAsync<TEntity>() where TEntity : class => Set<TEntity>().ToListAsync();

    public Task<List<TEntity>> GetAsync<TEntity>(Expression<Func<TEntity, bool>>? filter = null, string sortField = "", bool isDesc = false) where TEntity : class
    {
        IQueryable<TEntity> query = base.Set<TEntity>();
        if (filter != null)
        {
            query = query.Where(filter);
        }
        if (isDesc)
        {
            query = query.OrderByDescending(e => EF.Property<object>(e, sortField));
        }
        else
        {
            query = query.OrderBy(e => EF.Property<object>(e, sortField));
        }
        return query.ToListAsync();
    }

    public async Task<TEntity> GetByIDAsync<TEntity>(object id) where TEntity : class
    {
        var entity = await base.FindAsync<TEntity>(id) ?? throw new ArgumentException($"{typeof(TEntity).Name} with id {id} not found.");

        return entity;
    }

    public async Task UpdateAsync<TEntity>(TEntity entity) where TEntity : class
    {
        base.Update(entity);
        await SaveChangesAsync();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
                                    => options.UseInMemoryDatabase("UserManagement.Data.DataContext");

    protected override void OnModelCreating(ModelBuilder model)
        => model.Entity<User>().HasData(new[]
        {
            new User { Id = 1, Forename = "Peter", Surname = "Loew", Email = "ploew@example.com", IsActive = true, DateOfBirth = DateTime.Parse("2025-1-11") },
            new User { Id = 2, Forename = "Benjamin Franklin", Surname = "Gates", Email = "bfgates@example.com", IsActive = true, DateOfBirth = DateTime.Parse("2025-5-15") },
            new User { Id = 3, Forename = "Castor", Surname = "Troy", Email = "ctroy@example.com", IsActive = false, DateOfBirth = DateTime.Parse("2025-6-11") },
            new User { Id = 4, Forename = "Memphis", Surname = "Raines", Email = "mraines@example.com", IsActive = true, DateOfBirth = DateTime.Parse("2025-10-1") },
            new User { Id = 5, Forename = "Stanley", Surname = "Goodspeed", Email = "sgodspeed@example.com", IsActive = true, DateOfBirth = DateTime.Parse("2025-6-9") },
            new User { Id = 6, Forename = "H.I.", Surname = "McDunnough", Email = "himcdunnough@example.com", IsActive = true, DateOfBirth = DateTime.Parse("2025-7-21") },
            new User { Id = 7, Forename = "Cameron", Surname = "Poe", Email = "cpoe@example.com", IsActive = false, DateOfBirth = DateTime.Parse("2025-1-20") },
            new User { Id = 8, Forename = "Edward", Surname = "Malus", Email = "emalus@example.com", IsActive = false, DateOfBirth = DateTime.Parse("2025-3-21") },
            new User { Id = 9, Forename = "Damon", Surname = "Macready", Email = "dmacready@example.com", IsActive = false, DateOfBirth = DateTime.Parse("2025-9-15") },
            new User { Id = 10, Forename = "Johnny", Surname = "Blaze", Email = "jblaze@example.com", IsActive = true, DateOfBirth = DateTime.Parse("2025-4-19") },
            new User { Id = 11, Forename = "Robin", Surname = "Feld", Email = "rfeld@example.com", IsActive = true, DateOfBirth = DateTime.Parse("2025-8-30") },
        });
}
