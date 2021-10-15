using Microsoft.AspNetCore.Http;
using System;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Services.InCookies
{
    public class InCookiesCartService : ICartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProductData _productData;
        private readonly string _cartName;

        public InCookiesCartService(IHttpContextAccessor httpContextAccessor, IProductData productData)
        {
            _httpContextAccessor = httpContextAccessor;
            _productData = productData;
            var user = httpContextAccessor.HttpContext!.User;
            var userName = user.Identity!.IsAuthenticated ? $"{user.Identity.Name}" : null;

            _cartName = $"WebStore.Cart{userName}";
        }
        public void Add(int id)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void Decrement(int id)
        {
            throw new NotImplementedException();
        }

        public CartViewModel GetViewModel()
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}
