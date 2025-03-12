using PricePredict.Web.Models.Application;
using PricePredict.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace PricePredict.Web.Layout
{
    public partial class HeaderMenu
    {
        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        [Inject]
        private IMenuService MenuService { get; set; }

        public List<MenuItem> MenuItems { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            MenuItems = MenuService.GetHeaderMenu();
        }

        private void ToggleMenu(MenuItem item)
        {
            if (item.IsActive)
            {
                CloseAllMenus(new List<MenuItem> { item });
            }
            else
            {
                CloseAllMenus(MenuItems);
                item.IsActive = true;
            }
        }

        private void CloseAllMenus(List<MenuItem> items)
        {
            foreach (var item in items)
            {
                item.IsActive = false;
                if (item.HasChildren)
                {
                    CloseAllMenus(item.Children);
                }
            }
        }

        private async Task HandleSidebar()
        {
            await JSRuntime.InvokeVoidAsync("appInterop.toggleSidebarMenu");
        }
    }
}
