using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ba_Vofpffs.Models;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.IO;
using Newtonsoft.Json;

namespace ba_Vofpffs.Controllers
{

    public class UploadController : Controller
    {
        private readonly FileEntryContext _context;
        private readonly ILogger _logger;
        private String test = "123";

        public UploadController(ILogger<UploadController> logger, FileEntryContext context)
        {
            _logger = logger;
            _context = context;

            if (_context.FileEntryItems.Count() == 0)
            {
                _logger.LogWarning("DbSet is empty", test);
            }
        }

        // GET api/upload
        [HttpGet]
        [Route("api/upload")]
        public JsonResult Get()
        {
            return Json(_context.FileEntryItems.ToList());
        }

        // POST api/upload
        [HttpPost]
        [Route("api/upload")]
        public void Post()
        {
            string headers = string.Join(";", Request.Headers.Select(x => String.Format("{0}={1}", x.Key, x.Value)).ToArray());

            string ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            var files = Request.Form.Files;
            List<FileEntryItem> fileEntrys = new List<FileEntryItem>();

            foreach (var file in files)
            {
                byte[] fileArray = new byte[file.Length];

                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    fileArray = memoryStream.ToArray();
                }

                fileEntrys.Add(new FileEntryItem
                {
                    Hash = file.FileName,
                    File = null,
                    Size = fileArray.Length,
                    Headers = headers,
                    IPAddress = ipAddress,
                    DateTime = DateTime.Now
                });
            }

            if (fileEntrys.Count != 0)
            {
                _context.FileEntryItems.AddRange(fileEntrys);
                _context.SaveChanges();
            }
        }

        // PUT api/upload/5
        [HttpPut("{id}")]
        [Route("api/upload")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/upload/5
        [HttpDelete("{id}")]
        [Route("api/upload")]
        public void Delete(int id)
        {
        }
    }
}
