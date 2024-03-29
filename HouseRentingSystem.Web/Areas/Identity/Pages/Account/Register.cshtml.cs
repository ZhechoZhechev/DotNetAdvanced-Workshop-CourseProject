﻿#nullable disable
namespace HouseRentingSystem.Web.Areas.Identity.Pages.Account;

using System.ComponentModel.DataAnnotations;

using Griesoft.AspNetCore.ReCaptcha;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Mvc.RazorPages;

using HouseRentingSystem.Data.Models;
using static HouseRentingSystem.Common.EntityValidationConstants.UserConstants;
using static HouseRentingSystem.Common.GeneralApplicationConstants;

[AllowAnonymous]
[ValidateRecaptcha(Action = "submit",
    ValidationFailedAction = ValidationFailedAction.ContinueRequest)]
public class RegisterModel : PageModel
{
    private readonly SignInManager<ApplicationUser> signInManager;
    private readonly UserManager<ApplicationUser> userManager;
    private readonly IMemoryCache memoryCache;

    public RegisterModel(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IMemoryCache memoryCache)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;

        this.memoryCache = memoryCache;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public string ReturnUrl { get; set; }

    public class InputModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Fist Name")]
        [StringLength(FirstNameMaxLength, MinimumLength = FirstNameMinLength)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(LastNameMaxLength, MinimumLength = LastNameMinLength)]
        public string LastName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }


    public async Task OnGetAsync(string returnUrl = null)
    {
        ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser()
            {
                UserName = Input.Email,
                Email = Input.Email,
                FirstName = Input.FirstName,
                LastName = Input.LastName
            };

            TempData["FirstName"] = Input.FirstName;
            TempData["LastName"] = Input.LastName;

            var result = await userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false);
                memoryCache.Remove(UsersCacheKey);

                return LocalRedirect(returnUrl);
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        // If we got this far, something failed, redisplay form
        return Page();
    }
}
