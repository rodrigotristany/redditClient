using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RedditClient
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PostPageDetail : ContentPage
    {
        public PostPageDetail(string title)
        {
            InitializeComponent();
            this.labelTitle.Text = title;
            NavigationPage.SetHasNavigationBar(this, false);
        }

        public PostPageDetail()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}
