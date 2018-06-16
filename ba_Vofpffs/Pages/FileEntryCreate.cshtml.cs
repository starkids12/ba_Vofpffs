using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ba_Vofpffs.Models;

namespace ba_Vofpffs.Pages
{
    public class FileEntryCreateModel : PageModel
    {
        private readonly ba_Vofpffs.Models.FileEntryContext _context;

        public string selectedSet, filename, size, ipAddress, headers, headerFingerprint, dateTime, country, regionName, city, Lat, lon, isp;


        public FileEntryCreateModel(ba_Vofpffs.Models.FileEntryContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page ();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                return Page ();
            }

            //var ItemA = new FileEntryItemA { }

            //_context.FileEntryItemsA.Add (FileEntryItemA);
            //await _context.SaveChangesAsync ();

            return RedirectToPage ("./Index");
        }
    }
}