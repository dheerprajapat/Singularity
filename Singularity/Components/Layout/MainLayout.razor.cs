﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorBindGen;
using Microsoft.AspNetCore.Components.Routing;
using Singularity.Contracts;

namespace Singularity.Components.Layout;

public partial class MainLayout:IDisposable
{
    internal static MainLayout Current;
    internal static bool UserAuthStateRead = false;
    internal static IUser? User;
    protected override async Task OnInitializedAsync()
    {
        Current = this;
        await BindGen.InitAsync(Runtime);
        await base.OnInitializedAsync();
        await AuthService.WatchAuthStateAsync();
        AuthService.OnAuthStateChanged += OnAuthStateChanged;
    }

    internal void Refresh()
    {
        StateHasChanged();
    }


    private void OnAuthStateChanged(object? sender, IUser? user)
    {
        UserAuthStateRead = true;
        User = user;
        StateHasChanged();

        if (user == null)
            NavManager.NavigateTo("/login");
    }

    public void Dispose()
    {
        AuthService.OnAuthStateChanged -= OnAuthStateChanged;
    }

}
