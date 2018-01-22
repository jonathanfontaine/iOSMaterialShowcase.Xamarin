using CoreGraphics;
using UIKit;

namespace iOSMaterialShowcase.Xamarin
{
    public class Utility
    {
    }

    public static class UIViewExtension
    {
        public static void AsCircle(this UIView _view)
        {
            _view.Layer.CornerRadius = _view.Frame.Width * .5f;
            _view.Layer.MasksToBounds = true;
        }

        public static void SetTintColor(this UIView _view, UIColor _color, bool _recursive)
        {
            if (_recursive)
            {
                _view.TintColor = _color;
                foreach (var view in _view.Subviews)
                    view.SetTintColor(_color, true);
            }
            else
                _view.TintColor = _color;
        }
    }

    public static class CGRectExtension
    {
        public static CGPoint Center(this CGRect _rect)
        {
            return new CGPoint(_rect.GetMidX(), _rect.GetMidY());
        }
    }

    public static class UILabelExtension
    {
        public static void SizeToFitHeight(this UILabel _label)
        {
            UILabel tempLabel = new UILabel(new CGRect(0, 0, _label.Frame.Width, float.MaxValue))
            {
                Lines = _label.Lines,
                LineBreakMode = _label.LineBreakMode,
                Font = _label.Font,
                Text = _label.Text
            };
            tempLabel.SizeToFit();
            _label.Frame = new CGRect(_label.Frame.GetMinX(), _label.Frame.GetMinY(), _label.Frame.Width, tempLabel.Frame.Height);
        }
    }
}
