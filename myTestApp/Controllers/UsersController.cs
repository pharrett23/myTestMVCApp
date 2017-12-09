﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myTestApp.Models;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace myTestApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly MyTestAppContext _context;

        public UsersController(MyTestAppContext context)
        {
            _context = context;
        }

        public IActionResult getUser()
        {
            var usersName = from User in _context.User
                            select User;

            var myUser = new User();
            myUser.name = usersName.ToString();

            return View(myUser);
        }

        // POST: Create User
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("name, username, userID, isAdmin, password")] User user)
        {
            if (ModelState.IsValid)
            {
                byte[] salt = new byte[] { 1, 2, 3, 4, 5, 6, 7 };
                //using (var rng = RandomNumberGenerator.Create())
                //{
                //    rng.GetBytes(salt);
                //}
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: user.password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8));
                _context.Add(new User
                {
                    name = user.name,
                    username = user.username,
                    password = hashed
                });
                await _context.SaveChangesAsync();
                return RedirectToAction("myTestView", "Home");
            }
            return RedirectToAction("myTestView", "Home");
        }

        // POST: Log in user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogInUser([Bind("name, username, userID, isAdmin, password")] User user)
        {
            if (ModelState.IsValid)
            {
                var logInUserPassword = from myUser in _context.User
                                                               where myUser.username == user.username
                                                           select myUser.password;
                
                byte[] salt = new byte[] { 1, 2, 3, 4, 5, 6, 7 };
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: user.password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8));
                
                if(logInUserPassword.First() == hashed){
                    return RedirectToAction("Index", "Home");
                }else {
                    return RedirectToAction("myTestView", "Home");
                }
            }
            return RedirectToAction("myTestView", "Home");
        }
    }
}
