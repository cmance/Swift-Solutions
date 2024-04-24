// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PopNGo.Areas.Identity.Data;

namespace PopNGo.Areas.Identity.Pages.Account.Manage
{
    public class SettingsModel : PageModel
    {
        private readonly UserManager<PopNGoUser> _userManager;
        private readonly SignInManager<PopNGoUser> _signInManager;

        public SettingsModel(
            UserManager<PopNGoUser> userManager,
            SignInManager<PopNGoUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public SettingModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class SettingModel
        {
            /// <summary>
            ///    This property is used to specify a user's first name for short display purposes.
            ///    This property is required.
            /// </summary>
            [Required]
            [Display(Name = "Distance - Unit of Measurement")]
            public string DistanceUnit { get; set; }

            /// <summary>
            ///   This property is used to specify a user's preference for measuring Temperature (Fahrenheit, Celsius).
            /// </summary>
            [Required]
            [Display(Name = "Temperature - Unit of Measurement")]
            public string TemperatureUnit { get; set; }

            /// <summary>
            ///  This property is used to specify a user's preference for measuring Length/Depth (Inches, Millimeters).
            [Required]
            [Display(Name = "Length/Depth - Unit of Measurement")]
            public string MeasurementUnit { get; set; }
        }

        private async Task LoadAsync(PopNGoUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var distanceUnit = user.DistanceUnit;
            var temperatureUnit = user.TemperatureUnit;
            var measurementUnit = user.MeasurementUnit;

            Input = new SettingModel
            {
                DistanceUnit = distanceUnit,
                TemperatureUnit = temperatureUnit,
                MeasurementUnit = measurementUnit
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var distanceUnit = user.DistanceUnit;
            var temperatureUnit = user.TemperatureUnit;
            var measurementUnit = user.MeasurementUnit;
            var inputUnit = Input.DistanceUnit;
            var inputTemp = Input.TemperatureUnit;
            var inputMeasure = Input.MeasurementUnit;

            bool changes = false;
            if (inputUnit != distanceUnit)
            {
                user.DistanceUnit = inputUnit;
                changes = true;
            }
            if (inputTemp != temperatureUnit)
            {
                user.TemperatureUnit = inputTemp;
                changes = true;
            }
            if (inputMeasure != measurementUnit)
            {
                user.MeasurementUnit = inputMeasure;
                changes = true;
            }

            // Only update the user if there are changes
            if (changes)
                await _userManager.UpdateAsync(user);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
