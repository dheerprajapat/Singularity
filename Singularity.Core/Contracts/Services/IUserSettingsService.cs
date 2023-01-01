using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Singularity.Core.Models;

namespace Singularity.Core.Contracts.Services;
public interface IUserSettingsService
{
    public string SettingsPath
    {
        get;
    }
    public UserSettings CurrentSetting
    {
        get;
    }

    UserSettings Read();
    void Write(UserSettings settings);
}
