using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCNews.API.DTOs
{
    public class TopicDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public virtual IList<ArticleDTO> Articles { get; set; }
    }
}
