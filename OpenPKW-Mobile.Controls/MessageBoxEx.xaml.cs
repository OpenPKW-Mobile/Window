using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OpenPKW_Mobile.Controls
{
    public partial class MessageBoxEx : CustomMessageBox
    {
        public event Action LeftButtonPressed;
        public event Action RightButtonPressed;

        public MessageBoxEx()
        {
            this.Opacity = 0.8;
            this.Margin = new Thickness(20);
            this.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            this.Dismissed += MessageBoxEx_Dismissed;
        }

        void MessageBoxEx_Dismissed(object sender, DismissedEventArgs e)
        {
            switch (e.Result)
            {
                case CustomMessageBoxResult.LeftButton:
                    if (LeftButtonPressed != null)
                        LeftButtonPressed();
                    break;
                case CustomMessageBoxResult.RightButton:
                    if (RightButtonPressed != null)
                        RightButtonPressed();
                    break;
            }
        }
    }
}
