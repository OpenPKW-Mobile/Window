using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace OpenPKW_Mobile.Backend.Services
{
    [ServiceContract]
    public interface IStorageService
    {
        /// <summary>
        /// Wysłanie metadanych zdjęcia. Pobranie adresu dla danych.
        /// </summary>
        /// <param name="commissionID">Identyfikator komisji</param>
        /// <param name="fileName">Nazwa pliku</param>
        /// <returns>Adres, gdzie powinny być przesłane dane</returns>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "prepare", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        [return: MessageParameter(Name = "uploadUrl")]
        Uri Prepare(string commissionID, string fileName);

        /// <summary>
        /// Wysłanie danych zdjęcia.
        /// </summary>
        /// <param name="imageID">Identyfikator żądania</param>
        /// <param name="stream">Strumień danych</param>
        /// <returns>Liczba odebranych bajtów</returns>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "upload?id={imageID}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        [return: MessageParameter(Name = "bytesCount")]
        long Upload(string imageID, Stream stream);
    }
}
