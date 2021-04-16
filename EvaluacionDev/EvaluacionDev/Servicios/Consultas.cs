using EvaluacionDev.Data;
using EvaluacionDev.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvaluacionDev.Servicios
{
    public class Consultas
    {
        ApplicationDbContext _contexto;
        public Consultas(ApplicationDbContext contexto)
        {
            _contexto = contexto;
        }
         
    }
}
