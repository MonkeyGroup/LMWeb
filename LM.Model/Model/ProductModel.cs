using System;
using LM.Model.Common;

namespace LM.Model.Model
{
    public class ProductModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ProductType Type { get; set; }

        public string ImgSrc { get; set; }

        public string Company1 { get; set; }

        public string Description1 { get; set; }

        public string Company2 { get; set; }

        public string Description2 { get; set; }

        public string Company3 { get; set; }

        public string Description3 { get; set; }

        public DateTime SaveAt { get; set; }

    }
}
