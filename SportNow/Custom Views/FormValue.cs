using System;
using System.Diagnostics;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SportNow.CustomViews
{
    public class FormValue : Frame
    {

        public Label label;
        //public string Text {get; set; }

        public FormValue(string Text) {

            this.CornerRadius = 5;
            this.IsClippedToBounds = true;
            BorderColor = App.topColor;
			BackgroundColor = Color.White;
            Padding = new Thickness(2, 2, 2, 2);
            //this.MinimumHeightRequest = 50;
            this.HeightRequest = 45 * App.screenHeightAdapter;
            this.VerticalOptions = LayoutOptions.Center;
            this.HasShadow = false;

            label = new Label
            {
                Padding = new Thickness(5,0,5,0),
                Text = Text,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = App.normalTextColor,
                //BackgroundColor = Color.FromRgb(25, 25, 25),
                FontSize = App.formValueFontSize,
            }; 

            this.Content = label; // relativeLayout_Button;

        }

        public FormValue(string Text, int fontSize, Color backgroundColor, Color textColor, TextAlignment textHorizontalAlignment)
        {

            this.CornerRadius = 5;
            this.IsClippedToBounds = true;
            BorderColor = App.topColor;
            BackgroundColor= backgroundColor;
            Padding = new Thickness(2, 2, 2, 2);
            //this.MinimumHeightRequest = 50;
            this.HeightRequest = 45 * App.screenHeightAdapter;
            this.VerticalOptions = LayoutOptions.Center;
            this.HasShadow = false;

            label = new Label
            {
                Padding = new Thickness(5, 0, 5, 0),
                Text = Text,
                HorizontalTextAlignment = textHorizontalAlignment,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = textColor,
                BackgroundColor = backgroundColor,
                FontSize = fontSize,
            };

            this.Content = label; // relativeLayout_Button;

        }
    }
}
