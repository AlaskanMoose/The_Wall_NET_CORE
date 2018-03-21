using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheWall.Factories;
using TheWall.Models;

namespace TheWall.Controllers
{
    public class CommentsController : Controller
    {
        private readonly CommentFactory _commentFactory;
        private readonly MessageFactory _messageFactory;

        private readonly UserFactory _userFactory;

        public CommentsController(CommentFactory commentFactory, MessageFactory messageFactory, UserFactory userFactory)
        {
            _commentFactory = commentFactory;
            _messageFactory = messageFactory;
            _userFactory = userFactory;
        }

        [HttpPost]
        [Route("newcomment/{messageid}")]
        public IActionResult CreateComment(Comment model, int messageid)
        {
            if(!CheckLogin())
            {
                return RedirectToAction("RegisterPage", "Users");
            }
            model.user_id = (int)HttpContext.Session.GetInt32("CurrUserId");
            model.message_id = messageid; 
            TryValidateModel(model);
            if(ModelState.IsValid)
            {
                _commentFactory.Add(model);
                return RedirectToAction("Dash", "Messages");
            }
            List<Message> AllMessages = _messageFactory.GetAllMessages();
            ViewBag.CurrentUser = _userFactory.GetUserById((int)HttpContext.Session.GetInt32("CurrUserId"));
            ViewBag.Messages = AllMessages;
            return View("Dash", model);
        }

        private bool CheckLogin()
        {
            return (HttpContext.Session.GetInt32("CurrUserId") != null);
        }
    }
}