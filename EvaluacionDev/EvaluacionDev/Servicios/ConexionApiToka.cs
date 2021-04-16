using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EvaluacionDev.Servicios
{
    public class ConexionApiToka
    {

        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _clientFactory;
        public ConexionApiToka(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _config = configuration;
        }

        public async Task<string> Conectar()
        {
            var token = "";
            var usuario = _config.GetValue<string>("CredencialesApi:User");
            var password = _config.GetValue<string>("CredencialesApi:Password");
            var url = _config.GetValue<string>("ConexionApiToka:UrlAutenticacion");

            var client = _clientFactory.CreateClient();
            var content = $"{{\"Username\":\"{usuario}\",\"password\":\"{password}\"}}";

            var response = await client.PostAsync(url, new StringContent(content, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                token = await response.Content.ReadAsStringAsync();
            }
            return token;
        }



        public async Task<string> ConectarToken(string token)
        {
            var resultado = "";
            var client = _clientFactory.CreateClient();
            var url = _config.GetValue<string>("ConexionApiToka:UrlConsulta");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                resultado = await response.Content.ReadAsStringAsync();
            }
            return resultado;
        }

    }
}
