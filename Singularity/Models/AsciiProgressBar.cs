using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Singularity.Models;
internal class AsciiProgressBar
{

    static readonly char DashLight = '─';
    static readonly char DashThick = '━';
    static readonly char CircleCenter = '⬤';
    public static string GetProgressAscii(int value, int max, int size = 15)
    {
        int thickDashCount = (int)(value * size * 1.0f / max);
        var result = "".PadLeft(thickDashCount, DashThick);
        result += CircleCenter;
        result += "".PadLeft(size - thickDashCount, DashLight);
        return result;
    }
}

