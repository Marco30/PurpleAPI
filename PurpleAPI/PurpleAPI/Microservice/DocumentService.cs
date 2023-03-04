
using PurpleAPI.Interface;
using PurpleAPI.Model;
using System.Text.Json;


namespace PurpleAPI.Microservice
{
   
    public class DocumentService : IDocumentService
    {
        private readonly List<UserDocument> _documents;
        readonly static string startupPath = Environment.CurrentDirectory;
        private readonly string _jsonDbFilePath = $@"{startupPath}/JsonDB/" + "DocumentData.json";


        public DocumentService()
        {
            

            if (File.Exists(_jsonDbFilePath))
            {
                // Load documents from JSON file if it exists
                string json = File.ReadAllText(_jsonDbFilePath);
                _documents = JsonSerializer.Deserialize<List<UserDocument>>(json);
            }
            else
            {
                // Create an empty list if the JSON file does not exist
                _documents = new List<UserDocument>();
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

            // Return the found or newly created document
            return document;
        }
    }
}
