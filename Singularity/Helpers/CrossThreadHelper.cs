using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Dispatching;
using Windows.System;

namespace Singularity.Helpers;
public interface ICrossThreadOperable
{
    Microsoft.UI.Dispatching.DispatcherQueue DispatcherQueue
    {
        get;set;
    }
    async ValueTask ExecuteOnUIThread(Action action)
    {
        if (DispatcherQueue is null)
            return;

        await Task.Run(() =>
        {
            DispatcherQueue.TryEnqueue(() =>
            {
                action.Invoke();
            });
        });
    }

}
