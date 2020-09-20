using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NCNews.API.Models
{
    public class Article
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Votes { get; set; }

        public int TopicId { get; set; }

        public Author Author { get; set; }
    }
}
