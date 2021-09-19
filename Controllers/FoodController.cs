using FoodMvc.Data.Entity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FoodMvc.Controllers
{
    [Authorize]
    public class FoodController : Controller
    {
        public async Task<IActionResult> Index()
        {
            await LogTokenAndClaims();

            List<Food> list = new List<Food> 
            { 
                new Food { Name = "Apple", FoodType = "Orange", Color = "Green", Weight = "120gm"},
                new Food { Name = "Orange", FoodType = "Orange", Color = "Green", Weight = "120gm"},
                new Food { Name = "Grap", FoodType = "Orange", Color = "Green", Weight = "120gm"},
                new Food { Name = "Banana", FoodType = "Orange", Color = "Green", Weight = "120gm"},
                new Food { Name = "Coconut", FoodType = "Orange", Color = "Green", Weight = "120gm"},
                new Food { Name = "Nashpati", FoodType = "Orange", Color = "Green", Weight = "120gm"},
            };
            return View(list);
        }

        public async Task LogTokenAndClaims()
        {
            var identityToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);

            Debug.WriteLine($"Identity token: {identityToken}");

            //Debug.WriteLine($"Claim type: {Claim.ReferenceEquals} - Claim value: {Claim.Value}");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);

            return Ok();
        }
    }
}
