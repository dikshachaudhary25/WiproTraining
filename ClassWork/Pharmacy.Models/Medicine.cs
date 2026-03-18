using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Models
{
    public class Medicine
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [ForeignKey("pharma")]
        public int PharmacyId { get; set; }
        public Pharmacy? pharma { get; set; }
        public int Qty { get; set; }
    }
}