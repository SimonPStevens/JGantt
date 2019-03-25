using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using JGantt.Models;

namespace JGantt.Controllers
{
    public class JsonModel
    {
        public List<Project> Projects { get; set; }
        public List<JsonPersonProject> Plan { get; set; }
    }

    public class JsonPersonProject
    {
        private const string stringFormat = "yyyyMMdd";

        public string Person { get; set; }
        public string Project { get; set; }
        public string Start { get; set; }

        [JsonIgnore]
        public DateTime StartDate
        {
            get
            {
                return DateTime.ParseExact(this.Start, stringFormat, CultureInfo.CurrentCulture, DateTimeStyles.None);
            }
            //set
            //{
            //    this.Start = value.ToString(stringFormat);
            //}
        }

        public string End { get; set; }

        [JsonIgnore]
        public DateTime EndDate
        {
            get
            {
                if (DateTime.TryParseExact(this.End, "yyyyMMdd", CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime result))
                {
                    return result;
                }
                else if (this.DaysDuration.HasValue)
                {
                    return this.StartDate.AddDays(this.DaysDuration.Value);
                } else
                {
                    return this.StartDate;
                }
            }
            //set
            //{
            //    this.End = value.ToString(stringFormat);
            //}
        }

        public int? DaysDuration { get; set; }
    }

    public class SubmitModel
    {
        public string Json { get; set; }
    }

    public class HomeController : Controller
    {
        private IHostingEnvironment _env;

        public HomeController(IHostingEnvironment env)
        {
            _env = env;
        }
 
        [HttpPost]
        public IActionResult Index(SubmitModel submittedModel)
        {
            PlanModel model = BuildModelFromJson(submittedModel.Json);

            return View("Index", model);
        }

        public IActionResult Index()
        {
            //DateTime baselineDate = new DateTime(2019, 03, 26);
            var json = System.IO.File.ReadAllText(_env.WebRootPath + @"\plan.json");

            foreach (var d in Enumerable.Range(0, 21))
            {
                json = json.Replace($"D+{d}", DateTime.Today.AddDays(d).ToString("yyyyMMdd"));
            }

            PlanModel model = BuildModelFromJson(json);
            //, (jsonModel) => {
            //    foreach (var item in jsonModel.Plan)
            //    {
            //        item.StartDate = new DateTime(2019, 01, 01) + (item.StartDate - baselineDate);
            //        item.EndDate = new DateTime(2019, 01, 01) + (item.EndDate - baselineDate);
            //    }
            //});


            return View(model);
        }

        private static PlanModel BuildModelFromJson(string json, Action<JsonModel> modelTransform = null)
        {
            JsonModel jsonModel;
            try
            {
                jsonModel = JsonConvert.DeserializeObject<JsonModel>(json);
            }
            catch (Exception ex)
            {
                var errorModel = new PlanModel(null);
                errorModel.Json = json;
                errorModel.JsonError = $"Error processing JSON {ex.ToString()}";

                return errorModel;
            }

            modelTransform?.Invoke(jsonModel);

            List<Person> people = new List<Person>();
            people.AddRange(jsonModel.Plan.Select(p => p.Person).Distinct().Select(p => new Person(p)));

            List<Project> projects = new List<Project>();
            projects.AddRange(jsonModel.Plan.Select(p => p.Project).Distinct().Select(p => jsonModel.Projects.FirstOrDefault(pro => pro.Name == p) ?? new Project(p, "")));

            var personProjects = new List<PersonProject>();
            foreach (var item in jsonModel.Plan)
            {
                personProjects.Add(new PersonProject(people.First(p => p.Name == item.Person), projects.First(p => p.Name == item.Project), item.StartDate, item.EndDate));
            }

            var model = new PlanModel(personProjects);
            model.Json = json;

            return model;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
