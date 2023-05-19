using Domain.Models.Auth;
using Infrastructure.Options;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Infrastructure.Database
{
	public class ApplicationDbContext : IdentityDbContext
	{
		private readonly ConnectionStringsOptions _connectionStringOptions;
		public new DbSet<UserModel> Users { get; set; }

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IOptions<ConnectionStringsOptions> connectionStringOptions) : base(options)
		{
			_connectionStringOptions = connectionStringOptions.Value;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(_connectionStringOptions.DefaultConnection);
			
			base.OnConfiguring(optionsBuilder);
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			
			var assemblyWithConfigurations = GetType().Assembly;
			builder.ApplyConfigurationsFromAssembly(assemblyWithConfigurations);
		}
	}
}