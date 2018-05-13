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

        public FileEntryItemA(string hash, byte[] file, int size, string ipAddress, string headers, string headerFingerprint, DateTime dateTime)
        {
            this.Hash = hash;
            this.File = file;
            this.Size = size;
            this.IPAddress = ipAddress;
            this.Headers = headers;
            this.DateTime = dateTime;
            this.HeaderFingerprint = headerFingerprint;
        }

        public int ID { get; set; }
        public string Hash { get; set; }
        public byte[] File { get; set; }
        public int Size { get; set; }
        public string IPAddress { get; set; }
        public string Headers{ get; set; }
        public string HeaderFingerprint { get; set; }
        public DateTime DateTime { get; set; }
    }
}
