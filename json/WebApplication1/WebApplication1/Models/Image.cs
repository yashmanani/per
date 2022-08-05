using System;
using System.Collections.Generic;

#nullable disable

namespace WebApplication1.Models
{
    public partial class Image
    {
        public Image()
        {
            ProductDetails = new HashSet<ProductDetail>();
        }

        public int ImageId { get; set; }
        public string ImageName { get; set; }
        public string ImageUrl { get; set; }

        public virtual ICollection<ProductDetail> ProductDetails { get; set; }
    }
}
