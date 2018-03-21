using System.Collections.Generic;
using TheWall.Factories;
using TheWall.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace TheWall.Controllers
{
    public class UsersController : Controller{
        private readonly UserFactory _userFactory;
        public UsersController(UserFactory userFactory){
            _userFactory = userFactory;
        }
        [HttpGet]
        [Route("")]
        public IActionResult Index(){
            ViewBag.Count = 0;
            return View();
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(User model){
            if(ModelState.IsValid){
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                model.Password = Hasher.HashPassword(model, model.Password);
                _userFactory.Add(model);
                User CurrentUser = _userFactory.GetLatestUser();
                HttpContext.Session.SetInt32("CurrUserId", CurrentUser.UserId);
                return RedirectToAction("Dash", "Messages");
            }
            ViewBag.Count= 2;
            ViewBag.Errors= ModelState.Values;
            return View("Index", model);
        }
        [HttpPost]
        [Route("login")]
        public IActionResult Login(string Email, string Password){
            var user = _userFactory.GetUserByEmail(Email);
            if(user != null && Password != null){
                var Hasher = new PasswordHasher<User>();
                if(0 != Hasher.VerifyHashedPassword(user, user.Password, Password)){
                    HttpContext.Session.SetInt32("CurrUserId", user.UserId);
                    return RedirectToAction("Dash", "Messages");
                }else{
                    ViewBag.Count = 1;
                    ViewBag.Error = "Password is Incorrect";
                }
            }else{
                ViewBag.Count = 1;
                ViewBag.Error = "The User does not exist";
            }
            return View("Index");
        }
        [HttpGet]
        [Route("logout")]
        public IActionResult Logout(){
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
