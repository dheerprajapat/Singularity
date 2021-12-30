using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace SonicAudioApp.Converters
{
    public class LikeBoolToIconElementConvertor:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return null; 
            if ((bool)value)
                return new SymbolIcon(Symbol.Dislike);

            var c=SonicAudioApp.Services.LikedSongManager.LikedSongs;
            return new SymbolIcon(Symbol.Like);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
