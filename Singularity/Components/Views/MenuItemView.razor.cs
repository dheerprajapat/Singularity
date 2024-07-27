using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Singularity.Components.Views;

public partial class MenuItemView
{
    [Parameter]
    public string? Title { get; set; }
    [Parameter]
    public string? Description { get; set; }

    [Parameter]
    public string? Icon { get; set; }

    [Parameter]
    public EventCallback Clicked { get; set; }
    private string GetStyle(bool dark = false)
    {
        if (dark)
            return "rgba(7, 17, 27, 0.6)";
        return "rgba(7, 17, 27, 0.2)";
    }
}
