using CoreGraphics;
using UIKit;

namespace iOSMaterialShowcase.Xamarin
{
    public class MaterialShowcaseInstructionView : UIView
    {
        internal const float PrimaryTextSize = 20f;
        internal const float SecondaryTextSize = 15f;
        internal static UIColor PrimaryTextColor = UIColor.White;
        internal static UIColor SecondaryTextColor = UIColor.White.ColorWithAlpha(.87f);
        internal const string PrimaryDefaultText = "";
        internal const string SecondaryDefaultText = "";

        public UILabel primaryLabel;
        public UILabel secondaryLabel;

        // Text
        public string primaryText;
        public string secondaryText;
        public UIColor primaryTextColor;
        public UIColor secondaryTextColor;
        public float primaryTextSize;
        public float secondaryTextSize;
        public UIFont primaryTextFont;
        public UIFont secondaryTextFont;

        public MaterialShowcaseInstructionView()
            : base(new CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, 0))
        {
            Configure();
        }

        private void Configure()
        {
            SetDefaultProperties();
        }

        private void SetDefaultProperties()
        {
            // Text
            primaryText = PrimaryDefaultText;
            secondaryText = SecondaryDefaultText;
            primaryTextColor = PrimaryTextColor;
            secondaryTextColor = SecondaryTextColor;
            primaryTextSize = PrimaryTextSize;
            secondaryTextSize = SecondaryTextSize;
        }

        /// Configures and adds primary label view
        private void AddPrimaryLabel()
        {
            primaryLabel = new UILabel();

            if (primaryTextFont != null)
                primaryLabel.Font = primaryTextFont;
            else
                primaryLabel.Font = UIFont.BoldSystemFontOfSize(primaryTextSize);

            primaryLabel.TextColor = primaryTextColor;
            primaryLabel.TextAlignment = UITextAlignment.Left;
            primaryLabel.Lines = 0;
            primaryLabel.LineBreakMode = UILineBreakMode.WordWrap;
            primaryLabel.Text = primaryText;

            //// Calculate x position
            //MaterialShowcase materialShowcase = (MaterialShowcase)Superview;
            //var xPosition = materialShowcase.backgroundView.Frame.GetMinX() > 0 ?
            //  materialShowcase.backgroundView.Frame.GetMinX() + MaterialShowcase.labelMargin : MaterialShowcase.labelMargin;

            //// Calculate y position
            //float yPosition;

            //if (materialShowcase.GetTargetPosition(materialShowcase.targetView, materialShowcase.containerView) == MaterialShowcaseExtension.TargetPosition.Above)
            //    yPosition = (float)Center.Y + MaterialShowcase.textCenterOffset;
            //else
            //    yPosition = (float)Center.Y - MaterialShowcase.textCenterOffset - MaterialShowcase.labelDefaultHeight * 2;

            primaryLabel.Frame = new CGRect(0, 0, Frame.Width, 0);
            primaryLabel.SizeToFitHeight();

            AddSubview(primaryLabel);
        }

        /// Configures and adds secondary label view
        private void AddSecondaryLabel()
        {
            secondaryLabel = new UILabel();
            if (secondaryTextFont != null)
                secondaryLabel.Font = secondaryTextFont;
            else
                secondaryLabel.Font = UIFont.SystemFontOfSize(secondaryTextSize);
            secondaryLabel.TextColor = secondaryTextColor;
            secondaryLabel.TextAlignment = UITextAlignment.Left;
            secondaryLabel.LineBreakMode = UILineBreakMode.WordWrap;
            secondaryLabel.Text = secondaryText;
            secondaryLabel.Lines = 3;

            secondaryLabel.Frame = new CGRect(0, primaryLabel.Frame.Height, Frame.Width, 0);
            secondaryLabel.SizeToFitHeight();
            AddSubview(secondaryLabel);
            Frame = new CGRect(Frame.GetMinX(), Frame.GetMinY(), UIScreen.MainScreen.Bounds.Width, primaryLabel.Frame.Height + secondaryLabel.Frame.Height);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            AddPrimaryLabel();
            AddSecondaryLabel();

            foreach (var subview in Subviews)
                subview.UserInteractionEnabled = false;
        }
    }
}
