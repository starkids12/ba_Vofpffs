using System;

namespace ba_Vofpffs.Models
{
    public class FileEntryItemB : FileEntryItem
    {
        public FileEntryItemB()
        {

        }

        public FileEntryItemB(string hash, string filepath, long size, string ipAddress, string headers, string headerFingerprint, DateTime dateTime,
            string country, string regionName, string city, string lat, string lon, string isp) : base 
            (hash, filepath, size, ipAddress, headers, headerFingerprint, dateTime, country, regionName, city, lat, lon, isp)
        {

        }
    }
}