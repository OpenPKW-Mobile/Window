using OpenPKW_Mobile.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OpenPKW_Mobile.Providers
{
    class OpwStorageProvider : ProviderBase, IStorageProvider
    {
        const string URI_PREPARE = "http://openpkw.nazwa.pl/api/image-metadata.php";
        
        #region Komunikaty 

        [DataContract]
        struct PrepareRequestData
        {
            [DataMember(Name = "pkwId")]
            public string CommissionID { get; set; }

            [DataMember(Name = "fileName")]
            public string FileName { get; set; }
        }

        [DataContract]
        struct PrepareResponseData
        {
            [DataMember(Name = "uploadUrl")]
            public string UploadUrl { get; set; }
        }

        [DataContract]
        struct UploadResponseData
        {
            [DataMember(Name = "bytesCount")]
            public int BytesCount { get; set; }
        }
     
        #endregion Komunikaty

        #region Implementacja IStoreProvider

        public event Action<int> ProgressChanged;

        bool IStorageProvider.UploadFile(string commissionID, string fileName, Stream fileStream)
        {
            try
            {
                // wysłanie metadanych zdjęcia
                // serwer powinien zwrócić informację, gdzie wysłać dane zdjęcia
                var prepareRequest = new PrepareRequestData() 
                { 
                    CommissionID = commissionID, 
                    FileName = fileName 
                };
                var prepareUri = new Uri(URI_PREPARE);
                var prepareTask = GetResponse(prepareUri, null, 
                    JsonHelper.ToJson<PrepareRequestData>(prepareRequest));
                prepareTask.Wait();
                var prepareResponse = JsonHelper.FromJson<PrepareResponseData>(prepareTask.Result);

                // tymczasowe rozwiązanie raportowania o postepie operacji
                if (ProgressChanged != null)
                    ProgressChanged(50);

                // wysłanie na wskazany adres danych zdjęcia
                var uploadUri = new Uri(prepareResponse.UploadUrl);
                var uploadData = ToBytes(fileStream);
                var uploadTask = GetResponse(uploadUri, null, uploadData);
                uploadTask.Wait();
                var uploadResponse = JsonHelper.FromJson<UploadResponseData>(uploadTask.Result);

                // tymczasowe rozwiązanie raportowania o postepie operacji
                if (ProgressChanged != null)
                    ProgressChanged(100);

                // zgłoszony rozmiar zdjęcia powinien odpowiadać stanowi faktycznemu
                return (uploadResponse.BytesCount == uploadData.Length);
            }
            catch
            {
                return false;
            }
        }

        #endregion Implementacja IStoreProvider
    }
}
