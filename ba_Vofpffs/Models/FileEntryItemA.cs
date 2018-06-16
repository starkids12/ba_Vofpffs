using System;
using System.Globalization;

namespace ba_Vofpffs.Models
{
    public class FileEntryItemA : FileEntryItem
    {
        public FileEntryItemA()
        {

        }

        public FileEntryItemA(string hash, byte[] file, int size, string ipAddress, string headers, string headerFingerprint, 
            DateTime dateTime, string country, string regionName, string city, string lat, string lon, string isp) : base 
            (hash, file, size, ipAddress, headers, headerFingerprint, dateTime, country, regionName, city, lat, lon, isp)
        {

        }

    }
}

