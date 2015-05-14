using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace OpenPKW_Mobile.Backend
{
    static class Helper
    {
        public static string GetCurrentRequest()
        {
            StringBuilder sb = new StringBuilder();
            IncomingWebRequestContext request = WebOperationContext.Current.IncomingRequest;
            WebHeaderCollection headers = request.Headers;
            
            sb.AppendLine(request.Method + " " + request.UriTemplateMatch.RequestUri.AbsolutePath);
            foreach (string headerName in headers.AllKeys)
            {
                sb.AppendLine(headerName + ": " + headers[headerName]);
            }

            sb.AppendLine(OperationContext.Current.RequestContext.RequestMessage.ToString());

            return sb.ToString();
        }

        public static string GetRequestHeader(string key)
        {
            IncomingWebRequestContext request = WebOperationContext.Current.IncomingRequest;
            WebHeaderCollection headers = request.Headers;

            return headers[key];
        }

        public static void SetResponseStatus(HttpStatusCode code)
        {
            WebOperationContext context = WebOperationContext.Current;
            context.OutgoingResponse.StatusCode = code;
        }

        public static Uri GetBaseAddress()
        {
            return OperationContext.Current.IncomingMessageHeaders.To;
        }

        public static byte[] ReadMessageStream(System.IO.Stream stream)
        {
            byte[] buffer = new byte[1000];
            byte[] data = new byte[0];
            int bytesRead;

            do
            {
                bytesRead = stream.Read(buffer, 0, buffer.Length);

                int position = data.Length;
                Array.Resize(ref data, data.Length + bytesRead);
                Array.Copy(buffer, 0, data, position, bytesRead);
            } 
            while (bytesRead > 0);

            return data;
        }
    }
}
