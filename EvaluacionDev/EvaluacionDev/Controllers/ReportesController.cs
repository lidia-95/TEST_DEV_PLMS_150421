using EvaluacionDev.Models;
using EvaluacionDev.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EvaluacionDev.Controllers
{

    [Authorize]
    public class ReportesController : Controller
    {
        readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _config;
        readonly ConexionApiToka apiToka;

        public ReportesController(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _config = configuration;
            _clientFactory = clientFactory;
            apiToka = new ConexionApiToka(_clientFactory, configuration);
        }
        public async Task<IActionResult> Index()
        {
            var token = await apiToka.Conectar();
            var data = JsonConvert.DeserializeObject<Informacion>(token);
            var resultado = await apiToka.ConectarToken(data.Data);

            var clientes = Convert(resultado);
            return View(clientes.AsQueryable());
        }

        public async Task<IActionResult> ExportarExcel()
        {
            var token = await apiToka.Conectar();
            var data = JsonConvert.DeserializeObject<Informacion>(token);
            var resultado = await apiToka.ConectarToken(data.Data);

            var clientes = Convert(resultado);

            string excelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Clientes");
            worksheet.Cells.LoadFromCollection(clientes, PrintHeaders: true, TableStyles.Dark10);
            for (var columna = 1; columna < clientes.Count + 1; columna++)
            {
                worksheet.Column(columna).AutoFit();
            }
            worksheet.Cells[$"B2:B{clientes.Count() + 2}"].Style.Numberformat.Format = "mm-dd-yy";


            return File(package.GetAsByteArray(), excelContentType, "Clientes.xlsx");
        }

        private List<Clientes> Convert(string valueJson)
        {
            var lista = new List<Clientes>();
            try
            {
                var jsonResult = JsonConvert.DeserializeObject(valueJson).ToString();
                var jsonInformation = JsonConvert.DeserializeObject<ResultJsonList>(jsonResult);
                lista = jsonInformation.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }
    }
}
