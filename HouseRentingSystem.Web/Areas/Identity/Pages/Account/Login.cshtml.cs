#nullable disable
namespace HouseRentingSystem.Web.Areas.Identity.Pages.Account;

using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using HouseRentingSystem.Data.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Griesoft.AspNetCore.ReCaptcha;

[AllowAnonymous]
[ValidateRecaptcha(Action = "submit",
    ValidationFailedAction = ValidationFailedAction.ContinueRequest)]
public class LoginModel : PageModel
{
    private readonly SignInManager<ApplicationUser> signInManager;

    public LoginModel(SignInManager<ApplicationUser> signInManager)
    {
        this.signInManager = signInManager;
    }

    [BindProperty]
    public InputModel Input { get; set; }


    public string ReturnUrl { get; set; }

    [TempData]
    public string ErrorMessage { get; set; }

    public class InputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public async Task OnGetAsync(string returnUrl = null)
    {
        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            ModelState.AddModelError(string.Empty, ErrorMessage);
        }

        returnUrl ??= Url.Content("~/");

        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        ReturnUrl = returnUrl;
    }


    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        var user = await signInManager.UserManager.FindByEmailAsync(Input.Email);

        if (ModelState.IsValid)
        {
            var isValid = await signInManager.UserManager.CheckPasswordAsync(user, Input.Password);
            if (isValid)
            {
                var customClaims = new[] {
                    new Claim("FirstName", user.FirstName),
                    new Claim("LastName", user.LastName)
                }; 
                await signInManager.SignInWithClaimsAsync(user, false, customClaims);

                return LocalRedirect(returnUrl);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }
        }

        // If we got this far, something failed, redisplay form
        return Page();
    }
}
