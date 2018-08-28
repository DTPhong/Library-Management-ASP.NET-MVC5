namespace Lib.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class vBookDetail
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CallNumber { get; set; }

        [StringLength(50)]
        public string ISBN { get; set; }

        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(50)]
        public string Author { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int StudentId { get; set; }

        [StringLength(50)]
        public string StudentName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DueDate { get; set; }

        [StringLength(50)]
        public string IssueStatus { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CheckoutDate { get; set; }
    }
}
