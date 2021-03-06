﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ba_Vofpffs.Models;
using Microsoft.EntityFrameworkCore;

namespace ba_Vofpffs.Pages
{
    public class FileEntryModel : PageModel
    {

        private readonly ba_Vofpffs.Models.FileEntryContext _context;

        public FileEntryModel(ba_Vofpffs.Models.FileEntryContext context)
        {
            _context = context;
        }

        public IList<FileEntryItem> FileEntryItems { get; set; }

        public async void OnGetAsync()
        {
            FileEntryItems = await _context.FileEntryItems.ToListAsync();
        }
    }
}