using OpenPKW_Mobile.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace OpenPKW_Mobile.Services
{
    interface IPhotoService
    {
        event Action<int> UploadProgress;
        event Action UploadCompleted;
        event Action<string> UploadRejected;

        void BeginUpload(PhotoEntity entity);
    }
}
