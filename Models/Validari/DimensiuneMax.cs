using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SalonTatuaje.Models.Validari
{
    public class DimensiuneMax : ValidationAttribute
    {
        private readonly int _dimMax;

        public DimensiuneMax(int dm)
        {
            _dimMax = dm;
        }

        public override bool IsValid(object valoare)
        {
            return (int)valoare <= _dimMax;
        }
    }
}