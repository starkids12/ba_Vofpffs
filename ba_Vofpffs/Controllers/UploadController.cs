﻿using System;
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

            if (_context.FileEntryItemsA.Count() == 0)
            {
                _logger.LogWarning("DbSet is empty", test);
            }
        }

        // GET api/upload
        [HttpGet]
        [Route("api/uploadA")]
        public JsonResult GetA()
        {
            return Json(_context.FileEntryItemsA.ToList());
        }

        [HttpGet]
        [Route("api/uploadB")]
        public JsonResult GetB()
        {
            return Json(_context.FileEntryItemsB.ToList());
        }

        // POST api/upload
        [HttpPost]
        [Route("api/uploadA")]
        public void PostA()
        {
            string headers = string.Join("|", Request.Headers.Select(x => String.Format("{0}={1}", x.Key, x.Value)).ToArray());

            string headerFingerprint = "";

            foreach (var header in Request.Headers)
            {
                if (header.Key == "User-Agent" || header.Key == "Accept-Encoding" || header.Key == "Accept")
                {
                    headerFingerprint += String.Format("{0}={1}|", header.Key, header.Value);
                }
            }

            string ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            var files = Request.Form.Files;
            List<FileEntryItemA> fileEntrys = new List<FileEntryItemA>();

            foreach (var file in files)
            {
                byte[] fileArray = new byte[file.Length];

                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    fileArray = memoryStream.ToArray();
                }

                fileEntrys.Add(new FileEntryItemA
                {
                    Hash = file.FileName,
                    File = null,
                    Size = fileArray.Length,
                    Headers = headers,
                    HeaderFingerprint = headerFingerprint,
                    IPAddress = ipAddress,
                    DateTime = DateTime.Now
                });
            }

            if (fileEntrys.Count != 0)
            {
                _context.FileEntryItemsA.AddRange(fileEntrys);
                _context.SaveChanges();
            }
        }

        // POST api/upload
        [HttpPost]
        [Route("api/uploadB")]
        public void PostB()
        {
            string headers = string.Join(";", Request.Headers.Select(x => String.Format("{0}={1}", x.Key, x.Value)).ToArray());

            string headerFingerprint = "";

            foreach(var header in Request.Headers)
            {
                if (header.Key == "User-Agent" || header.Key == "Accept-Encoding" || header.Key == "Accept")
                {
                    headerFingerprint += String.Format("{0}={1}|", header.Key, header.Value);
                }
            }

            string ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            var files = Request.Form.Files;
            List<FileEntryItemB> fileEntrys = new List<FileEntryItemB>();

            foreach (var file in files)
            {
                byte[] fileArray = new byte[file.Length];

                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    fileArray = memoryStream.ToArray();
                }

                fileEntrys.Add(new FileEntryItemB
                {
                    Hash = file.FileName,
                    File = null,
                    Size = fileArray.Length,
                    Headers = headers,
                    HeaderFingerprint = headerFingerprint,
                    IPAddress = ipAddress,
                    DateTime = DateTime.Now
                });
            }

            if (fileEntrys.Count != 0)
            {
                _context.FileEntryItemsB.AddRange(fileEntrys);
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
