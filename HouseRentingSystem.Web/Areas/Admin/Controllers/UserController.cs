using HouseRentingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentingSystem.Web.Areas.Admin.Controllers
{
    public class UserController : BaseAdminController
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task<IActionResult> All()
        {
            var allUsersModel = await userService.AllUsersAsync();

            return View(allUsersModel);
        }
    }
}
