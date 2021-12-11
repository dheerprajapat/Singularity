using SonicAudioApp.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace SonicAudioApp.Converters
{
    internal class SliderValueDurationStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if(value == null)
                return null;
            var seconds = System.Convert.ToInt32(value);
            return MediaPlayerControl.ConvertTimeSpanToDuration(TimeSpan.FromSeconds(seconds));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
