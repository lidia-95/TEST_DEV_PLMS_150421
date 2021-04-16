using EvaluacionDev.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EvaluacionDev.Servicios
{
    public class ConexionApi
    {

        private readonly IConfiguration _config;
        public ConexionApi( IConfiguration configuration)
        {
            _config = configuration;
        }

        public string ConsultaPersonas()
        {
            var request = CrearClienteHTTP($"ConsultaTodo", "GET");

            string respuesta = EjecutarClienteHTTP(request);

            return respuesta;
        }

        public string ConsultaPersonaPorId(int idPersona)
        {
            var request = CrearClienteHTTP($"ConsultaPorId/{idPersona}", "GET");

            string respuesta = EjecutarClienteHTTP(request);

            return respuesta;
        }

        public string ConsultaPersona(int id,string nombre)
        {
            var request = CrearClienteHTTP($"ConsultaPersonas/{id}/{nombre}", "GET");

            string respuesta = EjecutarClienteHTTP(request);

            return respuesta;
        }

        public string AgregarPersona(PersonaFisica persona)
        {
            var request = CrearClienteHTTP("InsertarPersona", "POST");
            using (var stream = new StreamWriter(request.GetRequestStream()))
            {
                stream.Write(JsonConvert.SerializeObject(persona));
                stream.Flush();
            }

            string respuesta = EjecutarClienteHTTP(request);
            Console.WriteLine(respuesta);

            return respuesta;
        }

        public string ActualizarPersona(PersonaFisica persona)
        {
            var request = CrearClienteHTTP("ActualizarPersona", "POST");
            using (var stream = new StreamWriter(request.GetRequestStream()))
            {
                stream.Write(JsonConvert.SerializeObject(persona));
                stream.Flush();
            }

            string respuesta = EjecutarClienteHTTP(request);
            Console.WriteLine(respuesta);

            return respuesta;
        }


        public string EliminarPersona(int idPersona)
        {
            var request = CrearClienteHTTP($"EliminaPersona/{idPersona}", "POST");
             
 
            string respuesta = EjecutarClienteHTTP(request);
            Console.WriteLine(respuesta);

            return respuesta;
        }

        private HttpWebRequest CrearClienteHTTP(string urlAccion, string metodo)
        {
            var url = _config.GetValue<string>("ConexionLocal:Url");
            url = $"{url}{urlAccion}";
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = metodo;
            request.ContentType = "application/json";
            request.Accept = "application/json";

            return request;

        }

        private string EjecutarClienteHTTP(HttpWebRequest request)
        {
            var respuesta = "";
            try
            {
                using var response = request.GetResponse();
                using var strReader = response.GetResponseStream();
                if (strReader == null)
                {
                    respuesta = null;
                }

                using StreamReader objReader = new StreamReader(strReader);
                respuesta = objReader.ReadToEnd();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return respuesta;
        }

    }
}
