using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodMvc.Data.Entity
{
    public class Car : Base
    {
        public string BrandName { get; set; }
        public string CarName { get; set; }
        public string CarNumber { get; set; }
        public string Owner { get; set; }
    }
}
