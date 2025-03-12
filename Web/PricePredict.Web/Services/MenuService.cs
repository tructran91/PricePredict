using PricePredict.Web.Enums;
using PricePredict.Web.Models.Application;

namespace PricePredict.Web.Services
{
    public interface IMenuService
    {
        List<MenuItem> GetMenuItems(PageType pageType);

        List<MenuItem> GetHeaderMenu();

        List<MenuItem> SetActiveMenuItems(List<MenuItem> menuItems, string currentUrl);
    }

    public class MenuService : IMenuService
    {
        public List<MenuItem> GetMenuItems(PageType pageType)
        {
            return pageType switch
            {
                PageType.Site => GetIndicatorsMenu(),
                PageType.Catalog => GetCatalogMenu(),
                _ => new List<MenuItem>()
            };
        }

        public List<MenuItem> GetHeaderMenu()
        {
            return new List<MenuItem>
            {
                GetIndicatorsMenu().First(),
                GetCatalogMenu().First()
            };
        }

        public List<MenuItem> SetActiveMenuItems(List<MenuItem> menuItems, string currentUrl)
        {
            foreach (var item in menuItems)
            {
                item.IsActive = IsUrlMatch(item.Url, currentUrl);

                if (item.Children != null && item.Children.Any())
                {
                    foreach (var childItem in item.Children)
                    {
                        childItem.IsActive = IsUrlMatch(childItem.Url, currentUrl);
                        if (childItem.IsActive)
                        {
                            item.IsActive = true;
                        }
                    }
                }
            }

            return menuItems;
        }

        private bool IsUrlMatch(string menuUrl, string currentUrl)
        {
            var currentUrlWithoutParams = currentUrl.Split('?')[0];

            menuUrl = menuUrl.Trim('/').ToLower();
            currentUrlWithoutParams = currentUrlWithoutParams.Trim('/').ToLower();

            if (menuUrl == currentUrlWithoutParams) return true;

            return currentUrlWithoutParams.StartsWith(menuUrl + "/");
        }

        private List<MenuItem> GetIndicatorsMenu()
        {
            return new List<MenuItem>
            {
                new MenuItem
                {
                    SpecialMenu = @"
                        <li class=""nav-small-cap"">
                            <i class=""ti ti-dots nav-small-cap-icon fs-4""></i>
                            <span class=""hide-menu"">Site</span>
                        </li>",
                    Url = string.Empty,
                    Title = "Indicator",
                    Children = new List<MenuItem>
                    {
                        new MenuItem
                        {
                            Title = "EMA",
                            Icon = "ti ti-activity-heartbeat",
                            Url = "/indicator/ema",
                        },
                        new MenuItem
                        {
                            Title = "RSI",
                            Icon = "ti ti-layout-kanban",
                            Url = "/indicator/rsi",
                        }
                    }
                }
            };
        }

        private List<MenuItem> GetCatalogMenu()
        {
            return new List<MenuItem>
            {
                new MenuItem
                {
                    SpecialMenu = @"
                        <li class=""nav-small-cap"">
                            <i class=""ti ti-dots nav-small-cap-icon fs-4""></i>
                            <span class=""hide-menu"">Catalog</span>
                        </li>",
                    Title = "Catalog",
                    Url = string.Empty,
                    Children = new List<MenuItem>
                    {
                        new MenuItem
                        {
                            Title = "Categories",
                            Icon = "ti ti-timeline-event",
                            Url = "/categories",
                        },
                        new MenuItem
                        {
                            Title = "Brands",
                            Icon = "ti ti-badge",
                            Url = "/brands",
                        }
                    }
                },
            };
        }
    }
}
