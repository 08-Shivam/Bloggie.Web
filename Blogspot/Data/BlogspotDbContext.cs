using Blogspot.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blogspot.Data
{
    public class BlogspotDbContext:DbContext
    {
        public BlogspotDbContext(DbContextOptions<BlogspotDbContext> options) : base(options) 
        {

        } 

        public DbSet<BlogPost>BlogSpots {  get; set; }

        public DbSet<Tag>Tags { get; set; }

        public DbSet<BlogPostLike> BlogPostLike { get; set; }

        public DbSet<BlogPostComment> BlogPostComment { get; set;}
    }
}
