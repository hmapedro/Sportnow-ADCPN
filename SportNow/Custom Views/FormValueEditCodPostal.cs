using System;
using System.Diagnostics;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SportNow.CustomViews
{
   
     public class FormValueEditCodPostal : Frame
     {

         public Entry entry;
         //public string Text {get; set; }

         public FormValueEditCodPostal(string Text) {

            this.CornerRadius = 5 * (float) App.screenHeightAdapter;
            this.IsClippedToBounds = true;
            BorderColor = App.topColor;
            BackgroundColor = Color.Transparent;
            this.Padding = new Thickness(1, 2, 2, 2);
            //this.MinimumHeightRequest = 50;
            this.HeightRequest = 45 * App.screenHeightAdapter;
            this.VerticalOptions = LayoutOptions.Center;
            this.HasShadow = false;

             entry = new Entry
             {
                 //Padding = new Thickness(5,0,5,0),
                 Placeholder = "XXXX-XXX",
                 Keyboard = Keyboard.Numeric,
                 Text = Text,
                 HorizontalTextAlignment = TextAlignment.Start,
                 //VerticalTextAlignment = TextAlignment.Center,
                 TextColor = App.normalTextColor,
                 BackgroundColor = Color.White,
                 FontSize = App.formValueFontSize,
                 //HeightRequest = 30
             };
            
             this.Content = entry; // relativeLayout_Button;

            this.Content = entry;
            entry.Behaviors.Add(new MaskedBehavior() { Mask = "XXXX-XXX" });
        }
     }
}
