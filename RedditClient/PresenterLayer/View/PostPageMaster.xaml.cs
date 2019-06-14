using RedditClient.PresenterLayer.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RedditClient
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PostPageMaster : ContentPage
    {
        public ListView ListView;

        public PostPageMaster()
        {
            InitializeComponent();

            BindingContext = new PostPageMasterViewModel();
            ListView = MenuItemsListView;
        }
    }
}
