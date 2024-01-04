using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace SportNow.CustomViews
{
    public class MenuButton: RelativeLayout
    {
        public Button button;
        public Label buttontext;
        BoxView line;

        public MenuButton(string text, double width, double height)
        {
            buttontext = new Label()
            {
                Text = text,
                BackgroundColor = Color.Transparent,
                FontSize = App.menuButtonFontSize,
                TextColor = App.normalTextColor,
                LineBreakMode = LineBreakMode.WordWrap,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.End,
                MinimumWidthRequest = width,
                MinimumHeightRequest = (height - 7) * App.screenHeightAdapter,
                WidthRequest = width,
                HeightRequest = (height - 7) * App.screenHeightAdapter
            };

            if (Device.RuntimePlatform == Device.iOS)
            {
                button = new Button
                {
                    MinimumWidthRequest = width,
                    MinimumHeightRequest = (height) * App.screenHeightAdapter,
                    WidthRequest = width,
                    HeightRequest = (height) * App.screenHeightAdapter
                };
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                //this.BackgroundColor = Color.FromRgb(200, 200, 200);
                button = new Button
                {
                    //Text = text,
                    BackgroundColor = Color.Transparent,
                    TextColor = App.normalTextColor,
                    FontSize = App.menuButtonFontSize,
                    MinimumWidthRequest = width,
                    MinimumHeightRequest = (height) * App.screenHeightAdapter,
                    WidthRequest = width,
                    HeightRequest = (height) * App.screenHeightAdapter
                };
            }         

            this.MinimumHeightRequest = height * App.screenHeightAdapter;
            this.MinimumWidthRequest = width;
            this.HeightRequest = height * App.screenHeightAdapter;
            this.WidthRequest = width;

            line = new BoxView
            {
                Color = App.normalTextColor,
                WidthRequest = width,
                HeightRequest = 2 * App.screenHeightAdapter,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };


            this.Children.Add(buttontext,
                xConstraint: Constraint.Constant(0),
                yConstraint: Constraint.Constant(0),
                widthConstraint: Constraint.Constant(width),
                heightConstraint: Constraint.Constant(height-7) // size of screen -80
            );


            this.Children.Add(button,
                xConstraint: Constraint.Constant(0),
                yConstraint: Constraint.Constant(0),
                widthConstraint: Constraint.Constant(width),
                heightConstraint: Constraint.Constant(height) // size of screen -80
            );

            this.Children.Add(line,
                xConstraint: Constraint.Constant(0),
                yConstraint: Constraint.Constant(height - 3),
                widthConstraint: Constraint.Constant(width),
                heightConstraint: Constraint.Constant(2 * App.screenHeightAdapter) // size of screen -80
            );
        }

        public void activate() {

            this.buttontext.TextColor = App.topColor;
            this.line.Color = App.topColor;
        }

        public void deactivate()
        {

            this.buttontext.TextColor = App.normalTextColor;
            this.line.Color = App.normalTextColor;
        }
    }
}
