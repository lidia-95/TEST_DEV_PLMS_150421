using EvaluacionDev.Models;
using EvaluacionDev.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NonFactors.Mvc.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace EvaluacionDev.Controllers
{
    [Authorize]
    public class AdministracionController : Controller
    {
        readonly ConexionApi conexion;
        public AdministracionController(IConfiguration configuration)
        {
            conexion = new ConexionApi(configuration);

        }
        public IActionResult Index()
        {
            var personas = Convert(conexion.ConsultaPersonas());

            return View(personas.AsQueryable());
        }

        [HttpPost]
        public IActionResult Index(int id, string nombre)
        {
            var personas = Convert(conexion.ConsultaPersona(id, nombre));

            return View(personas.AsQueryable());
        }
        public IActionResult Agregar()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Agregar(PersonaRegistroVM registroVM)
        {
            if (ModelState.IsValid)
            {
                var personaFisica = new PersonaFisica
                {
                    Nombre = registroVM.Nombre,
                    ApellidoPaterno = registroVM.ApellidoPaterno,
                    ApellidoMaterno = registroVM.ApellidoMaterno,
                    RFC = registroVM.RFC.ToUpper(),
                    FechaNacimiento = registroVM.FechaNacimiento,
                    UsuarioAgrega = registroVM.UsuarioAgrega
                };
                var resultado = conexion.AgregarPersona(personaFisica);

                var stringObject = JsonConvert.DeserializeObject(resultado).ToString();

                var jsonInformation = JsonConvert.DeserializeObject<ResultadoSQL>(stringObject);

                //Registro exitoso
                if (jsonInformation.MENSAJEERROR.ToLower() == "registro exitoso")
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Mensaje = jsonInformation.MENSAJEERROR;
                    return View();
                }
            }
            else
            {
                ViewBag.Mensaje = "Ingrese los datos correctamente.";
                return View();
            }
        }


        public IActionResult Actualizar(int id)
        {

            var personas = conexion.ConsultaPersonaPorId(id);
            var jsonResult = JsonConvert.DeserializeObject(personas).ToString();

            var jsonInformation = JsonConvert.DeserializeObject<PersonaFisica>(jsonResult);
            var personaVM = new PersonaActualizaVM
            {
                ApellidoMaterno = jsonInformation.ApellidoPaterno,
                ApellidoPaterno = jsonInformation.ApellidoMaterno,
                FechaNacimiento = jsonInformation.FechaNacimiento,
                IdPersonaFisica = jsonInformation.IdPersonaFisica,
                Nombre = jsonInformation.Nombre,
                RFC = jsonInformation.RFC,
                UsuarioAgrega = jsonInformation.UsuarioAgrega
            };
            return View(personaVM);
        }

        [HttpPost]
        public IActionResult Actualizar(PersonaActualizaVM personaVM)
        {
            if (ModelState.IsValid)
            {

                var resultado = conexion.ConsultaPersonaPorId(personaVM.IdPersonaFisica);
                var stringJson = JsonConvert.DeserializeObject(resultado).ToString();
                var persona = JsonConvert.DeserializeObject<PersonaFisica>(stringJson);

                persona.Nombre = personaVM.Nombre;
                persona.ApellidoPaterno = personaVM.ApellidoPaterno;
                persona.ApellidoMaterno = personaVM.ApellidoMaterno;
                persona.RFC = personaVM.RFC.ToUpper();
                persona.FechaActualizacion = personaVM.FechaNacimiento;
                persona.UsuarioAgrega = personaVM.UsuarioAgrega;

                resultado = conexion.ActualizarPersona(persona);
                var stringObject = JsonConvert.DeserializeObject(resultado).ToString();

                var jsonInformation = JsonConvert.DeserializeObject<ResultadoSQL>(stringObject);

                //Registro exitoso
                if (jsonInformation.MENSAJEERROR.ToLower() == "registro exitoso")
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Mensaje = jsonInformation.MENSAJEERROR;
                    return View();
                }

            }
            else
            {
                ViewBag.Mensaje = "Ingrese los datos correctamente.";
                return View(personaVM);
            }
        }


        public IActionResult Eliminar(int id)
        {
            conexion.EliminarPersona(id);
            return Json("success");
        }

        private List<PersonaFisica> Convert(string valueJson)
        {
            var lista = new List<PersonaFisica>();
            try
            {
                var jsonResult = JsonConvert.DeserializeObject(valueJson).ToString();
                JArray array = JArray.Parse(jsonResult);

                for (int i = 0; i < array.Count(); i++)
                {

                    var jsonInformation = JsonConvert.DeserializeObject<PersonaFisica>(array[i].ToString());
                    lista.Add(jsonInformation);
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            return lista;
        }

    }
}
