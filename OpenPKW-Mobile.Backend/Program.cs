using OpenPKW_Mobile.Backend.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace OpenPKW_Mobile.Backend
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Exchange Service Hosting Server";
            ServiceHost hostAuth = null;
            ServiceHost hostStorage = null;
            ServiceHost hostElection = null;

            {
                Type serviceType = typeof(AuthenticationService);
                string httpBaseAddress = Properties.Settings.Default.AuthenticationBaseAddress;
                Uri[] baseAddress = new Uri[] { new Uri(httpBaseAddress) };
                hostAuth = new ServiceHost(serviceType, baseAddress);
                hostAuth.Open();
            }
            {
                Type serviceType = typeof(StorageService);
                string httpBaseAddress = Properties.Settings.Default.StorageBaseAddress;
                Uri[] baseAddress = new Uri[] { new Uri(httpBaseAddress) };
                hostStorage = new ServiceHost(serviceType, baseAddress);
                hostStorage.Open();
            }
            {
                Type serviceType = typeof(ElectionService);
                string httpBaseAddress = Properties.Settings.Default.ElectionBaseAddress;
                Uri[] baseAddress = new Uri[] { new Uri(httpBaseAddress) };
                hostElection = new ServiceHost(serviceType, baseAddress);
                hostElection.Open();
            }

            printServiceConfig();

            Console.WriteLine("Nacisnij dowolny klawisz aby zatrzymac serwer...");
            Console.ReadKey();

            hostAuth.Close();
            hostStorage.Close();
            hostElection.Close();
        }

        private static void printServiceConfig()
        {
            //string info = Ipp.Siso.Exchange.Service.Config.ToString();
            //Console.WriteLine(info);
        }
    }
}
