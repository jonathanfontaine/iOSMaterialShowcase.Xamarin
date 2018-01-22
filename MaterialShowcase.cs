using CoreGraphics;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;

namespace iOSMaterialShowcase.Xamarin
{
    public interface IMaterialShowcaseDelegate
    {
        void ShowCaseWillDismiss();
        void ShowCaseDidDismiss();
    }

    public class MaterialShowcase : UIView
    {
        // MARK: Material design guideline constant
        public const float BackgroundAlpha = .96f;
        public const float TargetHolderRadius = 44f;
        public const float TextCenterOffset = 44f + 20f;
        public const float InstructionsCenterOffset = 20f;
        public const float LabelMargin = 40f;
        public const float TargetPadding = 20f;

        // Other default properties
        public const float LabelDefaultHeight = 50f;
        public static UIColor BackgroundDefaultColor = UIColor.Black;
        public static UIColor TargetHolderColor = UIColor.White;

        // MARK: Animation properties
        public const float AniComeInDuration = .5f; // second
        public const float AniGoOutDuration = .5f;  // second
        public const float AniTargetHolderScale = 2.2f;
        public static UIColor AniRippleColor = UIColor.White;
        public const float AniRippleAlpha = .5f;
        public const float AniRippleScale = 1.6f;

        public const float OffsetThreshold = 88f;

        // MARK: Private view properties
        public UIView containerView;
        public UIView targetView;
        public UIView backgroundView;
        public UIView targetHolderView;
        public UIView hiddenTargetHolderView;
        public UIView targetRippleView;
        public UIView targetCopyView;
        public MaterialShowcaseInstructionView instructionView;

        // MARK: Public Properties

        // Background
        public UIColor backgroundPromptColor;
        public float backgroundPromptColorAlpha;
        // Target
        public bool shouldSetTintColor = true;
        public UIColor targetTintColor;
        public float targetHolderRadius;
        public UIColor targetHolderColor;
        // Text
        public string primaryText;
        public string secondaryText;
        public UIColor primaryTextColor;
        public UIColor secondaryTextColor;
        public float primaryTextSize;
        public float secondaryTextSize;
        public UIFont primaryTextFont;
        public UIFont secondaryTextFont;
        // Animation
        public float aniComeInDuration;
        public float aniGoOutDuration;
        public float aniRippleScale;
        public UIColor aniRippleColor;
        public float aniRippleAlpha;
        // Delegate
        public IMaterialShowcaseDelegate showcaseDelegate;

        public MaterialShowcase() : base(new CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height))
        {
            this.Configure();
        }
    }

    public static class MaterialShowcaseExtension
    {

        /// Defines the position of target view
        /// which helps to place texts at suitable positions
        public enum TargetPosition
        {
            Above, // at upper screen part
            Below // at lower screen part
        }

        public static void Configure(this MaterialShowcase materialShowcase)
        {
            materialShowcase.BackgroundColor = UIColor.Clear;
            var window = UIApplication.SharedApplication.Delegate?.GetWindow();
            if (window != null)
            {
                materialShowcase.containerView = window;
                materialShowcase.SetDefaultProperties();
            }
        }

        public static void SetDefaultProperties(this MaterialShowcase materialShowcase)
        {
            // Background
            materialShowcase.backgroundPromptColor = MaterialShowcase.BackgroundDefaultColor;
            materialShowcase.backgroundPromptColorAlpha = MaterialShowcase.BackgroundAlpha;
            // Target view
            materialShowcase.targetTintColor = MaterialShowcase.BackgroundDefaultColor;
            materialShowcase.targetHolderColor = MaterialShowcase.TargetHolderColor;
            materialShowcase.targetHolderRadius = MaterialShowcase.TargetHolderRadius;
            // Text
            materialShowcase.primaryText = MaterialShowcaseInstructionView.PrimaryDefaultText;
            materialShowcase.secondaryText = MaterialShowcaseInstructionView.SecondaryDefaultText;
            materialShowcase.primaryTextColor = MaterialShowcaseInstructionView.PrimaryTextColor;
            materialShowcase.secondaryTextColor = MaterialShowcaseInstructionView.SecondaryTextColor;
            materialShowcase.primaryTextSize = MaterialShowcaseInstructionView.PrimaryTextSize;
            materialShowcase.secondaryTextSize = MaterialShowcaseInstructionView.SecondaryTextSize;
            // Animation
            materialShowcase.aniComeInDuration = MaterialShowcase.AniComeInDuration;
            materialShowcase.aniGoOutDuration = MaterialShowcase.AniGoOutDuration;
            materialShowcase.aniRippleAlpha = MaterialShowcase.AniRippleAlpha;
            materialShowcase.aniRippleColor = MaterialShowcase.AniRippleColor;
            materialShowcase.aniRippleScale = MaterialShowcase.AniRippleScale;
        }

        public static void StartAnimation(this MaterialShowcase materialShowcase)
        {
            //UIViewAnimationOptions optionsOLD = UIViewAnimationOptions.CurveEaseInOut | UIViewAnimationOptions.Repeat;
            UIViewKeyframeAnimationOptions options = UIViewKeyframeAnimationOptions.Repeat | 0 << 16;

            UIView.AnimateKeyframes(1, 0, options, () =>
            {
                UIView.AddKeyframeWithRelativeStartTime(0, .5f, () =>
                {
                    materialShowcase.targetRippleView.Alpha = MaterialShowcase.AniRippleAlpha;
                    materialShowcase.targetHolderView.Transform = CGAffineTransform.MakeScale(1.1f, 1.1f);
                    materialShowcase.targetRippleView.Transform = CGAffineTransform.MakeScale(1.1f, 1.1f);
                });
                UIView.AddKeyframeWithRelativeStartTime(.5f, .5f, () =>
                {
                    materialShowcase.targetHolderView.Transform = CGAffineTransform.MakeIdentity();
                    materialShowcase.targetRippleView.Alpha = 0;
                    materialShowcase.targetRippleView.Transform = CGAffineTransform.MakeScale(materialShowcase.aniRippleScale, materialShowcase.aniRippleScale);
                });
            }, (completion) =>
             {
                 Console.WriteLine("ok");
             });
        }

        // Calculates the center point based on targetview
        public static CGPoint CalculateCenter(this MaterialShowcase materialShowcase, UIView targetView, UIView containerView)
        {
            var targetRect = targetView.ConvertRectToCoordinateSpace(targetView.Bounds, containerView);
            return targetRect.Center();
        }

        /// A background view which add ripple animation when showing target view
        public static void AddTargetRipple(this MaterialShowcase materialShowcase, CGPoint atCenter)
        {
            materialShowcase.targetRippleView = new UIView(new CGRect(0, 0, materialShowcase.targetHolderRadius * 2, materialShowcase.targetHolderRadius * 2));
            materialShowcase.targetRippleView.Center = atCenter;
            materialShowcase.targetRippleView.BackgroundColor = materialShowcase.aniRippleColor;
            materialShowcase.targetRippleView.Alpha = .0f; //set it invisible
            materialShowcase.targetRippleView.AsCircle();
            materialShowcase.AddSubview(materialShowcase.targetRippleView);
        }

        /// A circle-shape background view of target view
        public static void AddTargetHolder(this MaterialShowcase materialShowcase, CGPoint atCenter)
        {
            materialShowcase.hiddenTargetHolderView = new UIView();
            materialShowcase.hiddenTargetHolderView.Hidden = true;
            materialShowcase.targetHolderView = new UIView(new CGRect(0, 0, materialShowcase.targetHolderRadius * 2, materialShowcase.targetHolderRadius * 2));
            materialShowcase.targetHolderView.Center = atCenter;
            materialShowcase.targetHolderView.BackgroundColor = materialShowcase.targetHolderColor;
            materialShowcase.targetHolderView.AsCircle();
            materialShowcase.hiddenTargetHolderView.Frame = materialShowcase.targetHolderView.Frame;
            materialShowcase.targetHolderView.Transform = CGAffineTransform.MakeScale(1 / MaterialShowcase.AniTargetHolderScale, 1 / MaterialShowcase.AniTargetHolderScale); // Initial set to support animation
            materialShowcase.AddSubview(materialShowcase.hiddenTargetHolderView);
            materialShowcase.AddSubview(materialShowcase.targetHolderView);
        }

        /// Create a copy view of target view
        /// It helps us not to affect the original target view
        public static void AddTarget(this MaterialShowcase materialShowcase, CGPoint atCenter)
        {
            materialShowcase.targetCopyView = materialShowcase.targetView.SnapshotView(true);
            if (materialShowcase.shouldSetTintColor)
            {
                materialShowcase.targetCopyView.SetTintColor(materialShowcase.targetTintColor, true);
                if (materialShowcase.targetCopyView is UIButton)
                {
                    UIButton button = materialShowcase.targetView as UIButton;
                    UIButton buttonCopy = materialShowcase.targetCopyView as UIButton;
                    buttonCopy.SetImage(button.ImageForState(UIControlState.Normal)?.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate), UIControlState.Normal);
                    buttonCopy.SetTitleColor(materialShowcase.targetTintColor, UIControlState.Normal);
                    buttonCopy.Enabled = true;
                }
                else if (materialShowcase.targetCopyView is UIImageView)
                {
                    UIImageView imageView = materialShowcase.targetView as UIImageView;
                    UIImageView imageViewCopy = materialShowcase.targetCopyView as UIImageView;
                    imageViewCopy.Image = imageView.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                }
                else if (materialShowcase.targetCopyView.Subviews.FirstOrDefault() is UIImageView && materialShowcase.targetCopyView.Subviews.LastOrDefault() is UILabel)
                {
                    UIImageView imageViewCopy = materialShowcase.targetCopyView.Subviews.First() as UIImageView;
                    UILabel labelCopy = materialShowcase.targetCopyView.Subviews.Last() as UILabel;
                    UIImageView imageView = (UIImageView)materialShowcase.targetView.Subviews.First();
                    imageViewCopy.Image = imageView.Image?.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                    labelCopy.TextColor = materialShowcase.targetTintColor;
                }
                else if (materialShowcase.targetCopyView is UILabel)
                    (materialShowcase.targetCopyView as UILabel).TextColor = materialShowcase.targetTintColor;
            }

            var width = materialShowcase.targetCopyView.Frame.Width;
            var height = materialShowcase.targetCopyView.Frame.Height;
            materialShowcase.targetCopyView.Frame = new CGRect(0, 0, width, height);
            materialShowcase.targetCopyView.Center = atCenter;
            materialShowcase.targetCopyView.TranslatesAutoresizingMaskIntoConstraints = true;
            materialShowcase.AddSubview(materialShowcase.targetCopyView);
        }

        /// Detects the position of target view relative to its container
        public static TargetPosition GetTargetPosition(this MaterialShowcase materialShowcase, UIView target, UIView container)
        {
            var center = materialShowcase.CalculateCenter(materialShowcase.targetView, container);
            if (center.Y < container.Frame.Height * .5f)
                return TargetPosition.Above;
            else
                return TargetPosition.Below;
        }

        /// Configures and adds primary label view
        public static void AddInstructionView(this MaterialShowcase materialShowcase, CGPoint atCenter)
        {
            materialShowcase.instructionView = new MaterialShowcaseInstructionView();
            materialShowcase.instructionView.primaryTextFont = materialShowcase.primaryTextFont;
            materialShowcase.instructionView.primaryTextSize = materialShowcase.primaryTextSize;
            materialShowcase.instructionView.primaryTextColor = materialShowcase.primaryTextColor;
            materialShowcase.instructionView.primaryText = materialShowcase.primaryText;

            materialShowcase.instructionView.secondaryTextFont = materialShowcase.secondaryTextFont;
            materialShowcase.instructionView.secondaryTextSize = materialShowcase.secondaryTextSize;
            materialShowcase.instructionView.secondaryTextColor = materialShowcase.secondaryTextColor;
            materialShowcase.instructionView.secondaryText = materialShowcase.secondaryText;

            // Calculate x position
            var xPosition = MaterialShowcase.LabelMargin;

            // Calculate y position
            float yPosition;

            if (materialShowcase.GetTargetPosition(materialShowcase.targetView, materialShowcase.containerView) == TargetPosition.Above)
                yPosition = (float)atCenter.Y + MaterialShowcase.TextCenterOffset;
            else
                yPosition = (float)atCenter.Y - MaterialShowcase.TextCenterOffset - MaterialShowcase.LabelDefaultHeight * 2;

            materialShowcase.instructionView.Frame = new CGRect(
                xPosition,
                yPosition,
                materialShowcase.containerView.Frame.Width - (xPosition + xPosition),
                0);
            materialShowcase.AddSubview(materialShowcase.instructionView);
        }

        /// Add background which is a big circle
        public static void AddBackground(this MaterialShowcase materialShowcase)
        {
            float radius;
            var center = materialShowcase.GetOuterCircleCenterPoint(materialShowcase.targetCopyView);

            if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
                radius = 300.0f;
            else
                radius = materialShowcase.GetOuterCircleRadius(center, materialShowcase.instructionView.Frame, materialShowcase.targetCopyView.Frame);

            materialShowcase.backgroundView = new UIView(new CGRect(0, 0, radius * 2, radius * 2))
            {
                Center = center,
                BackgroundColor = materialShowcase.backgroundPromptColor.ColorWithAlpha(materialShowcase.backgroundPromptColorAlpha)
            };
            materialShowcase.backgroundView.AsCircle();
            materialShowcase.InsertSubviewBelow(materialShowcase.backgroundView, materialShowcase.targetRippleView);
        }

        /// Default action when dimissing showcase
        /// Notifies delegate, removes views, and handles out-going animation
        public static void CompleteShowcase(this MaterialShowcase materialShowcase, bool animated = true)
        {
            if (materialShowcase.showcaseDelegate != null)
                materialShowcase.showcaseDelegate.ShowCaseWillDismiss();
            if (animated)
            {
                materialShowcase.targetRippleView.RemoveFromSuperview();
                UIView.AnimateKeyframes(materialShowcase.aniGoOutDuration, 0, UIViewKeyframeAnimationOptions.CalculationModeLinear, () =>
                {
                    UIView.AddKeyframeWithRelativeStartTime(0, .6f, () =>
                    {
                        materialShowcase.targetHolderView.Transform = CGAffineTransform.MakeScale(.4f, .4f);
                        materialShowcase.backgroundView.Transform = CGAffineTransform.MakeScale(1.3f, 1.3f);
                        materialShowcase.backgroundView.Alpha = 0;
                    });
                    UIView.AddKeyframeWithRelativeStartTime(.6f, .4f, () =>
                    {
                        materialShowcase.Alpha = 0;
                    });
                }, (success) =>
                {
                    // Recycle subviews
                    materialShowcase.RecycleSubviews();
                    // Remove it from current screen
                    materialShowcase.RemoveFromSuperview();
                });
            }
            else
            {
                // Recycle subviews
                materialShowcase.RecycleSubviews();
                // Remove it from current screen
                materialShowcase.RemoveFromSuperview();
            }
            if (materialShowcase.showcaseDelegate != null)
                materialShowcase.showcaseDelegate.ShowCaseDidDismiss();
        }

        private static void RecycleSubviews(this MaterialShowcase materialShowcase)
        {
            foreach (var subview in materialShowcase.Subviews)
                subview.RemoveFromSuperview();
        }

        private static void TapGestureSelector(this MaterialShowcase materialShowcase)
        {
            materialShowcase.CompleteShowcase();
        }

        /// Handles user's tap
        public static UIGestureRecognizer TapGestureRecoganizer(this MaterialShowcase materialShowcase)
        {
            return new UITapGestureRecognizer(() => materialShowcase.TapGestureSelector())
            {
                NumberOfTapsRequired = 1,
                NumberOfTouchesRequired = 1
            };
        }

        public static void InitViews(this MaterialShowcase materialShowcase)
        {
            var center = materialShowcase.CalculateCenter(materialShowcase.targetView, materialShowcase.containerView);
            materialShowcase.AddTargetRipple(center);
            materialShowcase.AddTargetHolder(center);
            materialShowcase.AddTarget(center);
            materialShowcase.AddInstructionView(center);
            materialShowcase.instructionView.LayoutIfNeeded();
            materialShowcase.AddBackground();

            // Add gesture recognizer for both container and its subview
            materialShowcase.AddGestureRecognizer(materialShowcase.TapGestureRecoganizer());
            // Disable subview interaction to let users click to general view only
            foreach (var subView in materialShowcase.Subviews)
                subView.UserInteractionEnabled = false;
        }

        // Gets all UIView from TabBarItem.
        private static List<UIView> OrderedTabBarItemViews(UITabBar ofTabBar)
        {
            var interactionViews = ofTabBar.Subviews.Where(sv => sv.UserInteractionEnabled);
            return interactionViews.OrderBy(iv => iv.Frame.GetMinX()).ToList();
        }

        /// Sets a general UIView as target
        public static void SetTargetView(this MaterialShowcase materialShowcase, UIView view)
        {
            materialShowcase.targetView = view;
            if (materialShowcase.targetView is UILabel)
            {
                UILabel label = materialShowcase.targetView as UILabel;
                materialShowcase.targetTintColor = label.TextColor;
                materialShowcase.backgroundPromptColor = label.TextColor;
            }
            else if (materialShowcase.targetView is UIButton)
            {
                UIButton button = materialShowcase.targetView as UIButton;
                var tintColor = button.TitleColor(UIControlState.Normal);
                materialShowcase.targetTintColor = tintColor;
                materialShowcase.backgroundPromptColor = tintColor;
            }
            else
            {
                materialShowcase.targetTintColor = materialShowcase.targetView.TintColor;
                materialShowcase.backgroundPromptColor = materialShowcase.targetView.TintColor;
            }
        }

        /// Sets a UIBarButtonItem as target
        public static void SetTargetView(this MaterialShowcase materialShowcase, UIBarButtonItem barButtonItem)
        {
            var view = barButtonItem.ValueForKey(new NSString("view"));
            if (view != null && view is UIView)
                materialShowcase.targetView = ((UIView)view).Subviews.First();
            materialShowcase.backgroundPromptColor = UINavigationBar.Appearance.TintColor;
        }

        /// Sets a UITabBar Item as target
        public static void SetTargetView(this MaterialShowcase materialShowcase, UITabBar tabBar, int itemIndex)
        {
            var tabBarItems = OrderedTabBarItemViews(tabBar);
            if (itemIndex < tabBarItems.Count)
            {
                materialShowcase.targetView = tabBarItems[itemIndex];
                materialShowcase.targetTintColor = tabBar.TintColor;
                materialShowcase.backgroundPromptColor = tabBar.TintColor;
            }
            else
                Console.WriteLine("The tab bar item index is out of range");
        }

        /// Sets a UITableViewCell as target
        public static void SetTargetView(this MaterialShowcase materialShowcase, UITableView tableView, int section, int row)
        {
            var indexPath = NSIndexPath.FromRowSection(row, section);
            materialShowcase.targetView = tableView.CellAt(indexPath)?.ContentView;
            // for table viewcell, we do not need target holder (circle view)
            // therefore, set its radius = 0
            materialShowcase.targetHolderRadius = 0;
        }

        public static void Show(this MaterialShowcase materialShowcase, bool animated = true, Action completionHandler = null)
        {
            materialShowcase.InitViews();
            materialShowcase.Alpha = .0f;
            materialShowcase.containerView.AddSubview(materialShowcase);
            materialShowcase.LayoutIfNeeded();

            var scale = new nfloat(MaterialShowcase.TargetHolderRadius / (materialShowcase.backgroundView.Frame.Width * .5));
            var center = materialShowcase.backgroundView.Center;

            materialShowcase.backgroundView.Transform = CGAffineTransform.MakeScale(scale, scale); // Initial set to support animation
            materialShowcase.backgroundView.Center = materialShowcase.targetHolderView.Center;
            if (animated)
            {
                UIView.Animate(materialShowcase.aniComeInDuration, () =>
                {
                    materialShowcase.targetHolderView.Transform = CGAffineTransform.MakeScale(1, 1);
                    materialShowcase.backgroundView.Transform = CGAffineTransform.MakeScale(1, 1);
                    materialShowcase.backgroundView.Center = center;
                    materialShowcase.Alpha = 1.0f;
                }, () =>
                {
                    materialShowcase.StartAnimation();
                });
            }
            else
                materialShowcase.Alpha = 1.0f;
            // Handler user's action after showing.
            completionHandler?.Invoke();
        }
    }
}
