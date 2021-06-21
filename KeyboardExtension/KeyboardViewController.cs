using System;

using ObjCRuntime;
using Foundation;
using UIKit;
using Binding;

namespace KeyboardExtension
{
    public partial class KeyboardViewController : UIInputViewController
    {
        UIButton nextKeyboardButton;

        protected KeyboardViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void UpdateViewConstraints()
        {
            base.UpdateViewConstraints();

            // Add custom view sizing constraints here
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Perform custom UI setup here
            nextKeyboardButton = new UIButton(UIButtonType.System);

            nextKeyboardButton.SetTitle("Next Keyboard", UIControlState.Normal);
            nextKeyboardButton.SizeToFit();
            nextKeyboardButton.TranslatesAutoresizingMaskIntoConstraints = false;

            nextKeyboardButton.AddTarget(this, new Selector("advanceToNextInputMode"), UIControlEvent.TouchUpInside);

            View.AddSubview(nextKeyboardButton);

            var nextKeyboardButtonLeftSideConstraint = NSLayoutConstraint.Create(nextKeyboardButton, NSLayoutAttribute.Left, NSLayoutRelation.Equal, View, NSLayoutAttribute.Left, 1.0f, 0.0f);
            var nextKeyboardButtonBottomConstraint = NSLayoutConstraint.Create(nextKeyboardButton, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, View, NSLayoutAttribute.Bottom, 1.0f, 0.0f);
            View.AddConstraints(new[] { nextKeyboardButtonLeftSideConstraint, nextKeyboardButtonBottomConstraint });
        }

        class callback : KeyboardControllerCallback
        {

            KeyboardViewController controller;

            public callback(KeyboardViewController controller)
            {
                this.controller = controller;
            }
            public override void OnKeyPadPressedWithText(string text)
            {
                controller.TextDocumentProxy.InsertText(text);

            }

            public override void TextLeftOfCusorWithText(string text)
            {

            }

            public override void TextRightOfCusorWithText(string text)
            {

            }
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            //show view from binding library
            var proxy = new KeyboardProxy();

            proxy.OnloginWithViewController(this, new callback(this));
        }

        public override void TextWillChange(IUITextInput textInput)
        {
            // The app is about to change the document's contents. Perform any preparation here.
        }

        public override void TextDidChange(IUITextInput textInput)
        {
            // The app has just changed the document's contents, the document context has been updated.
            UIColor textColor = null;

            if (TextDocumentProxy.KeyboardAppearance == UIKeyboardAppearance.Dark)
            {
                textColor = UIColor.White;
            }
            else
            {
                textColor = UIColor.Black;
            }

            nextKeyboardButton.SetTitleColor(textColor, UIControlState.Normal);
        }
    }
}
