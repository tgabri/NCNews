﻿using System;
using System.Collections.Generic;
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
        public virtual TopicDTO Topic { get; set; }
        public int AuthorId { get; set; }
        public AuthorDTO Author { get; set; }
    }
}