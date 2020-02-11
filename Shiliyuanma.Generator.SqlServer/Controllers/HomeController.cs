using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.Extensions.Hosting;
using Shiliyuanma.Generator.SqlServer.Models;

namespace Shiliyuanma.Generator.SqlServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostEnvironment _env;
        private IReverseEngineerScaffolder _engineer;

        public HomeController(IHostEnvironment env, IReverseEngineerScaffolder engineer)
        {
            _env = env;
            _engineer = engineer;
        }

        public IActionResult Index()
        {
            return View(new SqlViewModel());
        }

        [HttpPost]
        public IActionResult Index(SqlViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ErrorMsg = string.Join("/", ModelState
                    .Where(q => q.Value.Errors != null && q.Value.Errors.Count > 0)
                    .SelectMany(q => q.Value.Errors).Select(q => q.ErrorMessage));
                return View(model);
            }

            //gen
            var tables = !string.IsNullOrEmpty(model.TableNames)
                ? model.TableNames.Trim().Split(new[] {",", " "}, StringSplitOptions.RemoveEmptyEntries).ToList()
                : null;
            var dir = Path.Combine(_env.ContentRootPath, "Data");
            var subDirName = $"{DateTime.Now.ToString("yyyyMMdd-HHmmss")}-{new Random().Next(100, 999)}";
            var subDir = Path.Combine(dir, subDirName);
            var scaffoldModel = _engineer.ScaffoldModel(model.ConnectionString.Trim(),
                new DatabaseModelFactoryOptions(tables, null),
                new ModelReverseEngineerOptions(),
                new ModelCodeGenerationOptions
                {
                    ConnectionString = model.ConnectionString.Trim(),
                    ContextName = model.DbContextName.Trim(),
                    ModelNamespace = model.NamespaceName.Trim(),
                    UseDataAnnotations = model.UseDataAnnotations
                });
            var files = _engineer.Save(scaffoldModel, subDir, true);

            //to zip
            var zipFileName = subDirName + ".zip";
            var zipPath = Path.Combine(dir, zipFileName);
            ZipFile.CreateFromDirectory(subDir, zipPath);
            var data = System.IO.File.ReadAllBytes(zipPath);
            Directory.Delete(subDir, true);
            System.IO.File.Delete(zipPath);

            return File(data, "application/x-zip-compressed", zipFileName);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}