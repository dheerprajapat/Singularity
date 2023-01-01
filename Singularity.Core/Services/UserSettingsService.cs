using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Singularity.Core.Contracts.Services;
using Singularity.Core.Models;

namespace Singularity.Core.Services;
public class UserSettingsService : IUserSettingsService
{
    private UserSettings settings;
    public UserSettings CurrentSetting
    {
        get
        {
            if (settings != null)
                return settings;
             settings=Read();
            return settings;
        }
    }
    public string SettingsPath
    {
        get
        {
            if (OperatingSystem.IsWindows())
            {
                return Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                    , "Singularity");
            }
            throw new NotImplementedException();
        }
    }
    private const string FileName = "settings.json";
    private readonly IFileService FileService = new FileService();

    public UserSettings Read()
    {
        UserSettings? settings = null;
        lock (lockObj)
        {
            settings = FileService.Read<UserSettings>(SettingsPath, FileName);
        }
        if (settings == null)
            return new UserSettings();
        return settings;
    }
    private static readonly object lockObj = new object();
    public void Write(UserSettings settings)
    {
        lock (lockObj)
        {
            FileService.Save(SettingsPath, FileName, settings);
        }
    }
}
