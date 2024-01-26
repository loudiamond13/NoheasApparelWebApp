using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Authentication;
using static Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal.LoginModel;
using System.Drawing;
using Microsoft.AspNetCore.Authorization;

namespace NoheasApparel.Areas.Identity.Pages.Account
{
    
    public class AdminLoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        
        private readonly UserManager<IdentityUser> _userManager;
        

        public AdminLoginModel(SignInManager<IdentityUser> signInManager, ILogger<LoginModel> logger, UserManager<IdentityUser> userManager )
        {
            _signInManager = signInManager;
            _logger = logger;
           _userManager = userManager;
           
        }


        
        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }
        public string ErrorMessage { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;
        }

      
        public async Task GetTaskAsync(string? returnURL = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnURL ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnURL;
        }

        public async Task<IActionResult> OnPostAsync(string? returnURL = null) 
        {
            returnURL ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                

                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, false , lockoutOnFailure: false);

                

                if (result.Succeeded)
                {
                    //gets the user role/s.
                    var Role = await _userManager.GetRolesAsync((IdentityUser)User.Identities);
                    
                    if (Role.ToString().ToLower().Contains("admin")) { 

                    _logger.LogInformation("Admin logged in.");
                    return LocalRedirect(returnURL);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Unauthorized User.");
                  
                }
            }
            return Page();


        }
       
    } 
}
