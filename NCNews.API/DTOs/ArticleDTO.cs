using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NCNews.API.DTOs
{
    public class ArticleDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Votes { get; set; }
        public int TopicId { get; set; }
        public int AuthorId { get; set; }
        public AuthorDTO Author { get; set; }
    }

    public class ArticleCreateDTO
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Votes { get; set; }
        public int TopicId { get; set; }
        public int AuthorId { get; set; }
    }

    public class ArticleUpdateDTO
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Votes { get; set; }
        public int TopicId { get; set; }

        // TODO - Find Out If AuthorID is needed for update ones there is a registered user
    }
}
