using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Singularity.Contracts;
using Singularity.Managers;

namespace Singularity.Data;

public abstract class SettingsBase<T>:ObservableObject where T: SettingsBase<T>
{
    private IDatabaseService? Database => SystemManager.GetService<IDatabaseService>();
    public static T Current => CurrentSettings[typeof(T)];

#nullable disable
    private static Dictionary<Type, T> CurrentSettings = new Dictionary<Type, T>()
    {
#pragma warning disable CS0612 // Type or member is obsolete
        [typeof(UserSettings)] = new UserSettings() as T,
        [typeof(PlaylistSettings)] = new PlaylistSettings() as T,
#pragma warning restore CS0612 // Type or member is obsolete
    };

#nullable restore

    public bool LoadedFromDb { get; protected set; }


    public async ValueTask LoadSettingsFromDb()
    {
        if (Database == null)
            return;

        var table = await Database.GetTableAsync(typeof(T).Name);

        if (table == null) return;

        var tableExists = await table.ExistsAsync();

        if(!tableExists)
        {
            await Database.CreateTableAsync(typeof(T).Name, Current);
            Current.LoadedFromDb = true;
            return;
        }

        var online = await table.ToAsync<T>();

        //online data might be corrupted
        if (online==null)
            throw new Exception("Can't load user settings from database");

        CurrentSettings[typeof(T)] = online;
        Current.LoadedFromDb = true;

    }

    public ValueTask SaveSettingsInDb()
    {
        if (!Current.LoadedFromDb || Database==null)
            return ValueTask.CompletedTask;

        return Database.UpdateTableAsync(typeof(T).Name, Current);
    }
}
