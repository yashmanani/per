using Newtonsoft.Json;
using System;
using System.Collections.Generic;

#nullable disable

namespace WebApplication1.Models
{
    public partial class Product
    {
        public Product()
        {
            ProductDetails = new HashSet<ProductDetail>();
        }

        public int ProductId { get; set; }
        public string Name { get; set; }
        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }
        public decimal? Price { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        [JsonIgnore]
        public virtual Brand Brand { get; set; }
        [JsonIgnore]
        public virtual Category Category { get; set; }
        [JsonIgnore]
        public virtual ICollection<ProductDetail> ProductDetails { get; set; }
    }
}
