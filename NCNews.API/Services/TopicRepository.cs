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
    public class TopicRepository : ITopicRepository
    {
        private readonly ApplicationDbContext _db;
        public TopicRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(Topic model)
        {
            await _db.Topics.AddAsync(model);
            return await Save();
        }

        public async Task<bool> Delete(Topic model)
        {
            _db.Topics.Remove(model);
            return await Save();
        }

        public async Task<IList<Topic>> FindAll()
        {
            var topics = await _db.Topics.Include(t => t.Articles).ToListAsync();
            return topics;
        }

        public async Task<Topic> FindById(int id)
        {
            var topic = await _db.Topics.Include(t => t.Articles).FirstOrDefaultAsync(t => t.Id == id);
            return topic;
        }

        public Task<bool> IsExists(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(Topic model)
        {
            _db.Topics.Update(model);
            return await Save();
        }
    }
}
