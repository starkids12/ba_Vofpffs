﻿using System;
using System.Globalization;

namespace ba_Vofpffs.Models
{
    public class FileEntryItem
    {
        public FileEntryItem()
        {

        }

        public FileEntryItem(string hash, byte[] file, int size, string ipAddress, string headers, string headerFingerprint, DateTime dateTime,
            string country, string regionName, string city, string lat, string lon, string isp)
        {
            this.Filename = hash;
            this.File = file;
            this.Size = size;
            this.IPAddress = ipAddress;
            this.Headers = headers;
            this.DateTime = dateTime;
            this.HeaderFingerprint = headerFingerprint;
            this.Country = country;
            this.RegionName = regionName;
            this.City = city;

            if(lat != null)
                this.Lat = float.Parse (lat, CultureInfo.InvariantCulture);
            else
                this.Lat = 0;

            if(lon != null)
                this.Lon = float.Parse (lon, CultureInfo.InvariantCulture);
            else
                this.Lat = 0;

            this.Isp = isp;
        }

        public int ID { get; set; }
        public string Filename { get; set; }
        public byte[] File { get; set; }
        public int Size { get; set; }
        public string IPAddress { get; set; }
        public string Headers { get; set; }
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