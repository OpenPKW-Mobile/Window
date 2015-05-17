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
    /// <summary>
    /// Usługa przesyłania zdjęcia do zewnętrzego magazynu danych.
    /// </summary>
    class PhotoService : ServiceBase, IPhotoService
    {
        /// <summary>
        /// Zestaw danych przekazywanych pomiędzy wątkami.
        /// </summary>
        struct WorkerData
        {
            public IStorageProvider StorageProvider;
            public CommissionEntity Commission;
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

        void IPhotoService.BeginUpload(CommissionEntity commission, PhotoEntity photo)
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
                    Commission = commission,
                    Photo = photo
                }
            };

            Upload = Begin(context);

            // tutaj cały czas trwa procedura przesyłania zdjęcia
            // ...
        }

        /// <summary>
        /// Obsługa przesyłania zdjęcia.
        /// </summary>
        /// <param name="sender">Nadawca zdarzenia</param>
        /// <param name="e">Parametry operacji</param>
        private void uploadProcess(object sender, DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker)sender;
            var provider = ((WorkerData)e.Argument).StorageProvider;
            var commission = ((WorkerData)e.Argument).Commission;
            var photo = ((WorkerData)e.Argument).Photo;

            // trzeba się upewnić, że zostało wskazane zdjęcie do wysłania
            BitmapSource bitmapSource = photo.Image as BitmapSource;
            if (bitmapSource == null)
            {
                throw new PhotoException(
                    PhotoException.ErrorReason.InvalidBitmap);
            }

            // podobno wszystkie bitmapy są tworzone w wątku GUI
            // należy przenieść zawartość zdjęcia do lokalnego wątku
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

            // raportowanie postępu wysyłania
            // zdarzenia pochodzą od dostawcy usługi
            provider.ProgressChanged += delegate(int percent)
            {
                if (worker.IsBusy)
                    worker.ReportProgress(percent);
            };

            // przesłanie zdjęcia przy pomocy dostawcy usługi
            using (MemoryStream stream = new MemoryStream())
            {
                // konwersja do formatu JPEG
                bitmap.SaveJpeg(stream, bitmap.PixelWidth, bitmap.PixelHeight, 0, 100);

                // przesłanie zdjęcia
                // dostawca nie generuje wyjątków; błąd transmisji zwracany jest jako wynik operacji
                bool result = provider.UploadFile(commission.Id, photo.Name, stream);                
                if (result == false)
                {
                    throw new PhotoException(
                        PhotoException.ErrorReason.CannotUploadFile);
                }
            }
        }

        /// <summary>
        /// Obsługa raportowania postępu operacji.
        /// </summary>
        /// <param name="sender">Nadawca zdarzenia</param>
        /// <param name="e">Postęp operacji</param>
        private void uploadProgress(object sender, ProgressChangedEventArgs e)
        {
            if (UploadProgress != null)
                UploadProgress(e.ProgressPercentage);
        }

        /// <summary>
        /// Obsługa wyniku operacji.
        /// </summary>
        /// <param name="sender">Nadawca zdarzenia</param>
        /// <param name="e">Wynik operacji</param>
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
