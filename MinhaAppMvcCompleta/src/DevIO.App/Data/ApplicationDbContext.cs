﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DevIO.App.ViewModels;
using DevIO.Data.Mappings;

namespace DevIO.App.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        //public DbSet<DevIO.App.ViewModels.FornecedorViewModel> FornecedorViewModel { get; set; }
        //public DbSet<DevIO.App.ViewModels.ProdutoViewModel> ProdutoViewModel { get; set; }
        //public DbSet<DevIO.App.ViewModels.EnderecoViewModel> EnderecoViewModel { get; set; }
    }
}