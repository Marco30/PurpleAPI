using PurpleAPI.Interface;
using PurpleAPI.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;
using System.Text;

namespace PurpleAPI.Microservice
{
    public class StorageService : IStorageService
    {
        readonly static string startupPath = Environment.CurrentDirectory;
        private readonly string _JsonDbFilePath = $@"{startupPath}/JsonDB/" + "PdfData.json"; // path to JSON file
        private readonly List<Pdf> _pdfs; // list of PDFs to simulate database
    private readonly IModel _channel;

        public StorageService(IModel channel)
        {
            _channel = channel;

            // Initialize the list of PDFs
            _pdfs = new List<Pdf>();

            // Initialize the list of PDFs
            if (File.Exists(_JsonDbFilePath))
            {
                string json = File.ReadAllText(_JsonDbFilePath);
                _pdfs = JsonSerializer.Deserialize<List<Pdf>>(json);
            }

            // Declare the exchange and bind to the queue
            _channel.ExchangeDeclare(exchange: "document-exchange", type: ExchangeType.Direct, durable: true);
            _channel.QueueBind(queue: "storage", exchange: "document-exchange", routingKey: "document-pdf");

            // Create the consumer and start listening for messages
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (sender, eventArgs) =>
            {
                var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
                Console.WriteLine($"Received message: {message}");

                // Extract the document data from the message body
                var documentData = message;

                // Simulate storing the PDF in a file
                var pdfBytes = Encoding.UTF8.GetBytes(documentData);
                var pdfId = await SavePdfAsync(pdfBytes);
                Console.WriteLine($"PDF saved with ID: {pdfId}");
            };
            _channel.BasicConsume(queue: "storage", autoAck: true, consumer: consumer);
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
