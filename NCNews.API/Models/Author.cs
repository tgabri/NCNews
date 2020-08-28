using Microsoft.AspNetCore.Identity;

namespace NCNews.API.Models
{
    public class Author : IdentityUser
    {
        public string Alias { get; set; }
    }
}