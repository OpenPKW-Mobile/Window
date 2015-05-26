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
    partial class PhotoService : ServiceBase, IPhotoService
    {
        /// <summary>
        /// Dostawca usługi przechowywania plików.
        /// </summary>
        private IStorageProvider _provider;

        /// <summary>
        /// Konstruktor klasy.
        /// </summary>
        /// <param name="provider">Dostawca usługi przechowywania plików</param>
        public PhotoService(IStorageProvider provider)
        {
            this._provider = provider;
        }
    }
}
