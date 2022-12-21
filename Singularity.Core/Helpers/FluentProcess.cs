using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Singularity.Core.Helpers;
public class FluentProcess
{
    readonly Process _process;
    public Process Process => _process;
    private FluentProcess(Process p)
    {
        _process = p;
    }

    public static FluentProcess Create(string filename, string arguments = "")
    {
        var _process = new Process();
        _process.StartInfo = new ProcessStartInfo()
        {
            FileName = filename,
            Arguments = arguments,
            UseShellExecute = true,
            RedirectStandardOutput = false,
            RedirectStandardError = false,
            CreateNoWindow = true,
        };
        return new FluentProcess(_process);
    }
    public FluentProcess WithArguments(string args)
    {
        _process.StartInfo.Arguments = args;
        return this;
    }
    public FluentProcess WithWorkingDirectory(string dir)
    {
        _process.StartInfo.WorkingDirectory = dir;
        return this;
    }
    public FluentProcess Run()
    {
        _process.Start();
        return this;
    }
    public FluentProcess RunAsAdmin()
    {
        _process.StartInfo.Verb = "runas";
        _process.Start();
        return this;
    }
}