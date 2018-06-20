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
        public IActionResult OnGet()
        {
            return Page ();
        }
    }
}