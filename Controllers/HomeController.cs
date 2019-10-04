using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dojodachi.Models;
using Microsoft.AspNetCore.Http;

namespace Dojodachi.Controllers
{
    public class HomeController : Controller
    {   
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            if(HttpContext.Session.GetInt32("fullness") == null) {
                HttpContext.Session.SetInt32("fullness", 20);
                HttpContext.Session.SetInt32("happiness", 20);
                HttpContext.Session.SetInt32("energy", 50);
                HttpContext.Session.SetInt32("meals", 3);
                HttpContext.Session.SetString("statusText", "Successfully raise your Dojodachi!");
            }
            int happiness = (int)HttpContext.Session.GetInt32("happiness");
            int fullness = (int)HttpContext.Session.GetInt32("fullness");
            int energy = (int)HttpContext.Session.GetInt32("energy");
            int meals = (int)HttpContext.Session.GetInt32("meals");
            ViewBag.statusText = HttpContext.Session.GetString("statusText");
            ViewBag.fullness = fullness;
            ViewBag.happiness = happiness;
            ViewBag.energy = energy;
            ViewBag.meals = meals;
            ViewBag.gameEnd = false;
            if(happiness <=0 || fullness <= 0) {
                ViewBag.gameEnd = true;
                ViewBag.statusText = "Your Dojodachi has ran away to find a new owner... :(";
            }
            if (fullness >= 100 && happiness >= 100 && energy >= 100) {
                ViewBag.statusText = "Congratulations!!! Your Dojodachi is successful and happy :)";
                ViewBag.gameEnd = true;
            }
            return View();
        }

        [HttpGet]
        [Route("/restart")]
        public IActionResult Restart() {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("/play")]
        public IActionResult Play() {
            if ((int)HttpContext.Session.GetInt32("energy") < 5) {
                HttpContext.Session.SetString("statusText", "Your Dojodachi is too tired!");
                return RedirectToAction("Index");
            }
            Random rand = new Random();
            int dislike = rand.Next(1,5);
            if (dislike == 1) {
                HttpContext.Session.SetString("statusText", "Your Dojodachi wasn't in the mood to play... Energy -5");
            }
            else {
                int happiness = rand.Next(5, 11);
                HttpContext.Session.SetInt32("happiness", (int)HttpContext.Session.GetInt32("happiness") + happiness);  
                HttpContext.Session.SetString("statusText", $"You played with your Dojodachi! Happiness +{happiness} Energy -5");
            }
            HttpContext.Session.SetInt32("energy", (int)HttpContext.Session.GetInt32("energy") - 5);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("/feed")]
        public IActionResult Feed() {
            if ((int)HttpContext.Session.GetInt32("meals") < 1) {
                HttpContext.Session.SetString("statusText", "You have no meals to feed!");
                return RedirectToAction("Index");
            }
            Random rand = new Random();
            int dislike = rand.Next(1,5);
            if (dislike == 1) {
                HttpContext.Session.SetString("statusText", "Your Dojodachi didn't like the food it was given! Meals -1");
            }
            else {
                
                int fullness = rand.Next(5, 11);
                HttpContext.Session.SetInt32("fullness", (int)HttpContext.Session.GetInt32("fullness") + fullness);
                HttpContext.Session.SetString("statusText", $"You fed your Dojodachi! Fullness +{fullness} Meals -1");
            }
            HttpContext.Session.SetInt32("meals", (int)HttpContext.Session.GetInt32("meals") - 1);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("/work")]
        public IActionResult Work() {
            if ((int)HttpContext.Session.GetInt32("energy") < 5) {
                HttpContext.Session.SetString("statusText", "Your Dojodachi is too tired!");
                return RedirectToAction("Index");
            }
            Random rand = new Random();
            int meals = rand.Next(1, 4);
            HttpContext.Session.SetInt32("energy", (int)HttpContext.Session.GetInt32("energy") - 5);
            HttpContext.Session.SetInt32("meals", (int)HttpContext.Session.GetInt32("meals") + meals);  
            HttpContext.Session.SetString("statusText", $"Your Dojodachi went to work! Meals +{meals} Energy -5");
            return RedirectToAction("Index");
        }

                [HttpGet]
        [Route("/sleep")]
        public IActionResult Sleep() {
            HttpContext.Session.SetInt32("energy", (int)HttpContext.Session.GetInt32("energy") + 15);
            HttpContext.Session.SetInt32("fullness", (int)HttpContext.Session.GetInt32("fullness") - 5);
            HttpContext.Session.SetInt32("happiness", (int)HttpContext.Session.GetInt32("happiness") - 5);
            HttpContext.Session.SetString("statusText", $"Your Dojodachi took a nap! Energy +15 Happiness -5 Fullness -5");
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
