namespace De02.DuLieu
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LoaiSP")]
    public partial class LoaiSP
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]

        public LoaiSP()
        {
            Sanphams = new HashSet<Sanpham>();
        }

        [Key]
        [StringLength(2)]
        public int MaLoai { get; set; }

        [Required]
        [StringLength(30)]
        public int TenLoai { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sanpham> Sanphams { get; set; }
    }
}
