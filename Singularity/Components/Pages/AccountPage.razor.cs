using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Singularity.Components.Layout;

namespace Singularity.Components.Pages;

public partial class AccountPage
{
    private async ValueTask Logout()
    {
        await AuthService.LogoutUserAsync();
        MainLayout.User = null;
        MainLayout.Current.Refresh();
        NavManager.NavigateTo("/login");
    }
}
