// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using PopNGo.Areas.Identity.Data;
using PopNGo.DAL.Abstract;
using PopNGo.Data;
using PopNGo.Models;
using PopNGo.Services;

namespace PopNGo.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<PopNGoUser> _signInManager;
        private readonly UserManager<PopNGoUser> _userManager;
        private readonly IUserStore<PopNGoUser> _userStore;
        private readonly IUserEmailStore<PopNGoUser> _emailStore;
        private readonly IPasswordValidator<PopNGoUser> _passwordValidator;
        private readonly ILogger<RegisterModel> _logger;
        private readonly EmailSender _emailSender;
        private readonly PopNGoDB _popNGoDBContext;

        private readonly IBookmarkListRepository _bookmarkListRepository;
        private readonly IScheduledNotificationRepository _scheduledNotificationRepository;
        private readonly IAccountRecordRepository _accountRecordRepository;

        public RegisterModel(
            UserManager<PopNGoUser> userManager,
            IUserStore<PopNGoUser> userStore,
            IPasswordValidator<PopNGoUser> passwordValidator,
            SignInManager<PopNGoUser> signInManager,
            ILogger<RegisterModel> logger,
            EmailSender emailSender,
            PopNGoDB popNGoDBContext,
            IBookmarkListRepository bookmarkListRepository,
            IScheduledNotificationRepository scheduledNotificationRepository,
            IAccountRecordRepository accountRecordRepository)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _passwordValidator = passwordValidator;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _popNGoDBContext = popNGoDBContext;
            _bookmarkListRepository = bookmarkListRepository;
            _scheduledNotificationRepository = scheduledNotificationRepository;
            _accountRecordRepository = accountRecordRepository;

            if(_popNGoDBContext == null)
            {
                throw new ArgumentNullException(nameof(popNGoDBContext));
            }
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            /// <summary>
            ///    This property is used to specify a user name.
            ///    This property is required.
            /// </summary>
            [Required]
            [Display(Name = "Username")]
            public string UserName { get; set; }

            /// <summary>
            ///    This property is used to specify a user's first name for short display purposes.
            ///    This property is required.
            /// </summary>
            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            /// <summary>
            ///   This property is used to specify a user's last name for short display purposes.
            ///   This property is required.
            /// </summary>
            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/Identity/Account/Login");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            // Validate the password before proceeding
            var passwordValid = await _passwordValidator.ValidateAsync(_userManager, null, Input.Password);
            if (!passwordValid.Succeeded)
            {
                foreach (var error in passwordValid.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

                return Page();
            }

            // Add a sanity check so that you can't create a second account with the same email.
            var emailExists = await _emailStore.FindByEmailAsync(Input.Email.ToUpper(), CancellationToken.None);
            if (emailExists != null)
            {
                ModelState.AddModelError("Email", "An incorrect email has been entered. Please try again.");

                return Page();
            }

            // Alright, we made it past the initial sanity checks. Were there any other errors?
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                user.FirstName = Input.FirstName;
                user.LastName = Input.LastName;
                user.NotificationEmail = Input.Email;
                user.DistanceUnit = "miles";
                user.TemperatureUnit = "f";
                user.MeasurementUnit = "inches";

                Console.WriteLine("User: " + user.FirstName + " " + user.Id);
                await _userStore.SetUserNameAsync(user, Input.UserName, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);
                await _userManager.AddToRoleAsync(user, "User");

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    await _userManager.AddToRoleAsync(user, "User");
                    
                    PgUser newUser = new()
                    {
                        AspnetuserId = user.Id
                    };
                    _popNGoDBContext.PgUsers.Add(newUser);
                    await _popNGoDBContext.SaveChangesAsync();

                    _bookmarkListRepository.AddBookmarkList(newUser.Id, "Favorites");
                    await _scheduledNotificationRepository.AddScheduledNotification(newUser.Id, DateTime.Now.AtMidnight().AddDays(1), "Upcoming Events");
                    _accountRecordRepository.AdjustBalance(DateTime.Now.AtMidnight(), "created", "increase");

                    // Set up the email confirmation
                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    // Send the email confirmation to the user's email (no checking if that exists!)
                    await _emailSender.SendEmailAsync(
                        Input.Email,
                        "Confirm your email",
                        new Dictionary<string, string>
                        {
                            { "template", "confirmation" },
                            { "confirmationURL", callbackUrl }
                        }
                    );

                    // If the user is required to confirm their email, redirect them to the confirmation page.
                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl, userCreated = true });
                    }
                    else
                    {
                        return RedirectToPage("Login", new { userCreated = true});
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private PopNGoUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<PopNGoUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(PopNGoUser)}'. " +
                    $"Ensure that '{nameof(PopNGoUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<PopNGoUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<PopNGoUser>)_userStore;
        }
    }
}
