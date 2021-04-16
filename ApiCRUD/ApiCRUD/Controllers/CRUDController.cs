using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiCRUD.Data;
using ApiCRUD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Text.Json;
using Newtonsoft.Json;


namespace ApiCRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CRUDController : ControllerBase
    {
        private readonly ContextoAplicacion _contexto;


        public CRUDController(ContextoAplicacion contexto)
        {
            _contexto = contexto;
        }


        [HttpGet]
        public string Inicio()
        {
            return "";
        }

        [HttpGet]
        [Route("ConsultaTodo")]
        public string ConsultaTodo()
        {
            var personas = _contexto.Tb_PersonasFisicas.Where(p => p.Activo == true).ToList();

         
            var json = JsonConvert.SerializeObject(personas, Formatting.Indented);

            return json;

        }
   

        [HttpGet]
        [Route("ConsultaPersonas/{id}/{nombre}")]
        public string ConsultaPersonas(int id, string nombre)
        {
            var personas = _contexto.Tb_PersonasFisicas.Where(p => p.Nombre.Contains(nombre) || p.IdPersonaFisica == id).ToList();

            var json = JsonConvert.SerializeObject(personas, Formatting.Indented);


            return json;
        }

        [HttpGet]
        [Route("ConsultaPorId/{id}")]
        public string ConsultaPorId(int id)
        {
            var persona = _contexto.Tb_PersonasFisicas.FirstOrDefault(p => p.IdPersonaFisica == id);
            var json = JsonConvert.SerializeObject(persona,Formatting.Indented);


            return json;
        }

        [HttpPost]
        [Route("InsertarPersona")]
        public string InsertarPersona([FromBody] PersonaFisica persona)
        {
            //var persona = (PersonaFisica)personaFisica;
            var resultado = new ResultadoSQL();

            using (var cmd = _contexto.Database.GetDbConnection().CreateCommand())
            {

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.CommandText = "dbo.sp_AgregarPersonaFisica";


                cmd.Parameters.Add(new SqlParameter
                { ParameterName = "@Nombre", Value = persona.Nombre, DbType = System.Data.DbType.String });

                cmd.Parameters.Add(new SqlParameter
                { ParameterName = "@ApellidoPaterno", Value = persona.ApellidoPaterno, DbType = System.Data.DbType.String });

                cmd.Parameters.Add(new SqlParameter
                { ParameterName = "@ApellidoMaterno", Value = (object)persona.ApellidoMaterno ?? DBNull.Value, DbType = System.Data.DbType.String });

                cmd.Parameters.Add(new SqlParameter
                { ParameterName = "@RFC", Value = persona.RFC, DbType = System.Data.DbType.String });

                cmd.Parameters.Add(new SqlParameter
                { ParameterName = "@FechaNacimiento", Value = persona.FechaNacimiento, DbType = System.Data.DbType.DateTime });

                cmd.Parameters.Add(new SqlParameter
                { ParameterName = "@UsuarioAgrega", Value = persona.UsuarioAgrega, DbType = System.Data.DbType.Int32 });

                try
                {
                    _contexto.Database.OpenConnection();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        resultado.ERROR = reader.GetInt32(0);
                        resultado.MENSAJEERROR = reader.GetString(1);
                    }

                    _contexto.Database.CloseConnection();

                }
                catch (Exception ex)
                {

                    resultado.ERROR = 0;
                    resultado.MENSAJEERROR = $"Exception: {ex.Message}";
                }
            }


            var json = JsonConvert.SerializeObject(resultado,Formatting.Indented);


            return json;

        }

        [HttpPost]
        [Route("ActualizarPersona")]
        public string ActualizarPersona(PersonaFisica persona)
        {
            var resultado = new ResultadoSQL();

            using (var cmd = _contexto.Database.GetDbConnection().CreateCommand())
            {

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.CommandText = "dbo.sp_ActualizarPersonaFisica";

                cmd.Parameters.Add(new SqlParameter
                { ParameterName = "@IdPersonaFisica", Value = persona.IdPersonaFisica, DbType = System.Data.DbType.Int32 });

                cmd.Parameters.Add(new SqlParameter
                { ParameterName = "@Nombre", Value = persona.Nombre, DbType = System.Data.DbType.String });

                cmd.Parameters.Add(new SqlParameter
                { ParameterName = "@ApellidoPaterno", Value = persona.ApellidoPaterno, DbType = System.Data.DbType.String });

                cmd.Parameters.Add(new SqlParameter
                { ParameterName = "@ApellidoMaterno", Value = persona.ApellidoMaterno, DbType = System.Data.DbType.String });

                cmd.Parameters.Add(new SqlParameter
                { ParameterName = "@RFC", Value = persona.RFC, DbType = System.Data.DbType.String });

                cmd.Parameters.Add(new SqlParameter
                { ParameterName = "@FechaNacimiento", Value = persona.FechaNacimiento, DbType = System.Data.DbType.DateTime });

                cmd.Parameters.Add(new SqlParameter
                { ParameterName = "@UsuarioAgrega", Value = persona.UsuarioAgrega, DbType = System.Data.DbType.Int32 });


                try
                {
                    _contexto.Database.OpenConnection();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        resultado.ERROR = reader.GetInt32(0);
                        resultado.MENSAJEERROR = reader.GetString(1);
                    }

                    _contexto.Database.CloseConnection();
                }
                catch (Exception ex)
                {

                    resultado.ERROR = 0;
                    resultado.MENSAJEERROR = $"Exception: {ex.Message}";
                }
            }
            var json = JsonConvert.SerializeObject(resultado, Formatting.Indented);

            return json;

        }


        [HttpPost]
        [Route("EliminaPersona/{idPersona}")]
        public string EliminaPersona(int idPersona)
        {
            var resultado = new ResultadoSQL();

            using (var cmd = _contexto.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.CommandText = $"dbo.sp_EliminarPersonaFisica";

                cmd.Parameters.Add(new SqlParameter
                { ParameterName = "@IdPersonaFisica", Value = idPersona, DbType = System.Data.DbType.Int32 });

                try
                {
                    _contexto.Database.OpenConnection();
                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            resultado.ERROR = reader.GetInt32(0);
                            resultado.MENSAJEERROR = reader.GetString(1);
                        }
                    }
                    else
                    {
                        resultado = null;
                    }

                }
                catch (Exception ex)
                {

                    resultado.ERROR = 0;
                    resultado.MENSAJEERROR = $"Exception: {ex.Message}";
                }
            }
            var json = JsonConvert.SerializeObject(resultado);


            return json;
        }

    }
    class ListaPersona
    {
        public List<PersonaFisica> PersonasFisicas { get; set; }
    }

}