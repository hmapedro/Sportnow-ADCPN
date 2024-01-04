using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace SportNow.Views
{
    public class DefaultPage : ContentPage
	{
        StackLayout stack;
        ActivityIndicator indicator;
        Image loading;

        public DefaultPage()
        {
			this.initBaseLayout();
		}

		public RelativeLayout relativeLayout;

		public void initBaseLayout()
		{

			this.BackgroundColor = Color.FromRgb(255, 255, 255);

			relativeLayout = new RelativeLayout
			{
				Margin = new Thickness(10)
			};
			Content = relativeLayout;

			NavigationPage.SetBackButtonTitle(this, "");


            if (Application.Current.MainPage != null)
            {
                var navigationPage = Application.Current.MainPage as NavigationPage;
                navigationPage.BarBackgroundColor = Color.White;
                navigationPage.BarTextColor = Color.Black;
            }

            stack = new StackLayout() { BackgroundColor = Color.FromRgb(250, 250, 250), Opacity = 0.6 };
            loading = new Image() { Source = "loading.gif", IsAnimationPlaying = true }; 

            //indicator = new ActivityIndicator() { Color = App.topColor, HeightRequest = 100, WidthRequest = 100, MinimumHeightRequest = 100, MinimumWidthRequest = 100};
        }

        public void showActivityIndicator()
        {
            //indicator.IsRunning = true;

            /*if (relativeLayout == null)
            {
                initBaseLayout();
            }*/

            relativeLayout.Children.Add(stack,
                xConstraint: Constraint.Constant(0),
                yConstraint: Constraint.Constant(0),
                widthConstraint: Constraint.RelativeToParent((parent) => { return (parent.Width); }),
                heightConstraint: Constraint.RelativeToParent((parent) => { return (parent.Height); }));
            /*relativeLayout.Children.Add(indicator,
                xConstraint: Constraint.RelativeToParent((parent) => { return ((parent.Width / 2) - 50); }),
                yConstraint: Constraint.RelativeToParent((parent) => { return ((parent.Height / 2) - 50); }),
                widthConstraint: Constraint.Constant(100),
                heightConstraint: Constraint.Constant(100));*/
            relativeLayout.Children.Add(loading,
                xConstraint: Constraint.RelativeToParent((parent) => { return ((parent.Width / 2) - 50); }),
                yConstraint: Constraint.RelativeToParent((parent) => { return ((parent.Height / 2) - 50); }),
                widthConstraint: Constraint.Constant(100),
                heightConstraint: Constraint.Constant(100));

        }

        public void hideActivityIndicator()
        {
            relativeLayout.Children.Remove(stack);
            relativeLayout.Children.Remove(loading);
            //indicator.IsRunning = false;
        }
    }
}
