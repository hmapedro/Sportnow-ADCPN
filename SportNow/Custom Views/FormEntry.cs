using System;
using Xamarin.Forms;

namespace SportNow.CustomViews
{
    public class FormEntry: Frame
    {

        public Entry entry;
        //public string Text {get; set; }


        public FormEntry(string text, string placeholder, double width)
        {
            createFormEntry(text, placeholder, width, Keyboard.Text);
        }

        public FormEntry(string text, string placeholder, double width, Keyboard keyboard)
        {
            createFormEntry(text, placeholder, width, keyboard);
        }

        public void createFormEntry(string text, string placeholder, double width, Keyboard keyboard)
        {
            //this.BackgroundColor = Color.FromRgb(25, 25, 25);
            this.BackgroundColor = Color.White;
            this.BorderColor = Color.FromRgb(43, 53, 129);

            this.CornerRadius = 10;
            this.IsClippedToBounds = true;
            this.Padding = new Thickness(10,2,10,2);
            this.WidthRequest = width;
            this.HeightRequest = 45 * App.screenHeightAdapter;
            this.HasShadow = false;

            //USERNAME ENTRY
            entry = new BorderlessEntry
            {
                //Text = "tete@hotmail.com",
                Text = text,
                TextColor = App.normalTextColor,
                BackgroundColor = Color.FromRgb(255, 255, 255),
                Placeholder = placeholder,
                HorizontalOptions = LayoutOptions.Start,
                WidthRequest = width,
                FontSize = App.formValueFontSize,
                Keyboard = keyboard

            };
            this.Content = entry;
        }
    }
}
