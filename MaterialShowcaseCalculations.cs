using CoreGraphics;
using System;
using UIKit;

namespace iOSMaterialShowcase.Xamarin
{
    public static class MaterialShowcaseCalculations
    {
        public static bool IsInGutter(this MaterialShowcase _materialShowcase, CGPoint _center)
        {
            return _center.Y < MaterialShowcase.OffsetThreshold || _materialShowcase.containerView.Frame.Height - _center.Y < MaterialShowcase.OffsetThreshold;
        }

        public static CGPoint GetOuterCircleCenterPoint(this MaterialShowcase _materialShowcase, UIView _target)
        {
            if (_materialShowcase.IsInGutter(_target.Center))
                return _target.Center;

            var targetRadius = Math.Max(_target.Frame.Width, _target.Frame.Height) * .5f + MaterialShowcase.TargetPadding;
            var totalTextHeight = _materialShowcase.instructionView.Frame.Height;

            var onTop = _materialShowcase.GetTargetPosition(_materialShowcase.targetView, _materialShowcase.containerView) == MaterialShowcaseExtension.TargetPosition.Below;

            var left = Math.Min(_materialShowcase.instructionView.Frame.GetMinX(), _target.Frame.GetMinX() - targetRadius);
            var right = Math.Max(_materialShowcase.instructionView.Frame.GetMaxX(), _target.Frame.GetMaxX() + targetRadius);
            var titleHeight = _materialShowcase.instructionView.primaryLabel.Frame.Height;
            var centerY = onTop ? _target.Center.Y - MaterialShowcase.TargetHolderRadius - MaterialShowcase.TargetPadding - totalTextHeight + titleHeight
                : _target.Center.Y + MaterialShowcase.TargetHolderRadius + MaterialShowcase.TargetPadding + titleHeight;

            return new CGPoint((left + right) * .5f, centerY);
        }

        public static float GetOuterCircleRadius(this MaterialShowcase _materialShowcase, CGPoint _center, CGRect _textBounds, CGRect _targetBounds)
        {
            var targetCenterX = _targetBounds.GetMidX();
            var targetCenterY = _targetBounds.GetMidY();

            var expandedRadius = new nfloat(1.1 * MaterialShowcase.TargetHolderRadius);
            var expandedBounds = new CGRect(targetCenterX, targetCenterY, 0, 0);
            expandedBounds.Inset(-expandedRadius, -expandedRadius);

            var textRadius = _materialShowcase.MaxDistance(_center, _textBounds);
            var targetRadius = _materialShowcase.MaxDistance(_center, expandedBounds);
            return Math.Max(textRadius, targetRadius) + 40;
        }

        public static float MaxDistance(this MaterialShowcase _materialShowcase, CGPoint _fromPoint, CGRect _toRect)
        {
            var tl = _materialShowcase.Distance(_fromPoint, new CGPoint(_toRect.GetMinX(), _toRect.GetMinY()));
            var tr = _materialShowcase.Distance(_fromPoint, new CGPoint(_toRect.GetMaxX(), _toRect.GetMinY()));
            var bl = _materialShowcase.Distance(_fromPoint, new CGPoint(_toRect.GetMinX(), _toRect.GetMaxY()));
            var br = _materialShowcase.Distance(_fromPoint, new CGPoint(_toRect.GetMaxX(), _toRect.GetMaxY()));
            return Math.Max(tl, Math.Max(tr, Math.Max(bl, br)));
        }

        public static float Distance(this MaterialShowcase _materialShowcase, CGPoint _a, CGPoint _b)
        {
            var xDist = _a.X - _b.X;
            var yDist = _a.Y - _b.Y;
            return (float)Math.Sqrt((xDist * xDist) + (yDist * yDist));
        }
    }
}
