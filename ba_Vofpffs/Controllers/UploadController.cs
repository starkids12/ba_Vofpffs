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

            if(_context.FileEntryItems.Count () == 0)
            {
                _logger.LogWarning ("DbSets are empty", "");
            }
        }

        // GET api/upload
        [HttpGet]
        [Route ("api/uploadA")]
        public JsonResult GetA() => Json (_context.FileEntryItems.Where (x => x.Set == "A").ToList ());

        [HttpGet]
        [Route ("api/uploadB")]
        public JsonResult GetB() => Json (_context.FileEntryItems.Where (x => x.Set == "B").ToList ());

        // POST api/upload
        [HttpPost]
        [Route ("api/uploadA")]
        public RedirectToPageResult PostA()
        {
            ProcessPost (Request, "A");
            return RedirectToPage ("/FileEntry");
        }

        // POST api/upload
        [HttpPost]
        [Route ("api/uploadB")]
        public RedirectToPageResult PostB()
        {
            ProcessPost (Request, "B");
            return RedirectToPage ("/FileEntry");
        }

        // POST api/upload
        [HttpPost]
        [Route ("api/uploadEmuA")]
        public RedirectToPageResult PostEmu(string filename, string ip, int size, string header, bool setA, bool setB)
        {
            ProcessPost (filename, ip, size, header, setA, setB);
            return RedirectToPage ("/FileEntry");
        }

        private Dictionary<string, string> GetGeoInfo(string ip)
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

        private void ProcessPost(HttpRequest request, string set)
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

            geoInfo.TryGetValue ("country", out string country);
            geoInfo.TryGetValue ("regionName", out string regionName);
            geoInfo.TryGetValue ("city", out string city);
            geoInfo.TryGetValue ("lat", out string lat);
            geoInfo.TryGetValue ("lon", out string lon);
            geoInfo.TryGetValue ("isp", out string isp);

            var files = Request.Form.Files;

            List<FileEntryItem> fileEntrys = new List<FileEntryItem> ();

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
                fileEntrys.Add (new FileEntryItem (set, file.FileName, filePath, file.Length, ipAddress, headers, headerFingerprint, dateTime, country, regionName, city, lat, lon, isp));
            }

            _context.FileEntryItems.AddRange (fileEntrys);
            _context.SaveChanges ();
        }

        private void ProcessPost(string filename, string ip, int size, string headers, bool setA, bool setB)
        {
            DateTime dateTime = DateTime.Now;

            List<string> keyValuePairs;
            Dictionary<string, string> headerDictonary;
            string headerFingerprint = "";

            if(headers != null)
            {
                keyValuePairs = headers.Split ("|").ToList ();
                headerDictonary = keyValuePairs.ToDictionary (x => x.Split ("=").FirstOrDefault (), x => x.Split ("=").LastOrDefault ());

                foreach(var header in headerDictonary)
                {
                    if(header.Key == "User-Agent" || header.Key == "Accept-Encoding" || header.Key == "Accept")
                    {
                        headerFingerprint += String.Format ("{0}={1}|", header.Key, header.Value);
                    }
                }
            }

            if(ip == null)
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

            geoInfo.TryGetValue ("country", out string country);
            geoInfo.TryGetValue ("regionName", out string regionName);
            geoInfo.TryGetValue ("city", out string city);
            geoInfo.TryGetValue ("lat", out string lat);
            geoInfo.TryGetValue ("lon", out string lon);
            geoInfo.TryGetValue ("isp", out string isp);

            if(setA)
            {
                _context.FileEntryItems.Add (new FileEntryItem ("A", filename, null, size, ip, headers, headerFingerprint, dateTime, country, regionName, city, lat, lon, isp));
            }

            if(setB)
            {
                _context.FileEntryItems.Add (new FileEntryItem ("B", filename, null, size, ip, headers, headerFingerprint, dateTime, country, regionName, city, lat, lon, isp));
            }

            _context.SaveChanges ();
        }
    }
}
