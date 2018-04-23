using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ba_Vofpffs.Models;

namespace ba_Vofpffs.Pages
{
    public class FileEntryDBListModel : PageModel
    {
        private readonly ba_Vofpffs.Models.FileEntryContext _context;

        public FileEntryDBListModel(ba_Vofpffs.Models.FileEntryContext context)
        {
            _context = context;
        }

        public IList<FileEntryItem> FileEntryItem { get;set; }

        public async Task OnGetAsync()
        {
            FileEntryItem = await _context.FileEntryItems.ToListAsync();
        }
    }
}
