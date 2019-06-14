﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RedditClient.PresenterLayer.ViewModel
{
    public class PostPageMasterViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<PostPageMenuItem> MenuItems { get; set; }

        public PostPageMasterViewModel()
        {
            MenuItems = new ObservableCollection<PostPageMenuItem>(new[]
            {
                    new PostPageMenuItem { Id = 0, Title = "Page 1" },
                    new PostPageMenuItem { Id = 1, Title = "Page 2" },
                    new PostPageMenuItem { Id = 2, Title = "Page 3" },
                    new PostPageMenuItem { Id = 3, Title = "Page 4" },
                    new PostPageMenuItem { Id = 4, Title = "Page 5" },
                });
        }

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}