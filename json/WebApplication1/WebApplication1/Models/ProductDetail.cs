using System;
using System.Collections.Generic;

#nullable disable

namespace WebApplication1.Models
{
    public partial class ProductDetail
    {
        public int ProductDetailId { get; set; }
        public int? ProductId { get; set; }
        public int? ImageId { get; set; }

        public virtual Image Image { get; set; }
        public virtual Product Product { get; set; }
    }
}
