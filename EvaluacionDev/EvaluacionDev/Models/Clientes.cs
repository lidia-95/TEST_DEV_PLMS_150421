using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvaluacionDev.Models
{
    public class Clientes
    {
        public int IdCliente { get; set; }
        public DateTime FechaRegistroEmpresa { get; set; }
        public string RazonSocial { get; set; }
        public string RFC { get; set; }
        public string Sucursal { get; set; }
        public int IdEmpleado { get; set; }
        public string Nombre { get; set; }
        public string Paterno { get; set; }
        public string Materno { get; set; } 
        public int IdViaje  { get; set; } 
    
    }
    public class ResultJsonList
    {
        public List<Clientes> Data { get; set; }
    }
    public class Informacion
    {
        public string Data { get; set; }
    }
}
