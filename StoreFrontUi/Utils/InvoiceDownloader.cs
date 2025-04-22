using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StoreFrontUi.Utils
{
    public class InvoiceDownloader
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;

        public InvoiceDownloader(string serverUrl, string username, string password)
        {
          
            _baseUrl = serverUrl.TrimEnd('/');

            _client = new HttpClient();
            var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);
        }

        public async Task<bool> DownloadReport(string reportUri, string outputPath, string paramName, string paramValue)
        {
            try
            {
          
                reportUri = "/" + reportUri.Trim('/');

                string encodedValue = Uri.EscapeDataString(paramValue);

          
                var url = $"{_baseUrl}/rest_v2/reports{reportUri}.pdf?{paramName}={encodedValue}";
                Console.WriteLine($"Requesting: {url}");

                var response = await _client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                  
                    Directory.CreateDirectory(Path.GetDirectoryName(outputPath));

                    using (var fileStream = File.Create(outputPath))
                    {
                        await response.Content.CopyToAsync(fileStream);
                    }

                    if (File.Exists(outputPath) && new FileInfo(outputPath).Length > 0)
                    {
                        Console.WriteLine($"Report downloaded successfully to {outputPath}");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine($"File not created or is empty at {outputPath}");
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine($"Failed to download report. Status: {response.StatusCode}");
                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in DownloadReport: {ex}");
                return false;
            }
        }
    }
}
