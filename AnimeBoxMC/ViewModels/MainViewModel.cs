using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeBoxMC.ViewModels
{
    public class MainViewModel : Conductor<IScreen>.Collection.OneActive, IShell
    {
        public MainViewModel()
        {
            ShowInListing();
        }
        
        public void ShowInListing()
        {
            ActivateItem(new InListingViewModel());
        }

        public void ShowInAlbum()
        {
            ActivateItem(new InAlbumViewModel());
        }
        public void ShowInLyric()
        {
            ActivateItem(new InLyricViewModel());
        }
    }
}
