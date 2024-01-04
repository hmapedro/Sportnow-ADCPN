using System;
using System.Diagnostics;
using Xamarin.CommunityToolkit.Behaviors;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace SportNow.CustomViews
{
    /*public class RoundImage : Frame
    {
        public Image photo;

        public RoundImage()
        {

            this.BackgroundColor = Color.Red;
            this.WidthRequest = (int)180 * App.screenHeightAdapter;
            this.HeightRequest = (int)180 * App.screenHeightAdapter;
             //this.WidthRequest = (int)40 * App.screenHeightAdapter;
             //this.MinimumWidthRequest = (int)40 * App.screenHeightAdapter;
             //this.HeightRequest = (int)40 * App.screenHeightAdapter;
             //this.MinimumHeightRequest = (int)40 * App.screenHeightAdapter;
            this.CornerRadius = (float)Convert.ToDouble(90 * App.screenHeightAdapter);
            this.IsClippedToBounds = true;
            this.Padding = 0;
            this.HasShadow = false;

            photo = new Image();

            this.HorizontalOptions = LayoutOptions.Center;

            photo.HorizontalOptions = LayoutOptions.Center;
            photo.VerticalOptions = LayoutOptions.Center;

            this.Content = photo;
            this.photo.Source = "iconadicionarfoto.png";

            /*photo.Clip = new EllipseGeometry()
            {
                Center = new Point(40, 80),
                RadiusX = 80,
                RadiusY = 80
            };


        }
    }*/

    public class RoundImage : Image
    {
        public RoundImage()
        {

            this.BackgroundColor = Color.Transparent;
            this.WidthRequest = (int)180 * App.screenHeightAdapter;
            this.HeightRequest = (int)180 * App.screenHeightAdapter;
            this.HorizontalOptions = LayoutOptions.Center;
            this.VerticalOptions = LayoutOptions.Center;
            this.Aspect = Aspect.AspectFill;
            this.Source = "iconadicionarfoto.png";

            this.Clip = new EllipseGeometry()
            {
                Center = new Point(90 * App.screenHeightAdapter, 90 * App.screenHeightAdapter),
                RadiusX = 90 * App.screenHeightAdapter,
                RadiusY = 90 * App.screenHeightAdapter
            };
        }
    }
}
