namespace Singularity.Core.Models;

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