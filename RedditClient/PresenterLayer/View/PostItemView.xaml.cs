using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace RedditClient.PresenterLayer.View
{
    public partial class PostItemView : ContentView
    {
        #region Properties
        public static readonly BindableProperty PostItemProperty =
            BindableProperty.Create(nameof(PostItem), typeof(PostPageMenuItem), typeof(PostItemView), new PostPageMenuItem(), BindingMode.TwoWay);

        public PostPageMenuItem PostItem
        {
            get { return (PostPageMenuItem)GetValue(PostItemProperty); }
            set { SetValue(PostItemProperty, value); }
        }
        #endregion

        public PostItemView()
        {
            InitializeComponent();
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName is "PostItem")
                labelPost.Text = PostItem.Title;
        }
    }
}
