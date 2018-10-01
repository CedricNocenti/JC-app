using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JC_PROJECT.Models
{
    public class Product
    {
        public int Identifiant { get; set; }
        public int ShopId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Origin { get; set; }
        public int Weight { get; set; }
        public string WeightUnit { get; set; }
        public int Lenght { get; set; }
        public string LenghtUnit { get; set; }
        public int Depth { get; set; }
        public string DepthUnit { get; set; }
        public int Width { get; set; }
        public string WidthUnit { get; set; }
        public int Height { get; set; }
        public string HeightUnit { get; set; }
        public int Price { get; set; }
        public string Currency { get; set; }
        public int Inventory { get; set; }
        public DateTime Creation { get; set; }
        public DateTime Modification { get; set; }
    }
}