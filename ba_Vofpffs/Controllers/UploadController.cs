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
using System.Net;

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

            if(_context.FileEntryItemsA.Count () == 0)
            {
                _logger.LogWarning ("DbSet is empty", test);
            }
        }

        // GET api/upload
        [HttpGet]
        [Route ("api/uploadA")]
        public JsonResult GetA()
        {
            return Json (_context.FileEntryItemsA.ToList ());
        }

        [HttpGet]
        [Route ("api/uploadB")]
        public JsonResult GetB()
        {
            return Json (_context.FileEntryItemsB.ToList ());
        }

        // POST api/upload
        [HttpPost]
        [Route ("api/uploadA")]
        public void PostA()
        {
            ProcessPost (Request, "A");
        }

        // POST api/upload
        [HttpPost]
        [Route ("api/uploadB")]
        public void PostB()
        {
            ProcessPost (Request, "B");
        }

        // PUT api/upload/5
        [HttpPut ("{id}")]
        [Route ("api/upload")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/upload/5
        [HttpDelete ("{id}")]
        [Route ("api/upload")]
        public void Delete(int id)
        {
        }

        public Dictionary<string, string> getGeoInfo(string ip) {

            List<string> o = new List<string> ();

            string json = "";
            string url = @"http://ip-api.com/json/" + ip;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create (url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse ();

            using(var streamReader = new StreamReader (response.GetResponseStream ()))
            {
                json = streamReader.ReadToEnd ();
            }

            var info = JsonConvert.DeserializeObject<Dictionary<string, string>> (json);

            string country, regionName, city, lat, lon, isp;

            info.TryGetValue ("country", out country);
            info.TryGetValue ("regionName", out regionName);
            info.TryGetValue ("city", out city);
            info.TryGetValue ("lat", out lat);
            info.TryGetValue ("lon", out lon);
            info.TryGetValue ("isp", out isp);

            o.Add (country);
            o.Add (regionName);
            o.Add (city);
            o.Add (lat);
            o.Add (lon);
            o.Add (isp);


            return info;
        }

        public void ProcessPost(HttpRequest request, string set)
        {
            DateTime dateTime = DateTime.Now;

            string headers = string.Join (";", Request.Headers.Select (x => String.Format ("{0}={1}", x.Key, x.Value)).ToArray ());

            string headerFingerprint = "";

            foreach(var header in Request.Headers)
            {
                if(header.Key == "User-Agent" || header.Key == "Accept-Encoding" || header.Key == "Accept")
                {
                    headerFingerprint += String.Format ("{0}={1}|", header.Key, header.Value);
                }
            }

            string ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString ();

            Dictionary<string, string> geoInfo = getGeoInfo (ipAddress);

            string country, regionName, city, lat, lon, isp;

            geoInfo.TryGetValue ("country", out country);
            geoInfo.TryGetValue ("regionName", out regionName);
            geoInfo.TryGetValue ("city", out city);
            geoInfo.TryGetValue ("lat", out lat);
            geoInfo.TryGetValue ("lon", out lon);
            geoInfo.TryGetValue ("isp", out isp);

            var files = Request.Form.Files;

            if (set == "A")
            {
                List<FileEntryItemA> fileEntrys = new List<FileEntryItemA> ();

                foreach(var file in files)
                {
                    byte[] fileArray = new byte[file.Length];

                    using(var memoryStream = new MemoryStream ())
                    {
                        file.CopyTo (memoryStream);
                        fileArray = memoryStream.ToArray ();
                    }

                    fileEntrys.Add (new FileEntryItemA (file.FileName, null, fileArray.Length, headers, headerFingerprint, ipAddress, dateTime, country, regionName, city, lat, lon, isp));
                }

                if(fileEntrys.Count != 0)
                {
                    _context.FileEntryItemsA.AddRange (fileEntrys);
                    _context.SaveChanges ();
                }
            }
            else
            {
                List<FileEntryItemB> fileEntrys = new List<FileEntryItemB> ();

                foreach(var file in files)
                {
                    byte[] fileArray = new byte[file.Length];

                    using(var memoryStream = new MemoryStream ())
                    {
                        file.CopyTo (memoryStream);
                        fileArray = memoryStream.ToArray ();
                    }

                    fileEntrys.Add (new FileEntryItemB (file.FileName, null, fileArray.Length, headers, headerFingerprint, ipAddress, dateTime, country, regionName, city, lat, lon, isp));
                }

                if(fileEntrys.Count != 0)
                {
                    _context.FileEntryItemsB.AddRange (fileEntrys);
                    _context.SaveChanges ();
                }
            }
        }
     }
}
