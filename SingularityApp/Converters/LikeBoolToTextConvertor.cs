using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace SonicAudioApp.Converters
{
    public class LikeBoolToTextConvertor:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var v = (bool)value;
            if (v) return "Dislike";
            return "Like";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
