﻿using ECommerce.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerce.Models;
using ECommerce.Models.Interfaces;
using System.ComponentModel;
using System.Configuration;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using ECommerce.Models.Services;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IProduct _Product;
        private readonly IConfiguration _configration;
        private readonly IProductsCart _productsCart;
        private readonly IUser _User;
        private readonly ICart _cart;


        public ProductsController(ApplicationDbContext context, IProduct Product, IConfiguration configration,IProductsCart productsCart, IUser user,ICart cart)
        {
            _context = context;
            _Product = Product;
            _configration = configration;
            _productsCart = productsCart;
            _User = user;
            _cart = cart;

        }


        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId)
        {
            var user = await _User.GetUser(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Main"); // Redirect to login page if the user is not authenticated
            }

            var product = await _Product.GetProductById(productId);

            if (product == null)
            {
                return NotFound(); // Handle the case where the product with the specified ID is not found
            }

            // Check if the user has an existing cart or create a new one
            var cart = await _cart.GetOrCreateCartAsync(user.Id);

            // Add the product to the cart
            var quantity = 1; // You can adjust the quantity as needed
            await _productsCart.AddProductToCartAsync(cart.Id, productId, quantity);

            return RedirectToAction("Index", "Main"); // Redirect to the cart view or any other desired page
        }




        public async Task<IActionResult> FilterProducts(string filter)
        {
            IQueryable<Product> query = _context.products;

            switch (filter)
            {
                case "HighToLowPrice":
                    query = query.OrderByDescending(p => p.Price);
                    break;

                case "LowToHighPrice":
                    query = query.OrderBy(p => p.Price);
                    break;

                case "OrderByAlphaBetAsend":
                    query = query.OrderBy(p => p.ProductName);
                    break;

                case "OrderByAlphaBetDesend":
                    query = query.OrderByDescending(p => p.ProductName);
                    break;

                default:
                    break;
            }

            var products = await query.ToListAsync();
            return View(products);
        }


        [HttpGet]
        public async Task<IActionResult> ViewAllProducts()
        {
            return View(await _context.products.ToListAsync());
        }
        [HttpPost]
        public  IActionResult ViewAllProducts(string productname)
        {
            
            HttpContext.Session.SetString("productname", productname);
            return RedirectToAction("Rows");
        }


        [HttpGet]
        public async Task<IActionResult> Rows(string productname)
        {
            var rows = _context.products.AsQueryable();

            if (!string.IsNullOrEmpty(productname))
            {
                rows = rows.Where(x => x.ProductName.Contains(productname));
            }

            return View(await rows.ToListAsync());
        }



        // GET: BookModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.products
                .Include(p => p.productsCategories) // Include the related categories
                .ThenInclude(pc => pc.category)      // Include the category details
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }







        // GET: BookModels/Create
        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, IFormFile file)
        {
            if (ModelState.IsValid && file != null)
            {
                // Upload the image to Azure Blob Storage and get the URI
                await _Product.GetFile(file, product);

                // Save the product to the database
                await _Product.Create(product);

                return RedirectToAction("ViewAllProducts", "Products");
            }

            return View(product);
        }




        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.products == null)
            {
                return NotFound();
            }

            var product = await _context.products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }



        // POST: BookModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductName,ImageUri,Price,Description")] Product product, IFormFile file)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _Product.GetFile(file, product);
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("ViewAllProducts", "Products");
            }
            return RedirectToAction("ViewAllProducts", "Products");
        }

        // GET: BookModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookModel = await _context.products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookModel == null)
            {
                return NotFound();
            }

            return View(bookModel);
        }

        // POST: BookModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.products.FindAsync(id);
            _context.products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("ViewAllProducts", "Products");
        }

        private bool ProductExists(int id)
        {
            return _context.products.Any(e => e.Id == id);
        }
        [HttpGet]
        public IActionResult AddCategoryToProduct(int ProductId)
        {
            ProductsCategory categoryProduct = new ProductsCategory()
            {
                ProductId = ProductId
            };
            ViewBag.Categories = _context.categories.ToList();
            return View(categoryProduct);

        }

        [HttpPost]
        public async Task<IActionResult> AddCategoryToProduct(ProductsCategory categoryProduct)
        {
            if (ModelState.IsValid)
            {
                await _Product.AddCategoryToProduct(categoryProduct.CategoryId, categoryProduct.ProductId);
                

                return RedirectToAction("Index", "Main");
            }
            else
            {
                return RedirectToAction("ViewAllProducts", "Products");
            }

        }

        

    }
}
