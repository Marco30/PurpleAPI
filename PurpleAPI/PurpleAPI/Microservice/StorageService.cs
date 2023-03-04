using PurpleAPI.Interface;
using PurpleAPI.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;


namespace PurpleAPI.Microservice
{
    public class StorageService : IStorageService
    {
        readonly static string startupPath = Environment.CurrentDirectory;
        private readonly string _JsonDbFilePath = $@"{startupPath}/JsonDB/" + "PdfData.json"; // path to JSON file
        private readonly List<Pdf> _pdfs; // list of PDFs to simulate database

        public StorageService()
        {

            // Load PDFs from JSON file
            if (File.Exists(_JsonDbFilePath))
            {
                string json = File.ReadAllText(_JsonDbFilePath);
                _pdfs = JsonSerializer.Deserialize<List<Pdf>>(json);
            }
            else
            {
                _pdfs = new List<Pdf>();
            }
        }

        public async Task<string> SavePdfAsync(byte[] pdfBytes)
        {
            // Save PDF to storage (e.g. file system, database, cloud storage)
            string pdfId = Guid.NewGuid().ToString();
            var pdf = new Pdf
            {
                Id = pdfId,
                Bytes = pdfBytes
            };
            _pdfs.Add(pdf);

            // Save PDFs to JSON file
            string json = JsonSerializer.Serialize(_pdfs);
            await File.WriteAllTextAsync(_JsonDbFilePath, json);

            return pdfId;
        }
    }
}
