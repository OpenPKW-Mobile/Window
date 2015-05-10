using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPKW_Mobile.Providers
{
    interface IStorageProvider
    {
        event Action<int> ProgressChanged;

        bool UploadFile(string name, Stream stream);
    }
}
