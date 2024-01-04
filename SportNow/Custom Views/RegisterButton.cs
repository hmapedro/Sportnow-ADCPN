using System;
using Xamarin.Forms;

namespace SportNow.CustomViews
{
    public class RegisterButton: Frame
    {

        /*public double width { get; set; }
        public string text { get; set; }*/

        //public Frame frame;
        public Button button;

        public RegisterButton(string text, double width, double height)
        {

            createRegisterButton(text, width, height, 1);
        }

        public RegisterButton(string text, double width, double height, double screenAdaptor)
        {
            createRegisterButton(text, width, height, screenAdaptor);
        }

        public void createRegisterButton(string text, double width, double height, double screenAdaptor)
        {

            /*GradientBrush gradient = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 1),
            };

            gradient.GradientStops.Add(new GradientStop(Color.FromRgb(180, 143, 86), Convert.ToSingle(0)));
            gradient.GradientStops.Add(new GradientStop(Color.FromRgb(246, 220, 178), Convert.ToSingle(0.5)));*/

            //BUTTON
            button = new Button
            {
                Text = text,
                BackgroundColor = Color.FromRgb(96, 182, 89), //gradient,
                TextColor = Color.White,
                FontSize = App.itemTitleFontSize, //* App.screenWidthAdapter,
                WidthRequest = width,
                HeightRequest = height
            };
            //geralButton.Clicked += OnGeralButtonClicked;

            //frame = new Frame { BackgroundColor = Color.FromRgb(25, 25, 25), BorderColor = Color.LightGray, CornerRadius = 20, IsClippedToBounds = true, Padding = 0 };
            this.BackgroundColor = Color.FromRgb(96, 182, 89);//Color.FromRgb(25, 25, 25);
            //this.BorderColor = Color.LightGray;
            this.CornerRadius = (float)(10 * screenAdaptor);
            this.IsClippedToBounds = true;
            this.Padding = 0;
            this.WidthRequest = width;
            this.HeightRequest = height;
            this.HasShadow = false;
            this.Content = button; // relativeLayout_Button;
        }
    }
}
