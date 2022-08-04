using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QVI_Vacay_API.Models;

namespace QVI_Vacay_API.Data
{
    public class QVI_Vacay_APIContext : DbContext
    {
        public QVI_Vacay_APIContext (DbContextOptions<QVI_Vacay_APIContext> options)
            : base(options)
        {
        }

        public DbSet<QVI_Vacay_API.Models.Account_Request> Account_Request { get; set; }
    }
}
