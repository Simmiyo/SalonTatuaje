using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SalonTatuaje.Models.Validari
{
    public class DimensiuneMin : ValidationAttribute
    {
        private readonly int _dimMin;

        public DimensiuneMin(int dm)
        {
            _dimMin = dm;
        }

        public override bool IsValid(object valoare)
        {
            return (int)valoare >= _dimMin;
        }
    }
}