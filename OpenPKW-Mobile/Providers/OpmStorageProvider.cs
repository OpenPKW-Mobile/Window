using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace OpenPKW_Mobile.Providers
{
    class OpmStorageProvider : ProviderBase, IStorageProvider
    {
        public event Action<int> ProgressChanged;

        bool IStorageProvider.UploadFile(string name, System.IO.Stream stream)
        {
            var message = String.Format("Czy odebrać plik o nazwie '{0}' ?", name);
            var result = ShowMessage(message);

            if (result == MessageBoxResult.OK)
            {
                // symuluje opóżnienia w komunikacji z zewnętrzną usługą
                for (int index = 0; index < 10; index++)
                {
                    Thread.Sleep(300);
                    if (ProgressChanged != null)
                        ProgressChanged(10 * (index + 1));
                }
            }
            else
            {
                Thread.Sleep(1000);
            }

            return (result == MessageBoxResult.OK);
        }

    }
}
