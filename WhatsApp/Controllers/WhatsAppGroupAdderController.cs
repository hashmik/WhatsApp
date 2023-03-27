using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FolkerKinzel.VCards;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using System.Web;
using Microsoft.AspNetCore.Mvc;

using Twilio.Rest.Api.V2010.Account.Queue;
using Twilio.Rest.Chat.V2.Service.Channel;
using Twilio.Rest.Chat.V1.Service;
using System.Formats.Asn1;
using System.Globalization;
using CsvHelper;
using OfficeOpenXml;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;

namespace WhatsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WhatsAppGroupAdderController : ControllerBase
    {
        private readonly ILogger<WhatsAppGroupAdderController> _logger;
        public WhatsAppGroupAdderController(ILogger<WhatsAppGroupAdderController> logger)
        {
            _logger= logger;
        }

        [HttpPost("AddContactsToGroup")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string),400)]
        public async Task<IActionResult> AddContactsToGroup([FromForm] IFormFile file)
        {
            if (file == null || file.Length <= 0)
            {
                return BadRequest("Invalid file");
            }

            using var stream = new MemoryStream();

            await file.CopyToAsync(stream);

            List<string> contacts = new List<string>();

            using var reader = new StreamReader(stream);

            Console.WriteLine(await reader.ReadToEndAsync());

            stream.Position = 0;

            using var parser = new TextFieldParser(reader)
            {
                TextFieldType = FieldType.Delimited,
                Delimiters = new[] { "," },
                HasFieldsEnclosedInQuotes = true
            };

            while (!parser.EndOfData)
            {
                var fields = parser.ReadFields();

                if (fields?.Length >= 2)
                {
                    var contact = fields[2];

                    if (!string.IsNullOrWhiteSpace(contact))
                    {
                        contacts.Add(contact);

                        _logger.LogError(contacts.Count.ToString());

                    }
                }
            }


            //
            var res=JsonConvert.SerializeObject(contacts);

            return Ok(res);

        }
    }
}