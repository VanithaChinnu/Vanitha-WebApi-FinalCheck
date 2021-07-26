using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MovieMvcApp.Controllers
{
    [Authorize(Roles = "Admin,Customer")]
    public class MovieController : Controller
    {
        static  HttpClient svc=new HttpClient();
        string baseAddress1;

        public MovieController()
        {  
            baseAddress1 = "http://localhost:2480/api/Movie/";
        }
        public async  Task UrlAdmin()
        {  
           var respone = await svc.GetAsync("http://localhost:2480/api/Auth?key=My name is James Bond&userId=1&userRole=Admin");
           string token = await respone.Content.ReadAsStringAsync();
           svc.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        public async Task UrlCustomer()
        {
            var respone = await svc.GetAsync("http://localhost:2480/api/Auth?key=My name is James Bond&userId=2&userRole=Customer");
            string token = await respone.Content.ReadAsStringAsync();
            svc.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        public async Task UrlAnonymousUser()
        {
            var respone = await svc.GetAsync("http://localhost:2480/api/Auth?key=My name is James Bond&userId=3&userRole=AnonymusUser");
            string token = await respone.Content.ReadAsStringAsync();
            svc.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        public async Task<ActionResult> Index()
        {
           await UrlAdmin();
           await UrlCustomer();
        //   await UrlAnonymousUser();
           return View(await svc.GetFromJsonAsync<List<Movie>>(baseAddress1));
        }
        [Route("Movie/Details/{title}")]
        public async Task<ActionResult> Details(string title)
        {
            try
            {
                await UrlCustomer();
                Movie movie = (await svc.GetFromJsonAsync<Movie>(title));
                return View(movie);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }

        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Movie movie)
        {
            try
            {
               await UrlAdmin();
                await svc.PostAsJsonAsync(baseAddress1, movie);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
        }
        [Route("Movie/Edit/{title}")]
        public async Task<ActionResult> Edit(string title)
        {
            try
            {
                await UrlAdmin();
                Movie movie = await svc.GetFromJsonAsync<Movie>(title);
                return View(movie);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Movie/Edit/{title}")]
        public async Task<ActionResult> Edit(string title, Movie movie)
        {
            try
            {
                await svc.PutAsJsonAsync(title, movie);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }
        [Route("Movie/Delete/{title}")]
        public async Task<ActionResult> Delete(string title)
        {
            try
            {
                await UrlAdmin();
                Movie movie = await svc.GetFromJsonAsync<Movie>(title);
                return View(movie);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Movie/Delete/{title}")]
        public async Task<ActionResult> Delete(string title, IFormCollection collection)
        {
            try
            {
                await svc.DeleteAsync(title);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }
        public async Task<ActionResult> MyFavourties()
        {
            try
            {
                await UrlCustomer();
                int count = 0;
                List<Movie> movies = new List<Movie>();
                List<Movie> mList = await svc.GetFromJsonAsync<List<Movie>>(baseAddress1);
                foreach (Movie m in mList)
                {
                    if (m.Favorite==true)
                    {
                        movies.Add(m);
                        count++;
                    }

                }
                if(count<=0)
                {
                    ViewBag.MovieCount = "no favourite movie set yet";
                }
                else
                {
                    ViewBag.MovieCount = count;

                }
                return View(movies);
                
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
        }
        public async Task<ActionResult> MoviesWhichAreActiveAndLaunchDate()
        {
            try
            {
                await UrlCustomer();
                List<Movie> movies = new List<Movie>();
                List<Movie> mList = await svc.GetFromJsonAsync<List<Movie>>(baseAddress1);
                foreach (Movie m in mList)
                {
                    if (m.Active == true && m.DateOfLaunch < DateTime.Now)
                    {
                        movies.Add(m);
                    }
                }
                return View(movies);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
        }
        public async Task<ActionResult> RemoveFavourite()
        {
            try
            {
                await UrlCustomer();
                Movie movie = await svc.GetFromJsonAsync<Movie>(baseAddress1);
                return View(movie);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveFavourite(string title,Movie movie)
        {
            try
            {
                await svc.PostAsJsonAsync(title,movie);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }
    }
}
