namespace Case
{
    using System;
    using System.Collections.Generic;
	using System.ComponentModel;
	using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Turma")]
    public partial class Turma
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_turma { get; set; }

		[DisplayName("Escola")]
		public int id_escola { get; set; }

        [StringLength(200)]
		[DisplayName("Turma")]
		public string nome_turma { get; set; }

        public virtual Escola Escola { get; set; }
    }
}
