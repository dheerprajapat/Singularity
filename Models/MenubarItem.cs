using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Singularity.Models
{
    public record MenubarItem
    {
        public string? Name { get; set; }
        public string? Text { get; set; }
        public MenubarItemType Type { get; set; }

        public MenubarItem(string name, string text, MenubarItemType menubarItemType) 
        { 
            Name = name;
            Text = text;
            Type = menubarItemType;
        }
        public enum MenubarItemType
        {
            AddToPlayList,
            Download,
            Share
        }
    }
}
