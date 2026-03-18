using Microsoft.EntityFrameworkCore;
using Pharmacy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.DAL
{
    public class PharmacyContext:DbContext
    {
        public PharmacyContext(DbContextOptions<PharmacyContext> options):base(options)
        {
            
        }
        public DbSet<Pharmacy.Models.Pharmacy> Pharmacies { get; set; }
        public DbSet<Medicine> Medicines { get; set; }

    }
}