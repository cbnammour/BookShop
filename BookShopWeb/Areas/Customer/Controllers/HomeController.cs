using BookShop.DataAccess.Repository.IRepository;
using BookShop.Models;
using BookShop.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Security.Claims;

namespace BookShopWeb.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;

        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType");
            return View(productList);
        }

        public async Task<IActionResult> Details(int productId)
        {
            
            ShoppingCart cartObj = new()
            {
                Product = _unitOfWork.Product.GetFirsrOrDefault(u => u.Id == productId, includeProperties: "Category,CoverType"),
                ProductId = productId,
                Count = 1
            };
            cartObj.Product.Review = await getBookReviewsFromApi(cartObj.Product.ISBN);
            return View(cartObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCart.ApplicationUserId = claim.Value;

            ShoppingCart cart = _unitOfWork.ShoppingCart.GetFirsrOrDefault(u => u.ApplicationUserId == claim.Value && u.ProductId == shoppingCart.ProductId);

            if (cart == null)
            {
                _unitOfWork.ShoppingCart.Add(shoppingCart);
            }
            else
            {
                _unitOfWork.ShoppingCart.incrementCount(cart, shoppingCart.Count);
            }


            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<string> getBookReviewsFromApi(string isbn)
        {
            // Set the ISBN of the book you want to get reviews for
            //isbn = "0735619670";
            string review = "";

            // Set the parameters for the request
            var parameters = new Dictionary<string, string>
            {
                { "q", $"isbn:{isbn}" },  // The search query
                { "key", SD.API_KEY },  // Your API key
            };

            // Set the URL for the request
            var url = "https://www.googleapis.com/books/v1/volumes";

            // Make the request
            var client = new HttpClient();
            var response = await client.GetAsync(url + "?" + string.Join('&', parameters.Select(x => x.Key + "=" + x.Value)));

            Console.WriteLine(url + "?" + string.Join('&', parameters.Select(x => x.Key + "=" + x.Value)));

            string json = await response.Content.ReadAsStringAsync();

            if(json != null)
            {
                BookReview bookReview = JsonConvert.DeserializeObject<BookReview>(json);
                if(bookReview != null && bookReview.Items.Length > 0 )
                {
                    review = bookReview.Items[0].VolumeInfo.Description;
                }
            }

            return review;

            // Print the response
            //Console.WriteLine(await response.Content.ReadAsStringAsync());
        }

    }
}