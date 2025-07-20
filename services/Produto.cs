using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyStoreApi.services
{
    public class Produto
    {
        public int? Id { get; set; }
        public string NameProduct { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string Picture { get; set; }
        public string? LinkVenda { get; set; }
        public string? PictureFirst { get; set; }
        public string? PictureSecond { get; set; }
        public string? PictureThird { get; set; }
    }

    public class ProdutoRequest
    {
        public string? Product { get; set; }
    }


}