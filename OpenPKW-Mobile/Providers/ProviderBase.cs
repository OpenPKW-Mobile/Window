using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace OpenPKW_Mobile.Providers
{
    abstract class ProviderBase
    {
#if DEBUG
        /// <summary>
        /// Symulacja decyzji podejmowanych przez zewnętrzną usługę logowania.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected MessageBoxResult ShowMessage(string message)
        {
            AutoResetEvent @event = new AutoResetEvent(false);
            MessageBoxResult result = MessageBoxResult.None;

            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                result = MessageBox.Show(message, GetType().Name, MessageBoxButton.OKCancel);
                @event.Set();
            });

            @event.WaitOne();

            return result;
        }
#endif

        /// <summary>
        /// Pobranie odpowiedzi HTTP z określonego URI przy użyciu metory GET.
        /// </summary>
        /// <param name="uri">Uri</param>
        /// <param name="headers">Nagłówki przesyłanego żądania HTTP</param>
        /// <returns></returns>
        /// <remarks>
        /// /// TODO: Rozważyć wykorzystanie Microsoft HTTP Client Libraries 
        /// https://www.nuget.org/packages/Microsoft.Net.Http/2.1.10
        /// </remarks>
        protected Task<string> GetResponse(Uri uri, WebHeaderCollection headers)
        {
            WebClient webClient = new WebClient();

            // przygotowanie metadanych
            webClient.Headers["Accept"] = "application/json";
            if (headers != null)
            {
                foreach (var headerKey in headers.AllKeys)
                {
                    webClient.Headers[headerKey] = headers[headerKey];
                }
            }

            // obsługa wyniku operacji
            var tcs = new TaskCompletionSource<string>();
            webClient.DownloadStringCompleted += (sender, e) =>
            {
                if (e.Error == null)
                {
                    tcs.SetResult(e.Result);
                }
                else
                {
                    tcs.SetException(new CommunicationProviderException(e.Error));
                }
            };

            // rozpoczęcie odbioru danych
            webClient.DownloadStringAsync(uri);

            return tcs.Task;
        }

        /// <summary>
        /// Wysłanie oraz odbiór danych tekstowych z określonego URI przy użyciu metody POST.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="headers"></param>
        /// <param name="requestData"></param>
        /// <returns></returns>
        protected Task<string> GetResponse(Uri uri, WebHeaderCollection headers, string requestData)
        {
            WebClient webClient = new WebClient();

            // przygotowanie metadanych
            webClient.Headers["Content-Type"] = "application/json";
            if (headers != null)
            {
                foreach (var headerKey in headers.AllKeys)
                {
                    webClient.Headers[headerKey] = headers[headerKey];
                }
            }

            // obsługa wyniku operacji
            var tcs = new TaskCompletionSource<string>();
            webClient.UploadStringCompleted += (sender, e) =>
            {
                if (e.Error == null)
                {
                    tcs.SetResult(e.Result);
                }
                else
                {
                    tcs.SetException(new CommunicationProviderException(e.Error));
                }
            };

            // rozpoczęcie wysyłania danych
            webClient.UploadStringAsync(uri, requestData);

            return tcs.Task;
        }

        /// <summary>
        /// Wysłanie danych binarnych oraz odbiór danych tekstowych z określonego URI przy użyciu metody POST.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="headers"></param>
        /// <param name="requestData"></param>
        /// <returns></returns>
        protected async Task<string> GetResponse(Uri uri, WebHeaderCollection headers, byte[] requestData)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);

            // przygotowanie metadanych
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            request.ContentLength = requestData.Length;
            request.AllowWriteStreamBuffering = false;

            // przygotowanie danych do wysłania
            using (var requestStream = await Task<Stream>.Factory.FromAsync(request.BeginGetRequestStream, request.EndGetRequestStream, request))
            {
                await requestStream.WriteAsync(requestData, 0, requestData.Length);
            }

            // wysłanie danych oraz oczekiwanie na odpowiedź
            WebResponse responseObject = await Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, request);
            
            // odbiór wyniku operacji
            var responseStream = responseObject.GetResponseStream();
            var sr = new StreamReader(responseStream);
            string received = await sr.ReadToEndAsync();

            return received;
        }

        /// <summary>
        /// Konwersja strumienia na łańcuch danych binarnych.
        /// </summary>
        /// <param name="stream">Strumień danych</param>
        /// <returns>Łańcuch danych binarnych</returns>
        protected byte[] ToBytes(Stream stream)
        {
            byte[] buffer = new byte[0];
            int bytesRead = 0;

            stream.Seek(0, SeekOrigin.Begin);

            byte[] chunk = new byte[4096];
            while ((bytesRead = stream.Read(chunk, 0, chunk.Length)) > 0)
            {
                int position = buffer.Length;
                Array.Resize(ref buffer, buffer.Length + bytesRead);
                Array.Copy(chunk, 0, buffer, position, bytesRead);
            }

            return buffer;
        }

        /// <summary>
        /// Wyjątek API.
        /// </summary>
        abstract public class ProviderException : Exception
        {
            public ProviderException(string message, Exception innerException)
                : base(message, innerException)
            {

            }
        }

        /// <summary>
        /// Wyjątek podczas komunikacji z API.
        /// </summary>
        public class CommunicationProviderException : ProviderException
        {

            public CommunicationProviderException(Exception innerException)
                : base("Błąd komunikacji z serwerem API", innerException)
            {
            }
        }

        /// <summary>
        /// Wyjątek podczas przetwarzania danych z API.
        /// </summary>
        public class DataFormatProviderException : ProviderException
        {
            public DataFormatProviderException(Exception innerException)
                : base("Błąd przetwarzania danych z API", innerException)
            {

            }
        }

    }
}
