using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Singularity.Models
{
    internal class MusicMenubar
    {
        private static MusicMenubar? instance;
        
        public static MusicMenubar Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new MusicMenubar();
                }
                return instance;
            }
        }

        public List<MenubarItem> MenubarItems { get; set; }

        public MusicMenubar()
        {
            MenubarItems = new List<MenubarItem>
            {
                new MenubarItem("AddToPlaylist", "Add to Playlist", MenubarItem.MenubarItemType.AddToPlayList),
                new MenubarItem("Share", "Share", MenubarItem.MenubarItemType.Share),
                new MenubarItem("Download", "Download", MenubarItem.MenubarItemType.Download)
            };
        }
    }
}
