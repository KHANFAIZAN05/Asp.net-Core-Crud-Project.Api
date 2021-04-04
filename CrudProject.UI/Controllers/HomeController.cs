using CrudProject.Api.Data;
using CrudProject.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CrudProject.UI.Controllers
{
    public class HomeController : Controller
    {

        private readonly string BaseUrl = "https://localhost:44351/";
        public HttpClient client { get; private set; }
        List<Joke> jokes = new();

        public HomeController()
        {
            client = new HttpClient();
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {

                HttpResponseMessage response = await client.GetAsync(BaseUrl + "api/jokes");

                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    jokes = JsonConvert.DeserializeObject<List<Joke>>(content);
                    return View(jokes);
                }

            }
            catch (Exception)
            {

                throw;
            }

            return View();
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Joke joke)
        {

            try
            {

                var jsondata = JsonConvert.SerializeObject(joke);
                var stringContent = new StringContent(jsondata, UnicodeEncoding.UTF8, "application/json"); // use MediaTypeNames.Application.Json in Core 3.0+ and Standard 2.1+
                HttpResponseMessage response = await client.PostAsync(BaseUrl + "api/jokes/", stringContent);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {

                throw;
            }

            return View();
        }

         [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Joke joke = new();
            try
            {
                HttpResponseMessage response = await client.GetAsync(BaseUrl + "api/jokes/" + id);


                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    joke = JsonConvert.DeserializeObject<Joke>(content);
                    return View(joke);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> DeleteConfirm(int id)
        {

            try
            {
                var employeeToDelete = await client.GetAsync(BaseUrl + "api/jokes/" + id);

                if (employeeToDelete == null)
                {
                    return NotFound($"Employee with Id = {id} not found");
                }
                HttpResponseMessage response = await client.DeleteAsync(BaseUrl + "api/jokes/" + id);
                if(response.IsSuccessStatusCode)
                    {
                    return RedirectToAction("Index");

                   }
               
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting data");
            }

            return View();

        }

        [HttpGet]
        public async Task< IActionResult> Edit (int id)
        {
            Joke joke = new();
            try
            {
                HttpResponseMessage response = await client.GetAsync(BaseUrl + "api/jokes/" + id);


                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    joke = JsonConvert.DeserializeObject<Joke>(content);
                    return View(joke);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id ,Joke joke)
        {
            try
            {
                var JokeToUpdate = await client.GetAsync(BaseUrl + "api/jokes/" + id);

                if (JokeToUpdate == null)
                {
                     return NotFound($"Employee with Id = {id} not found");
                }
                var content  = JsonConvert.SerializeObject(joke);
                var stringContent = new StringContent(content, UnicodeEncoding.UTF8, "application/json"); // use MediaTypeNames.Application.Json in Core 3.0+ and Standard 2.1+
                HttpResponseMessage response = await client.PutAsync(BaseUrl + "api/jokes/"+ id , stringContent);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");

                }

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting data");
            }

            return View("Edit");
        }

        public async Task<IActionResult> Search (string SearchString)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(BaseUrl + "api/jokes/SearchJokes/" + SearchString);
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    jokes = JsonConvert.DeserializeObject<List<Joke>>(content);
                    return View("Index",jokes);
                }

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting data");
            }

            return View("index");
        }
    }
}
