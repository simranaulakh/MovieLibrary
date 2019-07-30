using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MovieLibrary.Models;
using Microsoft.AspNetCore.Http;
using MovieLibrary.DAL;
using Microsoft.EntityFrameworkCore;
using MovieLibrary.Models;

namespace MovieLibrary.Controllers
{
    public class HomeController : Controller
    {
        public List<string[]> display = new List<string[]> { };
        public List<string> stringList = new List<string> { };
        public List<Movie> list = new List<Movie>();
        public readonly mySQLdbContext _context;

        public string[] SavedData;

        public HomeController(mySQLdbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
        
            return View();
        }

        //Create Method [POST]
        [HttpPost]
        public ActionResult Create(IFormCollection formCollection)
        {
            string id = Request.Form["id"];
            string title = Request.Form["title"];
            string lang = Request.Form["lang"];
            string category = Request.Form["category"];

            try
            {
                Movie movie = new Movie();
                movie.MovieID = id;
                movie.MovieTitle = title;
                movie.MovieLanguage = lang;
                movie.MovieCategory = category;
                _context.Add(movie);
                _context.SaveChanges();

            }catch(Exception e)
            {
                /*
                 * Error handling
                 */
                SavedData = getDataFromLocalDB();
                String data = id + ',' + title + ',' + lang + ',' + category;
                saveToLocalDB(data);
            }

            return View();
           
       }

        // Read Method [GET]
        [HttpGet]
        public ActionResult Read()
        {
            try
            {
                list = _context.Movies.ToList();
                var mID = _context.Movies.Include(e=> e.MovieID);
                var mTitle = _context.Movies.Include(e => e.MovieTitle);
                var mLang = _context.Movies.Include(e => e.MovieLanguage);
                var mCat = _context.Movies.Include(e => e.MovieCategory);
        }
            catch (Exception ex)
            {
                SavedData = getDataFromLocalDB();
                foreach (var line in SavedData)
                {
                    string[] localData = new string[4];
                    localData = line.Split(",");
                    display.Add(localData);
                }
                ViewData["Display"] = display;
            }

            return View();
        }

        // Delete Method [POST]
        [HttpPost]
        public ActionResult Delete(IFormCollection formCollection)
        {

            string id = Request.Form["id"];
            
            try
            {

                Movie movie = _context.Movies.Where(e => e.MovieID == id).FirstOrDefault();
                _context.Movies.Remove(movie);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                FindEntryByID(id);
            }

            return View();

        }

        // Update Method
        [HttpPost]
        public ActionResult Update(IFormCollection formCollection)
        {
            string id = Request.Form["id"];
            string title = Request.Form["title"];
            string lang = Request.Form["lang"];
            string category = Request.Form["category"];

            try
            {
                var movie = _context.Movies.SingleOrDefault(e => e.MovieID == id);

                if (movie != null)
                {
                    movie.MovieID = id;
                    movie.MovieTitle = title;
                    movie.MovieCategory = lang;
                    movie.MovieLanguage = category;
                    _context.SaveChanges();
                }

            }
            catch (Exception)
            {
                FindEntryByID(id);
                String data = id + ',' + title + ',' + lang + ',' + category;
                saveToLocalDB(data);
            }
         

            return View();

        }

        private string[] getDataFromLocalDB()
        {
            return System.IO.File.ReadAllLines("movielibrary.dat");
        }

        private void saveToLocalDB(String Data)
        {
            System.IO.File.AppendAllText("movielibrary.dat", Data + Environment.NewLine);
        }

        private void FindEntryByID(string id)
        {
            SavedData = getDataFromLocalDB();
            foreach (var line in SavedData)
            {
                stringList.Add(line.Split(",")[0]);
            }

            int counter = 0;
            foreach (string j in stringList)
            {
                if (j == id)
                {
                    break;
                }
                counter++;
            }
            stringList = SavedData.ToList();
            stringList.RemoveAt(counter);
            System.IO.File.WriteAllLines("movielibrary.dat", stringList);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
