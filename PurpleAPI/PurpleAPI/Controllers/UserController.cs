using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PurpleAPI.Interface;
using PurpleAPI.Microservice;
using PurpleAPI.Model;
using System.Text;
using System.Threading.Channels;
using static iText.Kernel.Pdf.Colorspace.PdfSpecialCs;

namespace PurpleAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {

        private readonly RabbitMQService _rabbitMQService;
        private readonly DocumentService _documentService;
        private readonly PdfService _pdfService;
        private readonly StorageService _storageService;

        public UserController(RabbitMQService rabbitMQService)
        {
            _rabbitMQService = rabbitMQService;
            var channel = _rabbitMQService.GetChannel();
            _documentService = new DocumentService(channel);
            _pdfService = new PdfService(channel);
            _storageService = new StorageService(channel);
        }

        // POST: UserController/Create
        [HttpPost("StartTest")]
        // [ValidateAntiForgeryToken]
        public async Task<ActionResult> Test()
        {
            var customerNumber = "67890";

            var document = await _documentService.GetDocumentAsync(customerNumber);


            if (document == null)
            {
                return NotFound(document);
            }

            return Ok(document);

        }


    }
}
