using Newtonsoft.Json;
using System.Configuration;
using System.IO;
using System.Net;
using System.Web.Mvc;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class FormController : Controller
    {
        public ITHealthCheckFormDataModel Model { get; set; }
        public ApplicationDbContext _ApplicationDbContextContext { get; set; }

        public ActionResult Index(ITHealthCheckFormDataModel Model)
        {
            if(Model.Category is null)
            {
                return View();
            }
            else
            {
                OnSubmit(Model);
                return View();
            }
        }

        public ActionResult OnSubmit(ITHealthCheckFormDataModel Model)
        {
            var path = ConfigurationManager.AppSettings["apiuri"];
            var username = ConfigurationManager.ConnectionStrings["username"].ConnectionString;
            var apikey = ConfigurationManager.ConnectionStrings["APIKey"].ConnectionString;
            WebRequest requestObj = WebRequest.Create(path);
            requestObj.Method = "POST";
            requestObj.ContentType = "application/json";
            requestObj.Credentials = new NetworkCredential(username, apikey);

            var modelData = JsonConvert.SerializeObject(Model);

            using (var streamWriter = new StreamWriter(requestObj.GetRequestStream()))
            {
                streamWriter.Write(modelData);
                streamWriter.Flush();
                streamWriter.Close();

                var httpResponse = (HttpWebResponse)requestObj.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var results = JsonConvert.DeserializeObject<ITHealthCheckDBModel>(result);
                    var context = new ApplicationDbContext();
                    context.dbModels.Add(results);
                    context.SaveChanges();
                }
            }
            return View();
        }

    }
            
}
