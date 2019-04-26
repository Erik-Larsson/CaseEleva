namespace Case
{
	using System;
	using System.Data.Entity;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;

	public partial class Models : DbContext
	{
		public Models()
			: base("name=Models")
		{
		}

		public virtual DbSet<Escola> Escola { get; set; }
		public virtual DbSet<Turma> Turma { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Escola>()
				.Property(e => e.nome_escola)
				.IsUnicode(false);

			modelBuilder.Entity<Escola>()
				.HasMany(e => e.Turma)
				.WithRequired(e => e.Escola)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Turma>()
				.Property(e => e.nome_turma)
				.IsUnicode(false);
		}
	}
}
