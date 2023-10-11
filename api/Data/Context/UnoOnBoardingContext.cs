using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Context
{
    public class UnoOnBoardingContext: DbContext
    {
        public UnoOnBoardingContext(DbContextOptions<UnoOnBoardingContext> options) : base(options)
        {
        }
        public DbSet<User>? Users { get; set; }
    }       
}
