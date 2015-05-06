using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OpenPKW_Mobile.Services
{

    /// <summary>
    /// Klient API OpenPKW
    /// </summary>
    public class OpenPkwApiClient
    {

        public async Task<string> GetCommissionsAsync()
        {
            WebHeaderCollection headers = new WebHeaderCollection();
            headers["X-OPW-API-token"] = "d171794c5c1f7a50aeb8f7056ab84a4fbcd6fbd594b1999bddaefdd03efc0591";
            headers["X-OPW-login"] = "admin";
            headers["X-OPW-token"] = "bacd5d2591d75daaa4fe191fca0345a7129e5827b7d7dadaea8c74c3972cb7";
            string jsonResponse = await GetResponse(new Uri("http://91.250.114.134:8080/opw/service/wynik/complete"),
                headers);
            return jsonResponse;
            //TODO: konwersja json->.NET
        }

        /// <summary>
        /// Pobranie tokena dla określonego użytkownika
        /// </summary>
        /// <param name="login">użytkownik</param>
        /// <param name="password">hasło</param>
        /// <returns></returns>
        public async Task<string> GetUserToken(string login, string password)
        {
            WebHeaderCollection headers = new WebHeaderCollection();
            headers["X-OPW-login"] = login;
            headers["X-OPW-password"] = password;
            string jsonResponse = await GetResponse(new Uri("http://91.250.114.134/rest-api/service/user/login"),
                headers);
            try
            {
                var userJson = JsonConvert.DeserializeObject(jsonResponse);
                return ((userJson as Newtonsoft.Json.Linq.JObject)["token"] as Newtonsoft.Json.Linq.JValue).Value as string;
            }
            catch (Exception ex)
            {
                throw new OpwApiDataFormatException(ex);
            }
        }

        /// <summary>
        /// Pobranie odpowiedzi HTTP z określonego URI
        /// TODO: Rozważyć wykorzystanie Microsoft HTTP Client Libraries 
        /// https://www.nuget.org/packages/Microsoft.Net.Http/2.1.10
        /// </summary>
        /// <param name="uri">Uri</param>
        /// <param name="headers">Nagłówki przesyłanego żądania HTTP </param>
        /// <returns></returns>
        private Task<string> GetResponse(Uri uri, WebHeaderCollection headers)
        {
            WebClient webClient = new WebClient();
            webClient.Headers["Accept"] = "application/json";
            if (headers != null)
            {
                foreach (var headerKey in headers.AllKeys)
                {
                    webClient.Headers[headerKey] = headers[headerKey];
                }
            }
            var tcs = new TaskCompletionSource<string>();
            webClient.DownloadStringCompleted += (sender, e) =>
                {
                    if (e.Error == null)
                    {
                        tcs.SetResult(e.Result);
                    }
                    else
                    {
                        tcs.SetException(new OpwApiCommunicationException(e.Error));
                    }
                };
            webClient.DownloadStringAsync(uri);
            return tcs.Task;
        }
    }


    /// <summary>
    /// Wyjątek API
    /// </summary>
    abstract public class OpwApiException : Exception
    {
        public OpwApiException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }

    /// <summary>
    /// Wyjątek podczas komunikacji z API
    /// </summary>
    public class OpwApiCommunicationException : OpwApiException
    {

        public OpwApiCommunicationException(Exception innerException)
            : base("Błąd komunikacji z serwerem API. Zobacz więcej w InnerException", innerException)
        {
        }
    }

    /// <summary>
    /// Wyjątek podczas przetwarzania danych z API
    /// </summary>
    public class OpwApiDataFormatException : OpwApiException
    {
        public OpwApiDataFormatException(Exception innerException)
            : base("Błąd przetwarzania danych z API. Zobacz więcej w InnerException", innerException)
        {

        }
    }


}
