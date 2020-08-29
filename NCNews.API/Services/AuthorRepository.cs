using Microsoft.EntityFrameworkCore;
using NCNews.API.Contracts;
using NCNews.API.Data;
using NCNews.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCNews.API.Services
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly ApplicationDbContext _db;

        public AuthorRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(Author model)
        {
            await _db.Authors.AddAsync(model);
            return await Save();
        }

        public async Task<bool> Delete(Author model)
        {
            _db.Authors.Remove(model);
            return await Save();
        }

        public async Task<IList<Author>> FindAll()
        {
            var authors = await _db.Authors.ToListAsync();
            return authors;
        }

        public async Task<Author> FindById(int id)
        {
            var author = await _db.Authors.FindAsync(id);
            return author;
        }

        public async Task<bool> Update(Author model)
        {
            _db.Authors.Update(model);
            return await Save();
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }
    }
}
