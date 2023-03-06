using System.IO;
using System.Threading.Tasks;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using PurpleAPI.Interface;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using PurpleAPI.Model;
using System.Text.Json;
using iText.Kernel.Pdf.Action;

namespace PurpleAPI.Microservice
{

    public class PdfService : IPdfService
    {
        private readonly IModel _channel;
        private readonly EventingBasicConsumer _consumer;

        public  PdfService(IModel channel)
        {
            _channel = channel;

            // Declare the pdf-generation queue and start consuming messages from it
            _channel.QueueDeclare(queue: "pdf-generation", durable: true, exclusive: false, autoDelete: false, arguments: null);
            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
            _consumer = new EventingBasicConsumer(_channel);
            _consumer.Received += async (model, ea) =>
            {
                var customerData = Encoding.UTF8.GetString(ea.Body.ToArray());

                // Deserialize the message into a class model
                var UserDocument = JsonSerializer.Deserialize<UserDocument>(customerData);

                // Generate a PDF for the customer number
                //var pdfContent = await GeneratePdfAsync(customerNumber);
                var pdf = new Pdf();

                pdf.UserId = UserDocument.CustomerNumber;
                pdf.Bytes = await GeneratePdfAsync(UserDocument.DocumentText);

                // Publish a message to the storage queue with the PDF content
                // _channel.BasicPublish(exchange: "", routingKey: "storage", basicProperties: null, body: pdfContent);

                var data = JsonSerializer.Serialize(pdf);

                // Encode the JSON string into a byte array
                var body = Encoding.UTF8.GetBytes(data);


                // Publish a message to the storage queue with the PDF content
                _channel.BasicPublish(exchange: "", routingKey: "storage", basicProperties: null, body: body);

                // Acknowledge the message
                _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };
            _channel.BasicConsume(queue: "pdf-generation", autoAck: false, consumer: _consumer);
        }

        public async Task<byte[]> GeneratePdfAsync(string documentText)
        {
            // Define the PDF document
            MemoryStream stream = new MemoryStream();
            PdfWriter writer = new PdfWriter(stream);
            PdfDocument pdfDoc = new PdfDocument(writer);
            Document document = new Document(pdfDoc);

            // Add the document text
            Paragraph paragraph = new Paragraph(documentText);
            document.Add(paragraph);

            // Close the document
            document.Close();

            // Convert the PDF document to a byte array
            byte[] pdfBytes = stream.ToArray();

            return pdfBytes;
        }
    }

}

