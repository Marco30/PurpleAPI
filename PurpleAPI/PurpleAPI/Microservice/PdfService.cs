using System.IO;
using System.Threading.Tasks;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using PurpleAPI.Interface;


namespace PurpleAPI.Microservice
{
  
    public class PdfService : IPdfService
    {
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
