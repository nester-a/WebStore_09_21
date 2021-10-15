using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Linq;
using WebStore.Domain.Entities;
using WebStore.Infrastructure.Mapping;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Services.InCookies
{
    public class InCookiesCartService : ICartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProductData _productData;
        private readonly string _cartName;

        private Cart Cart
        {
            get
            {
                var context = _httpContextAccessor.HttpContext;
                var cookies = context.Response.Cookies;

                var cartCookies = context.Request.Cookies[_cartName];
                if (cartCookies is null)
                {
                    var cart = new Cart();
                    cookies.Append(_cartName, JsonConvert.SerializeObject(cart));
                    return cart;
                }
                ReplaceCart(cookies, cartCookies);
                return JsonConvert.DeserializeObject<Cart>(cartCookies);
            }
            set => ReplaceCart(_httpContextAccessor.HttpContext!.Response.Cookies, JsonConvert.SerializeObject(value));
        }

        public InCookiesCartService(IHttpContextAccessor httpContextAccessor, IProductData productData)
        {
            _httpContextAccessor = httpContextAccessor;
            _productData = productData;
            var user = httpContextAccessor.HttpContext!.User;
            var userName = user.Identity!.IsAuthenticated ? $"{user.Identity.Name}" : null;

            _cartName = $"GB.WebStore.Cart{userName}";
        }
        public void Add(int id)
        {
            var cart = Cart;

            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);
            if (item is null)
                cart.Items.Add(new CartItem { ProductId = id, Quantity = 1 });
            else
                item.Quantity++;

            Cart = cart;
        }
        public void Clear()
        {
            var cart = Cart;
            cart.Items.Clear();
            Cart = cart;

            //Cart = new();
        }
        public void Decrement(int id)
        {
            var cart = Cart;

            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);
            if (item is null) return;

            if (item.Quantity > 0)
                item.Quantity--;

            if (item.Quantity <= 0)
                cart.Items.Remove(item);

           Cart = cart;
        }
        public void Remove(int id)
        {
            var cart = Cart;

            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);
            if (item is null) return;

            cart.Items.Remove(item);

            Cart = cart;
        }
        private void ReplaceCart(IResponseCookies cookies, string cart)
        {
            cookies.Delete(_cartName);
            cookies.Append(_cartName, cart);
        }
        public CartViewModel GetViewModel()
        {
            var products = _productData.GetProducts(new()
            {
                Ids = Cart.Items.Select(i => i.ProductId).ToArray()
            });

            var productsViews = products.ToView().ToDictionary(p => p.Id);

            return new CartViewModel
            {
                Items = Cart.Items
                .Where(i => productsViews.ContainsKey(i.ProductId))
                .Select(i => (productsViews[i.ProductId], i.Quantity))
            };
        }
    }
}
