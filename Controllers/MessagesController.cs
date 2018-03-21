using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheWall.Factories;
using TheWall.Models;

namespace TheWall.Controllers
{
    public class MessagesController : Controller
    {
        private readonly MessageFactory _messageFactory;

        private readonly UserFactory _userFactory;

        public MessagesController(MessageFactory messageFactory, UserFactory userFactory)
        {
            _messageFactory = messageFactory;
            _userFactory = userFactory;
        }

        [HttpGet]
        [Route("dash")]
        public IActionResult Dash()
        {
            if(!CheckLogin())
            {
                return RedirectToAction("Index", "Users");
            }
            List<Message> AllMessages = _messageFactory.GetAllMessages();
            ViewBag.CurrentUser = _userFactory.GetUserById((int)HttpContext.Session.GetInt32("CurrUserId"));
            ViewBag.Messages = AllMessages;
            return View("Dash");
        }

        [HttpPost]
        [Route("createmessage")]
        public IActionResult CreateMessage(Message model)
        {
            if(!CheckLogin())
            {
                return RedirectToAction("Index", "Users");
            }
            if(ModelState.IsValid)
            {
                model.user_id = (int)HttpContext.Session.GetInt32("CurrUserId");
                _messageFactory.Add(model);
                return RedirectToAction("Dash");
            }
            List<Message> AllMessages = _messageFactory.GetAllMessages();
            ViewBag.CurrentUser = _userFactory.GetUserById((int)HttpContext.Session.GetInt32("CurrUserId"));
            ViewBag.Messages = AllMessages;
            return View("Dash", model);
        }

        [HttpPost]
        [Route("deletemessage/{messageid}")]
        public IActionResult DeleteMessage(int messageid)
        {
            if(!CheckLogin())
            {
                return RedirectToAction("Index", "Users");
            }
            Message CheckMessage = _messageFactory.GetMessageById(messageid);
            if(CheckMessage.user_id == (int)HttpContext.Session.GetInt32("CurrUserId") && CheckMessage.Deletable)
            {
                _messageFactory.DeleteMessage(messageid);
            }
            return RedirectToAction("Dash");
        }

        private bool CheckLogin()
        {
            return (HttpContext.Session.GetInt32("CurrUserId") != null);
        }
    }
    
}