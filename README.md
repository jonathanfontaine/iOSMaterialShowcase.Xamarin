# iOSMaterialShowcase.Xamarin
Here is my quick Xamarin iOS translation of the **material-showcase-ios** classes. All credits go to aromajoin, I just translated his code from swift to C#.

For more informations, please visite his repo : https://github.com/aromajoin/material-showcase-ios

## Show a showcase :
```cs
public class TestViewController : UIViewController
{
    private UIBarButtonItem target;
    
    public override void ViewDidLoad()
    {
        base.ViewDidLoad();
        
        // Place a button in the UITabBar
        target = new UIBarButtonItem(UIBarButtonSystemItem.Add, (sender, args) => Console.WriteLine("bar button clicked"));
        NavigationItem.SetRightBarButtonItem(target, true);
    }
    
    public override void ViewDidAppear()
    {
        base.ViewDidAppear();

        // Instantiate a new MaterialShowcase
        var showcase = new MaterialShowcase();
        showcase.primaryText = "Hello";
        showcase.secondaryText = "I'm a showcase";

        // Set the UIBarButtonItem as the target view
        showcase.SetTargetView(target);

        // Show the showcase
        showcase.Show();
    }
}
```
## Execute some code when the showcase is dismissed :
```cs
public override void ViewDidAppear()
{
    ...
    
    // Define an action when the showcase is dismissed and set his delegate
    Action showcaseDidDismiss = () => Console.WriteLine("showcase dismissed");
    showcase.showcaseDelegate = new ShowcaseDelegate(showcaseDidDismiss);
}
        
// Delegate to execute code when showcase is dismissed
private class ShowcaseDelegate : IMaterialShowcaseDelegate
{
    private Action ShowcaseDidDismiss;

    public ShowcaseDelegate(Action showcaseDidDismiss)
    {
        ShowcaseDidDismiss = showcaseDidDismiss;
    }

    public void ShowCaseDidDismiss()
    {
        ShowcaseDidDismiss?.Invoke();
    }

    public void ShowCaseWillDismiss()
    {
    }
}
```
