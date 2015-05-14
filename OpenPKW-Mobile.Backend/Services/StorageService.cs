using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace OpenPKW_Mobile.Backend.Services
{
    public class StorageService : IStorageService
    {
        public Uri Prepare(string commissionID, string fileName)
        {
            Console.WriteLine(Helper.GetCurrentRequest());

            if (commissionID == "1")
            {
                Helper.SetResponseStatus(HttpStatusCode.OK);
                Uri baseUri = Helper.GetBaseAddress();
                return new Uri(baseUri, "upload?id=" + fileName);
            }
            else
            {
                Helper.SetResponseStatus(HttpStatusCode.InternalServerError);
                return null;
            }
        }

        public void Upload(string id, System.IO.Stream stream)
        {
            Console.WriteLine(Helper.GetCurrentRequest());
            /*
            byte[] data = Helper.ReadMessageStream(stream);
            string encodedData = String.Join(" ", data.Select(item => String.Format("{0:X2}", item)));
            Console.WriteLine(encodedData);
            */
            if (id == "Strona1")
            {
                Helper.SetResponseStatus(HttpStatusCode.OK);
            }
            else
            {
                Helper.SetResponseStatus(HttpStatusCode.InternalServerError);
            }
        }
    }
}
