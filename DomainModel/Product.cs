using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainModel
{
    public class Product
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Rating { get; set; }
        public double Price { get; set; }
        public DateTime ReleaseDate { get; set; }
    }

}
