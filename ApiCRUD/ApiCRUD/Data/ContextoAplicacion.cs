using ApiCRUD.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCRUD.Data
{
    public class ContextoAplicacion : DbContext
    {
        public ContextoAplicacion(DbContextOptions<ContextoAplicacion> options)
            : base(options)
        { }

        public DbSet<PersonaFisica> Tb_PersonasFisicas { get; set; }


    }
}
