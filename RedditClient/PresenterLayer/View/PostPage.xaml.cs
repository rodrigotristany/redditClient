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
    public partial class PostPage : MasterDetailPage
    {
        public PostPage()
        {
            InitializeComponent();
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as PostPageMenuItem;
            if (item == null)
                return;

            Detail = new PostPageDetail(item.Title);
            IsPresented = false;

            MasterPage.ListView.SelectedItem = null;
        }
    }
}
