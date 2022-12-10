using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Singularity.Models;

namespace Singularity.ViewModels;
public partial class SearchItemViewModel:ObservableRecipient
{
    [ObservableProperty]
    public SearchFragmentItem item;
}
