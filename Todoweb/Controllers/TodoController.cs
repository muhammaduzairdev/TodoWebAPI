using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Todoweb.Models;

namespace Todoweb.Controllers
{
    public class TodoController : Controller
    {
        public async Task<IActionResult> Home()
        {
            List<Todo> AllTodos = new List<Todo>();
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync("https://localhost:7152/api/Todo");
                if (response.IsSuccessStatusCode)
                {
                    string apiresponse = await response.Content.ReadAsStringAsync();
                    AllTodos = JsonConvert.DeserializeObject<List<Todo>>(apiresponse)!;
                }
            }
            return View(AllTodos);
        }
        public async Task<IActionResult> Detail(string Id)
        {
            Todo data = new Todo();
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"https://localhost:7152/api/Todo/{Id}");
                if (response.IsSuccessStatusCode)
                {
                    string apiresponse = await response.Content.ReadAsStringAsync();
                    data = JsonConvert.DeserializeObject<Todo>(apiresponse)!;
                }
            }
            return View(data);
        }
        [HttpGet]
        public IActionResult AddTodo()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddTodo(TodoDTO todoDTO)
        {
            Todo data = new Todo();
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(todoDTO), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("https://localhost:7152/api/Todo", content);
                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    string apiresponse = await response.Content.ReadAsStringAsync();
                    return RedirectToAction("Home" , "Todo");
                }
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string Id)
        {
            using (var httpClient = new HttpClient())
            {
                TodoDTO data = new TodoDTO();
                var response = await httpClient.GetAsync($"https://localhost:7152/api/Todo/{Id}");
                if (response.IsSuccessStatusCode)
                {
                    string apiresponse = await response.Content.ReadAsStringAsync();
                    data = JsonConvert.DeserializeObject<TodoDTO>(apiresponse)!;
                    return View(data);
                }
                else
                {
                    ViewBag.ErrorMsg = "Todo Not Found";
                    return View();
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(TodoDTO todoDTO)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(todoDTO), Encoding.UTF8, "application/json");
                var response = await httpClient.PutAsync($"https://localhost:7152/api/Todo/{todoDTO.Id}", content);
                if (response.IsSuccessStatusCode)
                {
                    string apiresponse = await response.Content.ReadAsStringAsync();
                    TempData["UpdateMsg"] = "Todo Updated Successfully";
                    return RedirectToAction(nameof(Home));
                }
                else
                {
                    ViewBag.Message = "Something Went Wrong";
                    return View();
                }
            }
        }
        public async Task<IActionResult> Delete(string Id)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.DeleteAsync($"https://localhost:7152/api/Todo/{Id}");
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Home));
                }
            } 
            return View(nameof(Home));
        }
    }
}
