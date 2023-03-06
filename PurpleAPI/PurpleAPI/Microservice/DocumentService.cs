
using PurpleAPI.Interface;
using PurpleAPI.Model;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;


namespace PurpleAPI.Microservice
{
   
    public class DocumentService : IDocumentService
    {
        private readonly List<UserDocument> _documents;
        readonly static string startupPath = Environment.CurrentDirectory;
        private readonly string _jsonDbFilePath = $@"{startupPath}/JsonDB/" + "DocumentData.json";
        private readonly IModel _channel;

        public DocumentService(IModel channel)
        {
            _channel = channel;

            _documents = new List<UserDocument>();

            if (File.Exists(_jsonDbFilePath))
            {
                // Load documents from JSON file if it exists
                string json = File.ReadAllText(_jsonDbFilePath);
                _documents = JsonSerializer.Deserialize<List<UserDocument>>(json);
            }
    
        }

        public async Task<UserDocument> GetDocumentAsync(string customerNumber)
        {
            // try to find the document with given customer number in the simulated database
            var document = _documents.FirstOrDefault(d => d.CustomerNumber == customerNumber);

            if (document == null)
            {
                // If the document is not found, create a new one
                document = new UserDocument
                {
                    CustomerNumber = customerNumber,
                    DocumentNumber = Guid.NewGuid().ToString(),
                    DocumentText = "Document text goes here"
                };

                // Add the new document to the simulated database
                _documents.Add(document);

                // Save the updated list of documents to the JSON file
                string json = JsonSerializer.Serialize(_documents);
                File.WriteAllText(_jsonDbFilePath, json);

                // TODO: Simulate publishing an event to generate PDF and save to storage
                // This could involve creating an event object, passing it to a method in another class,
                // which would then handle the actual event publishing and PDF generation/storage logic.
                // For the purposes of this example, we can just leave this as a TODO item.
            }

            var data = JsonSerializer.Serialize(document);

            // Encode the JSON string into a byte array
            var body = Encoding.UTF8.GetBytes(data);


            // Publish a message to the pdf-generation queue
           // var body = Encoding.UTF8.GetBytes(document.CustomerNumber);

            _channel.BasicPublish(exchange: "", routingKey: "pdf-generation", basicProperties: null, body: body);

            // Return the found or newly created document
            return document;
        }
    }
}
