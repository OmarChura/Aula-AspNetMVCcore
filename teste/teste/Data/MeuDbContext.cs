using teste.Models;
using Microsoft.EntityFrameworkCore;

namespace teste.Data
{
    public class MeuDbContext : DbContext
    {
        public MeuDbContext(DbContextOptions<MeuDbContext> options) : base(options)
        {

        }

        public DbSet<Aluno> Alunos { get; set; }

    }
}
