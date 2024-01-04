using System;
using Xamarin.Forms;

namespace SportNow.CustomViews
{
    public class RoundButton: Frame
    {

        /*public double width { get; set; }
        public string text { get; set; }*/

        //public Frame frame;
        public Button button;

        public RoundButton(string text, double width, double height)
        {
            GradientBrush gradient = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 1),
            };

            gradient.GradientStops.Add(new GradientStop(Color.FromRgb(180, 143, 86), Convert.ToSingle(0)));
            gradient.GradientStops.Add(new GradientStop(Color.FromRgb(246, 220, 178), Convert.ToSingle(0.5)));

            //BUTTON
            button = new Button
            {
                Text = text,
                Background = gradient,
                TextColor = Color.Black,
                FontSize = 13,
                WidthRequest = width,
                HeightRequest = height
            };
            //geralButton.Clicked += OnGeralButtonClicked;

            //frame = new Frame { BackgroundColor = Color.FromRgb(25, 25, 25), BorderColor = Color.LightGray, CornerRadius = 20, IsClippedToBounds = true, Padding = 0 };
            this.BackgroundColor = Color.FromRgb(25, 25, 25);
            //this.BorderColor = Color.LightGray;
            this.CornerRadius = 10;
            this.IsClippedToBounds = true;
            this.Padding = 0;
            this.WidthRequest = width;
            this.HeightRequest = height;
            this.Content = button; // relativeLayout_Button;
        }
    }
}
