using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using InventarioDesktop.Models;

namespace InventarioDesktop.Services
{
    public static class ApiService
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly string apiUrl = "http://localhost:8100/productos";

        public static async Task<bool> EnviarProductoAsync(Producto producto)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = false
                };

                var json = JsonSerializer.Serialize(producto, options);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Producto '{producto.Nombre}' enviado correctamente a la API.");
                    return true;
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error del servidor ({response.StatusCode}): {error}");
                    return false;
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"No se pudo conectar con la API: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado al enviar producto: {ex.Message}");
                return false;
            }
        }
    }
}
