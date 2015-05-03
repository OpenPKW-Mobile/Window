using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;

namespace OpenPKW_Mobile.Providers
{
    abstract class ProviderBase
    {
#if DEBUG
        /// <summary>
        /// Symulacja decyzji podejmowanych przez zewnętrzną usługę logowania.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected MessageBoxResult ShowMessage(string message)
        {
            AutoResetEvent @event = new AutoResetEvent(false);
            MessageBoxResult result = MessageBoxResult.None;

            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                result = MessageBox.Show(message, GetType().Name, MessageBoxButton.OKCancel);
                @event.Set();
            });

            @event.WaitOne();

            return result;
        }
#endif
    }
}
