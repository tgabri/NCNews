using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

    public class TopicCreateDTO
    {
        [Required]
        [StringLength(100)]

        public string Title { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
    }

    public class TopicUpdateDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
    }
}
