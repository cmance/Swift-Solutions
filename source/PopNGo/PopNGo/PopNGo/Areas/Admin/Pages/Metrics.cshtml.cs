// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using PopNGo.Areas.Identity.Data;

namespace PopNGo.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class MetricsModel : PageModel
    {
        public MetricsModel()
        {
        }

        [TempData]
        public string StatusMessage { get; set; }

        public IActionResult OnGetAsync()
        {
            StatusMessage = "You are not authorized to view this page.";
            
            if(User.Identity.IsAuthenticated)
            {
                StatusMessage = "";
            }

            return Page();
        }
    }
}