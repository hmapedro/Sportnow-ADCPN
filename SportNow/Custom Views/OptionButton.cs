using System;
using Xamarin.Forms;

namespace SportNow.CustomViews
{
    public class OptionButton: RelativeLayout
    {

        /*public double width { get; set; }
        public string text { get; set; }*/

        //public Frame frame;
        public Label label;
        public Image image;
		public Frame frame;


		public OptionButton(string text, string imageSource, double width, double height)
        {
			this.WidthRequest = width;
			this.HeightRequest = 80 * App.screenHeightAdapter;
			frame = new Frame
			{
				CornerRadius = 5,
				IsClippedToBounds = true,
				BorderColor = Color.Transparent,
				BackgroundColor = Color.Black,
				Padding = new Thickness(2, 2, 2, 2),
				HeightRequest = 80 * App.screenHeightAdapter,
				VerticalOptions = LayoutOptions.Center,
				HasShadow = false
			};

			image = new Image { Source = imageSource, Aspect = Aspect.AspectFill, Opacity = 0.4 }; //, HeightRequest = 60, WidthRequest = 60
			frame.Content = image;

			this.Children.Add(frame,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(0),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width - 5);
				}),
				heightConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Height);
				}));

			label = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.titleFontSize, TextColor = Color.White, LineBreakMode = LineBreakMode.WordWrap };
			label.Text = text;

			this.Children.Add(label,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Height) / 2 - (20 * App.screenHeightAdapter);
				}),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width);
				}),
				heightConstraint: Constraint.Constant(40 * App.screenHeightAdapter));

			//return itemRelativeLayout;
		}
    }
}
