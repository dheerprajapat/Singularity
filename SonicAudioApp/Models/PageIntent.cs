using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace SonicAudioApp.Models
{
    public class PageIntent<T>
    {
        public Page FromPage { get; set; }
        public T Data { get; set; }
    }
}
