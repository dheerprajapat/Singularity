using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Singularity.Managers;

public static class SystemManager
{
    internal static IServiceProvider? ServiceProvider { get; set; }
    public static T? GetService<T>()
    {
        if (ServiceProvider == null)
            return default;

        return ServiceProvider.GetService<T>();
    }
}
