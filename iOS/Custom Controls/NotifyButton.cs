using System;
using UIKit;
using CoreGraphics;
using Foundation;
using System.ComponentModel;

namespace SaveTheDate.iOS.CustomControls
{
    [Register("NotifyButton"), DesignTimeVisible(true)]
    public class NotifyButton : UIButton
    {
        public NotifyButton(IntPtr handle)
            : base(handle)
        {        
            Initialize();
        }

        public void Initialize()
        {
            this.SetTitleColor(UIColor.White, UIControlState.Normal);
            this.SetTitleColor(UIColor.FromRGB(73, 96, 134), UIControlState.Disabled);
        }

        public override bool Enabled
        {
            get
            {
                return base.Enabled;
            }
            set
            {
                borderColor = value == true ? UIColor.White.CGColor : UIColor.FromRGB(73, 96, 134).CGColor;
                SetNeedsDisplay();
                base.Enabled = value;
            }
        }

        CGColor borderColor = UIColor.White.CGColor;


        public override void Draw(CoreGraphics.CGRect rect)
        {
            Layer.BorderColor = borderColor;
            Layer.BorderWidth = 1;
            Layer.CornerRadius = Frame.Height / 2;
        }
    }
}

