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
        /// <summary>
        /// Zdarzenie informujące o przesłaniu kolejnych danych.
        /// </summary>
        event Action<int> UploadProgress;

        /// <summary>
        /// Zdarzenie informujące o zakończeniu wysyłania zdjęcia.
        /// </summary>
        event Action UploadCompleted;

        /// <summary>
        /// Zdarzenie informujące o błędzie podczas wysyłania zdjęcia.
        /// Zdjęcie nie zostało poprawnie przesłane, należy ponowić transmisję danych.
        /// </summary>
        event Action<string> UploadRejected;

        /// <summary>
        /// Rozpoczęcie wysyłania zdjęcia.
        /// Procedura jest wykonywana w tle, a jej wynik zgłaszany przez zdarzenia.
        /// </summary>
        /// <param name="commission"></param>
        /// <param name="entity"></param>
        void BeginUpload(CommissionEntity commission, PhotoEntity entity);
    }
}
