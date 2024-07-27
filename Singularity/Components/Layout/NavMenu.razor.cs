using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Routing;
using Singularity.Models;

namespace Singularity.Components.Layout;

public partial class NavMenu
{
    private readonly List<TabPage> Tabs = new List<TabPage>
    {
        new TabPage("./images/home.svg","/","Home"),
        new TabPage("./images/search.svg","/search","Search"),
        new TabPage("./images/heart.svg","/like","Favourite"),
        new TabPage("./images/playlist.svg","/playlist","Playlist"),
        new TabPage("./images/account.svg","/account","Account"),
    };

    protected override void OnInitialized()
    {
        base.OnInitialized();
        NavManager.LocationChanged += LocationChanged;
    }

    private void LocationChanged(object? sender, LocationChangedEventArgs e)
    {
        foreach (var page in Tabs)
        {
            page.IsActive = e.Location.EndsWith(page.Link);
        }
        StateHasChanged();
    }

    private string GetActiveClass(TabPage tab)
    {
        return tab.IsActive ? "active" : string.Empty;
    }
}
