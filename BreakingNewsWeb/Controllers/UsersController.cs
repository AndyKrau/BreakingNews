using BreakingNewsWeb.Models.ViewModels;
using DBConnection.Models.Classes;
using DBConnection.Models.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection.Metadata.Ecma335;

namespace BreakingNewsWeb.Controllers
{
    
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApiDataConnectionContext _apiData;

        public UsersController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ApiDataConnectionContext apiData)
        {
            _userManager = userManager;
            _roleManager = roleManager;

            // получаем контекст APIData
            _apiData = apiData;
        }

        public async Task<IActionResult> Card()
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x=> x.UserName == User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [Authorize(Roles = "admin")]
        public IActionResult Users()
        {
            return View(_userManager.Users.ToList());
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Users");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            EditUserViewModel model = new EditUserViewModel { 
                                            Id = user.Id, 
                                            Email = user.Email,
                                            PhoneNumber = user.PhoneNumber,
                                            Country = user.Country,
                                            City = user.City,
                                            PostalCode = user.PostalCode,
                                            DateOfBirth = user.DateOfBirth,
                                            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if(user != null)
                {
                    user.Email = model.Email;
                    user.PhoneNumber = model.PhoneNumber;
                    user.PostalCode = model.PostalCode;
                    user.Country = model.Country;
                    user.City = model.City;
                    user.DateOfBirth = DateTime.SpecifyKind((DateTime)model.DateOfBirth, DateTimeKind.Utc);

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Users");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
        return View(model);
        }

        public async Task<IActionResult> ChangePassword(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ChangePasswordViewModel model = new ChangePasswordViewModel { Id = user.Id, Email = user.Email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    IdentityResult result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Users");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Пользователь не найден");
                }
            }
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Settings()
        {
            // находим первую строчку подключения в базе
            var apiSettings = await _apiData.ApiData.SingleOrDefaultAsync();
            if (apiSettings == null)
            {
                return NotFound();
            }

            // получаем текущую страну из базы для отображения и передаём название в новый экз. класса ChangeApiDataViewModel
            var currentContryId = await _apiData.Countries.FirstOrDefaultAsync(c => c.Id == apiSettings.CountryId);
            if (currentContryId != null)
            {
                ChangeApiDataViewModel model = new()
                {
                    ApiKey = apiSettings.ApiKey,
                    Url = apiSettings.Url,
                    CountryName = currentContryId.CountryName
                };
                return View(model);
            }
            return View();
        }


        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EditSettings()
        {
            // список стран для выпадающего списка в View
            List<Country> countries = _apiData.Countries.ToList();
            ViewBag.Countries = countries.Select(y => y.CountryName).ToList();

            // находим первую строчку подключения в базе
            var apiSettings = await _apiData.ApiData.SingleOrDefaultAsync();
            if (apiSettings == null)
            {
                return NotFound();
            }

            // получаем текущую страну из базы для отображения и передаём название в новый экз. класса ChangeApiDataViewModel
            var currentContryId = await _apiData.Countries.FirstOrDefaultAsync(c => c.Id == apiSettings.CountryId);
            if (currentContryId != null)
            {
                ChangeApiDataViewModel model = new()
                {
                    ApiKey = apiSettings.ApiKey,
                    Url = apiSettings.Url,
                    CountryName = currentContryId.CountryName 
                };
                return View(model);
            }
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EditSettings(ChangeApiDataViewModel data)
        {
            //bool hasCountry = await _apiData.Countries.AnyAsync(c => c.CountryName == data.CountryName);
            //if (hasCountry)
            //{
            var existCountry = _apiData.Countries.FirstOrDefault(x => x.CountryName == data.CountryName);
            ApiData apiSettings = await _apiData.ApiData.FirstAsync();

            if (apiSettings != null)
            {
                apiSettings.ApiKey = data.ApiKey;
                apiSettings.Url = data.Url;
                apiSettings.CountryId = existCountry.Id;

                await _apiData.SaveChangesAsync();
            }
        //}
            return RedirectToAction("Settings");
        }

    }
}

