using Microsoft.EntityFrameworkCore;

namespace UCondo.PlanoDeContas.Domain.AgregacaoDePlanoDeConta
{
    public class AgregacaoDeContaDbContext : DbContext
    {
        public DbSet<PlanoDeConta> PlanosDeConta { get; set; }
        public DbSet<Receita> Receitas { get; set; }
        public DbSet<Despesa> Despesas { get; set; }


        public AgregacaoDeContaDbContext(DbContextOptions<AgregacaoDeContaDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Receita>().HasData(new Receita { AceitaLancamento = false, Codigo = "1", Nome = "Receitas" });
            modelBuilder.Entity<Despesa>().HasData(new Despesa { AceitaLancamento = false, Codigo = "2", Nome = "Despesas" });

            base.OnModelCreating(modelBuilder);
        }
    }
}
