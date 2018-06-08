using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ba_Vofpffs.Models
{
    public class FileEntryItemA
    {
        public FileEntryItemA()
        {

        }

        public FileEntryItemA(string hash, byte[] file, int size, string ipAddress, string headers, string headerFingerprint, DateTime dateTime, 
            string country, string regionName, string city, string lat, string lon, string isp)
        {
            this.Hash = hash;
            this.File = file;
            this.Size = size;
            this.IPAddress = ipAddress;
            this.Headers = headers;
            this.DateTime = dateTime;
            this.HeaderFingerprint = headerFingerprint;
            this.Country = country;
            this.RegionName = regionName;
            this.City = city;
            this.Lat = float.Parse (lat);
            this.Lon = float.Parse (lon);
            this.Isp = isp;
        }

        public int ID { get; set; }
        public string Hash { get; set; }
        public byte[] File { get; set; }
        public int Size { get; set; }
        public string IPAddress { get; set; }
        public string Headers{ get; set; }
        public string HeaderFingerprint { get; set; }
        public DateTime DateTime { get; set; }
        public string Country { get; set; }
        public string RegionName { get; set; }
        public string City { get; set; }
        public float Lat { get; set; }
        public float Lon { get; set; }
        public string Isp { get; set; }
    }
}
