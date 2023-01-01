using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Singularity.Core.Models;
public class UserSettings
{
    public MediaSettngs Media
    {
        get; set;
    }
}

public class MediaSettngs
{
    public int Volume
    {
        get; set;
    } = 50;
    public bool RepeatEnabled
    {
        get; set;
    } = true;

    public string? LastPlayedId
    {
        get; set;
    } = null;
}