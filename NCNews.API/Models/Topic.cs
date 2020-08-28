using System.Collections;
using System.Collections.Generic;

namespace NCNews.API.Models
{
    public class Topic
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public virtual IList<Article> Articles { get; set; }

    }
}