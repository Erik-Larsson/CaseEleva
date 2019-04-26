namespace Case
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Turma")]
    public partial class Turma
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id_turma { get; set; }

        public int id_escola { get; set; }

        [StringLength(200)]
        public string nome_turma { get; set; }

        public virtual Escola Escola { get; set; }
    }
}
