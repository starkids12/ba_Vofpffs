using System;

namespace ba_Vofpffs.Models
{
    public class FileEntryItemB : FileEntryItem
    {
        public FileEntryItemB()
        {

        }

        public FileEntryItemB(string hash, byte[] file, int size, string ipAddress, string headers, string headerFingerprint, DateTime dateTime,
            string country, string regionName, string city, string lat, string lon, string isp) : base (hash, file, size, ipAddress, headers, headerFingerprint, dateTime, country, regionName, city, lat, lon, isp)
        {

        }
    }
}
