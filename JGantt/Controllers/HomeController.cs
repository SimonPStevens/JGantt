using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using JGantt.DataModels;
using JGantt.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace JGantt.Controllers
{
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
            var json = System.IO.File.ReadAllText(_env.WebRootPath + @"\plan.json");

            foreach (var d in Enumerable.Range(0, 21).Reverse())
            {
                json = json.Replace($"D+{d}", DateTime.Today.AddDays(d).ToString("yyyyMMdd"));
            }

            PlanModel model = BuildModelFromJson(json);

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
                var errorModel = new PlanModel(null, null);
                errorModel.Json = json;
                errorModel.JsonError = $"Error processing JSON {ex.ToString()}";

                return errorModel;
            }

            PlanModel model;
            try
            {
                modelTransform?.Invoke(jsonModel);

                List<Category> categoires = jsonModel.Categories?.Distinct().Select(c => new Category(c.Name, c.Colour)).ToList();

                List<Holiday> holidays = jsonModel.Holidays?.Distinct().Select(h => new Holiday(h.Date, h.Colour)).ToList();
                List<DateTime> holidayDates = holidays?.Select(h => h.Date).Distinct().ToList();

                List<Person> definedPeople = jsonModel.People?.Distinct().Select(p => new Person(p.Name, "", categoires?.FirstOrDefault(c => c.Name == p.Type))).ToList();

                List<Person> people = jsonModel.Plan?.Select(p => p.Person).Distinct().Select(personName =>
                {
                    var person = definedPeople?.FirstOrDefault(p => p.Name == personName);
                    return person ?? new Person(personName, "", null);
                }).ToList();

                List<Project> projects = new List<Project>();
                projects.AddRange(jsonModel.Plan.Select(p => p.Project).Distinct().Select(p =>
                {
                    var mileStones = jsonModel.Milestones?.Where(m => m.Project == p).Select(m => new Milestone(m.Title, m.Date));
                    var project = new Project(p, jsonModel.Projects?.FirstOrDefault(pro => pro.Name == p)?.Colour, mileStones);
                    return project;
                }));

                foreach (var project in projects)
                {
                    foreach (var milestone in project.Milestones)
                    {
                        milestone.Project = project;
                    }
                }

                var personProjects = new List<PersonProject>();
                foreach (var item in jsonModel.Plan)
                {
                    var endDate = item.EndDate(holidayDates);
                    personProjects.Add(new PersonProject(people.First(p => p.Name == item.Person), projects.First(p => p.Name == item.Project), item.StartDate, endDate));
                }

                model = new PlanModel(personProjects, holidays);
                model.Json = json;
            }
            catch (Exception ex)
            {
                model = new PlanModel(null, null);
                model.Json = json;
                model.JsonError = $"Error building view model {ex.ToString()}";
            }

            return model;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
