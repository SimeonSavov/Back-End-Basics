using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MVCIntroDemo.Models.Product;
using System.Text;
using System.Text.Json;

namespace MVCIntroDemo.Controllers
{
    public class ProductController : Controller
    {
        private List<ProductViewModel> _products = new List<ProductViewModel>
        {
            new ProductViewModel()
            {
                Id = 1,
                Name = "Cheese",
                Price = 7.00m
            },
            new ProductViewModel()
            {
                Id = 2,
                Name = "Ham",
                Price = 5.50m
            },
            new ProductViewModel()
            {
                Id = 3,
                Name = "Bread",
                Price = 1.50m
            }
        };

        [ActionName("My-Products")]
        public IActionResult All(string keyword)
        {
            if (keyword != null)
            {
                var products = _products.Where(p => p.Name.ToLower().Contains(keyword.ToLower()));

                return View("All", products);
            }

            return View("All", _products);
        }

        public IActionResult ById(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public IActionResult AllAsJson()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            return Json(_products, options);
        }

        public IActionResult AllAsText()
        {
            var text = string.Empty;
            foreach (var item in _products)
            {
                text += $"Product {item.Id}: {item.Name} - {item.Price} lv. {Environment.NewLine}";
            }

            return Content(text);
        }

        public IActionResult AllAsTextFile()
        {
            var text = string.Empty;
            foreach (var item in _products)
            {
                text += $"Product: {item.Id}: {item.Name} - {item.Price:f2} lv. {Environment.NewLine}";
            }

            Response.Headers.Add(HeaderNames.ContentDisposition, @"attachment; filename=products.txt");

            return File(Encoding.UTF8.GetBytes(text.TrimEnd()), "text/plain");
        }

	}
}
