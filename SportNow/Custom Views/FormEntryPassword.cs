using System;
using Xamarin.Forms;

namespace SportNow.CustomViews
{
    public class FormEntryPassword: Frame
    {

        public Entry entry;
        //public string Text {get; set; }

        public FormEntryPassword(string Text, string placeholder) {

            //this.BackgroundColor = Color.FromRgb(25, 25, 25);
            this.BackgroundColor = Color.FromRgb(255, 255, 255);
            this.BorderColor = Color.FromRgb(43, 53, 129);

            this.CornerRadius = 10;
            this.IsClippedToBounds = true;
            this.Padding = new Thickness(10, 2, 10, 2);
            //this.WidthRequest = 300;
            this.HeightRequest = 45 * App.screenHeightAdapter;
            this.HasShadow = false;

            //USERNAME ENTRY
            entry = new BorderlessEntry
            {
                //Text = "tete@hotmail.com",
                Text = Text,
                IsPassword = true,
                TextColor = App.normalTextColor,
                BackgroundColor = Color.FromRgb(255, 255, 255),
                Placeholder = placeholder,
                HorizontalOptions = LayoutOptions.Start,
                WidthRequest = 300 * App.screenWidthAdapter,
                FontSize = App.formValueFontSize
            };
            this.Content = entry;

        }
    }
}
