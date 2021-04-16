using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EvaluacionDev.Models
{
    public class PersonaFisica
    {
        [Key]
        public int IdPersonaFisica { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string RFC { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public int? UsuarioAgrega { get; set; }
        public bool? Activo { get; set; }
    }

    public class PersonaRegistroVM
    {
        [Required(ErrorMessage = "*")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "*")]
        public string ApellidoPaterno { get; set; }

        public string ApellidoMaterno { get; set; }

        [Required(ErrorMessage = "*")]
        [RegularExpression(pattern: "[A-Za-z]{4}[0-9]{2}[01][0-9][0-3][0-9][!-}ñÑ]{3}", ErrorMessage = "*Formato no válido")]
        public string RFC { get; set; }

        [Required(ErrorMessage = "*")]
        public DateTime? FechaNacimiento { get; set; }

        [Required(ErrorMessage = "*")]
        public int? UsuarioAgrega { get; set; }

    }
    public class PersonaActualizaVM
    {
        [Required(ErrorMessage = "*")]
        public int IdPersonaFisica { get; set; }

        [Required(ErrorMessage = "*")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "*")]
        public string ApellidoPaterno { get; set; }

        public string ApellidoMaterno { get; set; }

        [Required(ErrorMessage = "*")]
        [RegularExpression(pattern: "[A-Za-z]{4}[0-9]{2}[01][0-9][0-3][0-9][!-}ñÑ]{3}", ErrorMessage = "*Formato no válido")]
        public string RFC { get; set; }

        [Required(ErrorMessage = "*")]
        public DateTime? FechaNacimiento { get; set; }

        [Required(ErrorMessage = "*")]
        public int? UsuarioAgrega { get; set; }
    }


    public class ResultadoSQL
    {
        public int ERROR { get; set; }
        public string MENSAJEERROR { get; set; }

    }
}
