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
        /// <summary>
        /// Zdarzenie informujące o przesłaniu kolejnych danych.
        /// </summary>
        event Action<int> ProgressChanged;

        /// <summary>
        /// Wysłanie zdjęcia do magazynu danych.
        /// </summary>
        /// <param name="commissionID">Komisja wyborcza</param>
        /// <param name="fileName">Nazwa pliku</param>
        /// <param name="fileStream">Strumień danych</param>
        /// <returns>TRUE, jeśli operacja się powiodła, w przeciwnym wypadku FALSE</returns>
        bool UploadFile(string commissionID, string fileName, Stream fileStream);
    }
}
