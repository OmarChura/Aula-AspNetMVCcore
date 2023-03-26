using Microsoft.EntityFrameworkCore;
using DevIO.UI.Site;
using DevIO.UI.Site.Models;

namespace DevIO.UI.Site.Data
{
    public class MeuDbContext : DbContext
    {
        public MeuDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Aluno> Alunos { get; set; }
    }
}
