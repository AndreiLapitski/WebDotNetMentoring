using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NorthwindApp.DTO;

namespace NorthwindApp.ViewComponents
{
    public class Breadcrumbs : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(BreadcrumbsConfiguration dto)
        {
            string viewName = string.Empty;
            switch (dto.Mode)
            {
                case BreadcrumbsMode.List:
                    viewName = "_BreadcrumbsList";
                    break;

                case BreadcrumbsMode.Edit:
                case BreadcrumbsMode.Create:
                    viewName = "_Breadcrumbs";
                    break;
            }

            return View(viewName, dto);
        }
    }
}
