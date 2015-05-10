using OpenPKW_Mobile.Entities;
using OpenPKW_Mobile.Providers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OpenPKW_Mobile.Services
{
    class PhotoService : ServiceBase, IPhotoService
    {
        /// <summary>
        /// Zestaw danych przekazywanych pomiędzy wątkami.
        /// </summary>
        struct WorkerData
        {
            public IStorageProvider StorageProvider;
            public PhotoEntity Photo;
        }

        WorkerHandle Upload;

        public event Action<int> UploadProgress;
        public event Action UploadCompleted;
        public event Action<string> UploadRejected;

        /// <summary>
        /// Dostawca usługi przechowywania plików.
        /// </summary>
        private IStorageProvider _provider;

        public PhotoService(IStorageProvider provider)
        {
            this._provider = provider;
        }

        void IPhotoService.BeginUpload(PhotoEntity photo)
        {
            IStorageProvider provider = this._provider;
            WorkerContext context = new WorkerContext()
            {
                DoWork = uploadProcess,
                ProgressChanged = uploadProgress,
                RunWorkerCompleted = uploadCompleted,
                UserData = new WorkerData()
                {
                    StorageProvider = provider,
                    Photo = photo
                }
            };

            Upload = Begin(context);

            // tutaj cały czas trwa procedura przesyłania zdjęcia
            // ...
        }

        private void uploadProcess(object sender, DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker)sender;
            var provider = ((WorkerData)e.Argument).StorageProvider;
            var photo = ((WorkerData)e.Argument).Photo;

            BitmapSource bitmapSource = photo.Image as BitmapSource;
            if (bitmapSource == null)
            {
                throw new PhotoException(
                    PhotoException.ErrorReason.InvalidBitmap);
            }

            provider.ProgressChanged += delegate(int percent)
            {
                if (worker.IsBusy)
                    worker.ReportProgress(percent);
            };

            WriteableBitmap bitmap = null;
            using (AutoResetEvent ev = new AutoResetEvent(false))
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    bitmap = new WriteableBitmap(bitmapSource);
                    ev.Set();
                });
                ev.WaitOne();
            }

            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.SaveJpeg(stream, bitmap.PixelWidth, bitmap.PixelHeight, 0, 100);

                bool result = provider.UploadFile(photo.Name, stream);                
                if (result == false)
                {
                    throw new PhotoException(
                        PhotoException.ErrorReason.CannotUploadFile);
                }
            }
        }

        private void uploadProgress(object sender, ProgressChangedEventArgs e)
        {
            if (UploadProgress != null)
                UploadProgress(e.ProgressPercentage);
        }

        private void uploadCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                // poinformowanie słuchaczy o problemie
                if (UploadRejected != null)
                    UploadRejected(e.Error.Message);
            }
            else
            {
                if (UploadCompleted != null)
                    UploadCompleted();
            }
        }

    }
}
