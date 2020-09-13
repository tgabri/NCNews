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
    public class ArticleRepository : IArticleRepository
    {
        private readonly ApplicationDbContext _db;
        public ArticleRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(Article model)
        {
            await _db.Articles.AddAsync(model);
            return await Save();
        }

        public async Task<bool> Delete(Article model)
        {
            _db.Articles.Remove(model);
            return await Save();
        }

        public async Task<IList<Article>> FindAll()
        {
            var articles = await _db.Articles.ToListAsync();
            return articles;
        }

        public async Task<Article> FindById(int id)
        {
            var article = await _db.Articles.FirstOrDefaultAsync(a => a.Id == id);
            return article;
        }

        public async Task<bool> IsExists(int id)
        {
            return await _db.Articles.AnyAsync(a => a.Id == id);
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(Article model)
        {
            _db.Articles.Update(model);
            return await Save();
        }
    }
}
