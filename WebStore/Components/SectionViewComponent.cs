using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Components
{
    public class SectionViewComponent :ViewComponent
    {
        private readonly IProductData _productData;

        public SectionViewComponent(IProductData productData)
        {
            _productData = productData;
        }
        public IViewComponentResult Invoke()
        {
            var sections = _productData.GetSections();

            var parent_sections = sections.Where(s => s.ParentId is null);

            var parent_sections_views = parent_sections
                .Select(s => new SectionViewModel()
                {
                    Id = s.Id,
                    Name = s.Name,
                    Order = s.Order,
                })
                .ToList();

            foreach (var parent_section in parent_sections_views)
            {
                var childs = sections.Where(s => s.Id == parent_section.Id);

                foreach (var child_section in childs)
                {
                    parent_section.ChildSections.Add(new SectionViewModel() 
                    { 
                        Id = child_section.Id,
                        Name = child_section.Name,
                        Order = child_section.Order,
                        Parent = parent_section,
                    });
                }
                parent_section.ChildSections.Sort((a, b) => Comparer<int>.Default.Compare(a.Order, b.Order));
            }
            parent_sections_views.Sort((a, b) => Comparer<int>.Default.Compare(a.Order, b.Order));
            return View(parent_sections_views);
        }
    }
}
