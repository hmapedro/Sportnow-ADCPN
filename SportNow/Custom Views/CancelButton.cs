using System;
using Xamarin.Forms;

namespace SportNow.CustomViews
{
    public class CancelButton : Frame
    {
        public Button button;

        public CancelButton(string text, double width, double height)
        {

            createCancelButton(text, width, height, 1);
        }

        public CancelButton(string text, double width, double height, double screenAdaptor)
        {
            createCancelButton(text, width, height, screenAdaptor);
        }

        public void createCancelButton(string text, double width, double height, double screenAdaptor)
        {
            //BUTTON
            button = new Button
            {
                Text = text,
                BackgroundColor = Color.FromRgb(233, 93, 85), //gradient,
                TextColor = Color.White,
                FontSize = App.itemTitleFontSize, //* App.screenWidthAdapter,
                WidthRequest = width,
                HeightRequest = height
            };

            this.BackgroundColor = Color.FromRgb(233, 93, 85);
            this.CornerRadius = (float)(10 * screenAdaptor);
            this.IsClippedToBounds = true;
            this.Padding = 0;
            this.WidthRequest = width;
            this.HeightRequest = height;
            this.Content = button; // relativeLayout_Button;
        }
    }
}
