using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class HomeController : Controller
    {
       [Authorize]
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var Role = GetRole();
            if (!string.IsNullOrEmpty(Role))
            {
                ViewBag.Role = Role;
            }    
            
            string str = Startup.URL + "apps";
            var authValue = new AuthenticationHeaderValue("Basic", Startup.User_Key);
            var httpClient = new HttpClient()
            {
                DefaultRequestHeaders = { Authorization = authValue }
            };
            var contentsTask = await httpClient.GetAsync(str);
            var jsonResponse = await contentsTask.Content.ReadAsStringAsync();
            var obj= JsonConvert.DeserializeObject<List<OneSignal>>(await contentsTask.Content.ReadAsStringAsync());
            return View(obj);
        }
        
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Edit(string Id)
        {
            string str = Startup.URL + "apps/" + Id;
            var authValue = new AuthenticationHeaderValue("Basic", Startup.User_Key);
         
            var httpClient = new HttpClient()
            {
                DefaultRequestHeaders = { Authorization = authValue }
               
            };
            var contentsTask = await httpClient.GetAsync(string.Format(str,Id));
            var jsonResponse = await contentsTask.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<OneSignal>(await contentsTask.Content.ReadAsStringAsync());
            return View(obj);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Details(string Id)
        {
            string str = Startup.URL + "apps/" + Id;
            var authValue = new AuthenticationHeaderValue("Basic", Startup.User_Key);
            
            var httpClient = new HttpClient()
            {
                DefaultRequestHeaders = { Authorization = authValue }
               
            };

            var contentsTask = await httpClient.GetAsync(str);
            var jsonResponse = await contentsTask.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<OneSignal>(await contentsTask.Content.ReadAsStringAsync());
            return View(obj);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Edit(OneSignal one)
        {
            string str = Startup.URL + "apps/" + one.id;
            var authValue = new AuthenticationHeaderValue("Basic", Startup.User_Key);

            var httpClient = new HttpClient()
            {
                DefaultRequestHeaders = { Authorization = authValue }

            };
            var json = new JavaScriptSerializer().Serialize(one);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var contentsTask = await httpClient.PutAsync(string.Format(str), httpContent);
            var jsonResponse = await contentsTask.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<OneSignal>(await contentsTask.Content.ReadAsStringAsync());
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Save(OneSignal one)
        {
            string str = Startup.URL + "apps";
            var authValue = new AuthenticationHeaderValue("Basic", Startup.User_Key);

            var httpClient = new HttpClient()
            {
                DefaultRequestHeaders = { Authorization = authValue }

            };
            var json = new JavaScriptSerializer().Serialize(one);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var contentsTask = await httpClient.PostAsync(string.Format(str), httpContent);
            var jsonResponse = await contentsTask.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<OneSignal>(await contentsTask.Content.ReadAsStringAsync());
            return RedirectToAction("Index");
        }
        public string GetRole()
        {
            string role = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s.Count >0)
                {
                    role=  s.FirstOrDefault();
                }
            }
            return role;
        }
    }

}