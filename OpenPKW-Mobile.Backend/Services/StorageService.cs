using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace OpenPKW_Mobile.Backend.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class StorageService : ServiceBase, IStorageService
    {
        const string PREPARE_ACCEPT = "Prepare.Accept";
        const string PREPARE_ERROR = "Prepare.Error";
        const string FILENAME_PATTERN = "Filename.Pattern";
        const string UPLOAD_ACCEPT = "Upload.Accept";
        const string UPLOAD_ERROR = "Upload.Error";

        const string VALUE_NO = "0";
        const string VALUE_YES = "1";
        const string PATTERN_ALL = ".*";
        const string OUTPUT_DIRECTORY = "./Storage";

        private List<string> _fileNames = new List<string>();

        public StorageService()
        {
            AddParam(PREPARE_ACCEPT, VALUE_YES);
            AddParam(PREPARE_ERROR, VALUE_NO);
            AddParam(FILENAME_PATTERN, PATTERN_ALL);
            AddParam(UPLOAD_ACCEPT, VALUE_YES);
            AddParam(UPLOAD_ERROR, VALUE_NO);
        }

        #region Implementacja IStorageService

        /// <summary>
        /// Odebranie metadanych zdjęcia.
        /// </summary>
        /// <param name="commissionID">Identyfikator komisji</param>
        /// <param name="fileName">Nazwa zdjęcia</param>
        /// <returns>
        /// Adres, na który powinno być przesłane zdjęcie. 
        /// Zawiera parametr "id" będący identyfikatorem żądania.
        /// </returns>
        public Uri Prepare(string commissionID, string fileName)
        {
            Console.WriteLine(Helper.GetCurrentRequest());
            Thread.Sleep(1000);

            // czy użytkownik ma odpowiednie uprawnienia?
            // serwer może odrzucić żądanie, jeśli użytkownik jest nieznany
            // błąd 403
            if (GetParam(PREPARE_ACCEPT) == VALUE_YES)
            {
                string pattern = GetParam(FILENAME_PATTERN);
                Regex expression = new Regex(pattern);

                // czy nastąpił błąd podczas wykonywania żądania?
                // serwer może zgłosić błąd 500
                if (GetParam(PREPARE_ERROR) == VALUE_YES)
                {
                    Helper.SetResponseStatus(HttpStatusCode.InternalServerError);
                    return null;
                }
                // serwer może odrzucić niektóre poprawne zapytania
                // błąd 403
                else if (expression.IsMatch(fileName))
                {
                    Helper.SetResponseStatus(HttpStatusCode.OK);
                    Uri baseUri = Helper.GetBaseAddress();
                    return new Uri(baseUri, "upload?id=" + getRequestID(fileName));
                }
                else
                {
                    Helper.SetResponseStatus(HttpStatusCode.Forbidden);
                    return null;
                }
            }
            else
            {
                Helper.SetResponseStatus(HttpStatusCode.Forbidden);
                return null;
            }
        }

        /// <summary>
        /// Odebranie zdjęcia.
        /// </summary>
        /// <param name="id">Identyfikator żądania</param>
        /// <param name="stream">Strumień binarnych danych</param>
        /// <returns>Liczba odebranych bajtów danych.</returns>
        public long Upload(string id, System.IO.Stream stream)
        {
            Console.WriteLine(Helper.GetCurrentRequest());
            Thread.Sleep(1000);

            // czy użytkownik ma odpowiednie uprawnienia?
            // serwer może odrzucić żądanie, jeśli użytkownik jest nieznany
            // błąd 403
            if (GetParam(UPLOAD_ACCEPT) == VALUE_YES)
            {
                // symulacja zerwania połączenia w trakcie transmisji danych
                // serwer zwraca niepełną liczbę danych
                if (GetParam(UPLOAD_ERROR) == VALUE_YES)
                {
                    Helper.SetResponseStatus(HttpStatusCode.InternalServerError);
                    return getStreamLength(stream) / 4;
                }
                else
                {
                    // serwer może odrzucić żądanie, jeśli wcześniej nie zostały wysłane metadane
                    // błąd 403
                    string fileName = getFileName(int.Parse(id));
                    if (fileName != null)
                    {
                        Helper.SetResponseStatus(HttpStatusCode.OK);
                        return writeStream(fileName, stream);
                    }
                    else
                    {
                        Helper.SetResponseStatus(HttpStatusCode.Forbidden);
                        return 0;
                    }
                }
            }
            else
            {
                Helper.SetResponseStatus(HttpStatusCode.Forbidden);
                return 0;
            }
        }

        #endregion Implementacja IStorageService

        #region Funkcje pomocnicze

        /// <summary>
        /// Określenie liczby bajtów w strumieniu.
        /// Strumień nie musi posiadać informacji o długości.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public long getStreamLength(Stream stream)
        {
            byte[] readBuffer = new byte[4096];
            long bytesRead;
            long totalBytesRead = 0;

            while ((bytesRead = stream.Read(readBuffer, 0, 4096)) > 0)
            {
                totalBytesRead += bytesRead;
            }

            return totalBytesRead;
        }

        /// <summary>
        /// Zapisanie strumienia do pliku.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public long writeStream(string fileName, Stream stream)
        {
            long totalBytesRead = 0;

            if (Directory.Exists(OUTPUT_DIRECTORY) == false)
                Directory.CreateDirectory(OUTPUT_DIRECTORY);

            string filePath = Path.Combine(OUTPUT_DIRECTORY, fileName);
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                stream.CopyTo(fs);
                totalBytesRead = fs.Length;
            }

            return totalBytesRead;
        }

        /// <summary>
        /// Zwraca identyfikator żądania na podstawie nazwy pliku.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private int getRequestID(string fileName)
        {
            var fileNames = this._fileNames;
            int fileIndex = fileNames.IndexOf(fileName);

            if (fileIndex == -1)
            {
                fileNames.Add(fileName);
                fileIndex = fileNames.Count - 1;
            }

            return fileIndex;
        }

        /// <summary>
        /// Zwraca nazwę pliku na podstawie identyfikatora żądania.
        /// </summary>
        /// <param name="requestID"></param>
        /// <returns></returns>
        private string getFileName(int requestID)
        {
            var fileNames = this._fileNames;
            string fileName = null; ;

            if (requestID < fileNames.Count)
            {
                fileName = fileNames[requestID];
                fileName = fileName + ".jpg";
            }
 
            return fileName;
        }

        #endregion Funkcje pomocnicze
    }
}
