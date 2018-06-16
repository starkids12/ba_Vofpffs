using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ba_Vofpffs.Models;


namespace ba_Vofpffs.Controllers
{

    public class UploadController : Controller
    {
        private readonly FileEntryContext _context;
        private readonly ILogger _logger;

        public UploadController(ILogger<UploadController> logger, FileEntryContext context)
        {
            _logger = logger;
            _context = context;

            if(_context.FileEntryItemsA.Count () == 0 && _context.FileEntryItemsB.Count() == 0)
            {
                _logger.LogWarning ("DbSets are empty", "");
            }
        }

        // GET api/upload
        [HttpGet]
        [Route ("api/uploadA")]
        public JsonResult GetA() => Json (_context.FileEntryItemsA.ToList ());

        [HttpGet]
        [Route ("api/uploadB")]
        public JsonResult GetB() => Json (_context.FileEntryItemsB.ToList ());

        // POST api/upload
        [HttpPost]
        [Route ("api/uploadA")]
        public void PostA() => ProcessPost (Request, "A");

        // POST api/upload
        [HttpPost]
        [Route ("api/uploadB")]
        public void PostB() => ProcessPost (Request, "B");

        // POST api/upload
        [HttpPost]
        [Route ("api/uploadEmuA")]
        public void PostEmu(string filename, string ip, int size, string header, bool setA, bool setB) => 
            ProcessPost (filename, ip, size, header, setA, setB);

        public Dictionary<string, string> GetGeoInfo(string ip)
        {

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

            string headers = string.Join ("|", Request.Headers.Select (x => String.Format ("{0}={1}", x.Key, x.Value)).ToArray ());

            string headerFingerprint = "";

            foreach(var header in Request.Headers)
            {
                if(header.Key == "User-Agent" || header.Key == "Accept-Encoding" || header.Key == "Accept")
                {
                    headerFingerprint += String.Format ("{0}={1}|", header.Key, header.Value);
                }
            }

            string ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString ();

            Dictionary<string, string> geoInfo = GetGeoInfo (ipAddress);

            string country, regionName, city, lat, lon, isp;

            geoInfo.TryGetValue ("country", out country);
            geoInfo.TryGetValue ("regionName", out regionName);
            geoInfo.TryGetValue ("city", out city);
            geoInfo.TryGetValue ("lat", out lat);
            geoInfo.TryGetValue ("lon", out lon);
            geoInfo.TryGetValue ("isp", out isp);

            var files = Request.Form.Files;

            if(set == "A")
            {
                List<FileEntryItemA> fileEntrysA = new List<FileEntryItemA> ();
                List<FileEntryItemB> fileEntrysB = new List<FileEntryItemB> ();

                foreach(var file in files)
                {
                    // full path to file in temp location
                    var filePath = Path.GetTempFileName ();

                    if(file.Length > 0)
                    {
                        using(var stream = new FileStream (filePath, FileMode.Create))
                        {
                            file.CopyTo (stream);
                        }
                    }

                    if (set == "A")
                        fileEntrysA.Add (new FileEntryItemA (file.FileName, filePath, file.Length, ipAddress, headers, headerFingerprint, dateTime, country, regionName, city, lat, lon, isp));
                    else if(set == "B")
                        fileEntrysB.Add (new FileEntryItemB (file.FileName, filePath, file.Length, ipAddress, headers, headerFingerprint, dateTime, country, regionName, city, lat, lon, isp));
                }

                if(fileEntrysA.Count != 0 && set == "A")
                {
                    _context.FileEntryItemsA.AddRange (fileEntrysA);
                }
                else if (fileEntrysB.Count != 0 && set == "B")
                {
                    _context.FileEntryItemsB.AddRange (fileEntrysB);
                }

                _context.SaveChanges ();
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

                    fileEntrys.Add (new FileEntryItemB (file.FileName, null, file.Length, ipAddress, headers, headerFingerprint, dateTime, country, regionName, city, lat, lon, isp));
                }

                if(fileEntrys.Count != 0)
                {
                    _context.FileEntryItemsB.AddRange (fileEntrys);
                    _context.SaveChanges ();
                }
            }
        }

        public void ProcessPost(string filename, string ip, int size, string headers, bool setA, bool setB)
        {
            DateTime dateTime = DateTime.Now;

            List<string> keyValuePairs = headers.Split ("|").ToList();

            Dictionary<string, string> headerDictonary = keyValuePairs.ToDictionary (x => x.Split ("=").FirstOrDefault(), x => x.Split ("=").LastOrDefault());

            string headerFingerprint = "";

            foreach(var header in headerDictonary)
            {
                if(header.Key == "User-Agent" || header.Key == "Accept-Encoding" || header.Key == "Accept")
                {
                    headerFingerprint += String.Format ("{0}={1}|", header.Key, header.Value);
                }
            }

            if (ip == null)
            {
                Random random = new Random ();

                ip = "";
                for(int i = 0; i < 4; i++)
                {
                    ip += random.Next (0, 255);
                    if(i != 3)
                        ip += ".";
                }
            }

            Dictionary<string, string> geoInfo = GetGeoInfo (ip);

            string country, regionName, city, lat, lon, isp;

            geoInfo.TryGetValue ("country", out country);
            geoInfo.TryGetValue ("regionName", out regionName);
            geoInfo.TryGetValue ("city", out city);
            geoInfo.TryGetValue ("lat", out lat);
            geoInfo.TryGetValue ("lon", out lon);
            geoInfo.TryGetValue ("isp", out isp);

            if(setA)
            {
                _context.FileEntryItemsA.Add (new FileEntryItemA (filename, null, size, ip, headers, headerFingerprint, dateTime, country, regionName, city, lat, lon, isp));
                _context.SaveChanges ();
            }

            if (setB)
            {
                _context.FileEntryItemsB.Add (new FileEntryItemB (filename, null, size, ip, headers, headerFingerprint, dateTime, country, regionName, city, lat, lon, isp));
                _context.SaveChanges ();
            }
        }
    }
}
