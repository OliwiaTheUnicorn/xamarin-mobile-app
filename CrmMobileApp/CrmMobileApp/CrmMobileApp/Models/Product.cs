using System;
using System.Collections.Generic;
using System.Text;

namespace CrmMobileApp.Models
{
    public class Product
    {
        public string Name { get; set; }
        public string Model { get; set; }
        public string Image { get; set; }
        public double Rating { get; set; }
        public double Views { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
    }
}
