using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodMvc.Data.Entity
{
    public class Food : Base
    {
        public string Name { get; set; }
        public string FoodType { get; set; }
        public string Color { get; set; }
        public string Weight { get; set; }
    }
}
