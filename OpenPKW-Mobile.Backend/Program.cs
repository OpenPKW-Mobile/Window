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
        static AuthenticationService Authentication;
        static ElectionService Election;
        static StorageService Storage;

        static void Main(string[] args)
        {
            Console.Title = "Exchange Service Hosting Server";
            ServiceHost hostAuth = null;
            ServiceHost hostElection = null;
            ServiceHost hostStorage = null;

            {
                Authentication = new AuthenticationService();
                string httpBaseAddress = Properties.Settings.Default.AuthenticationBaseAddress;
                Uri[] baseAddress = new Uri[] { new Uri(httpBaseAddress) };
                hostAuth = new ServiceHost(Authentication, baseAddress);
                hostAuth.Open();
            }
            {
                Election = new ElectionService();
                string httpBaseAddress = Properties.Settings.Default.ElectionBaseAddress;
                Uri[] baseAddress = new Uri[] { new Uri(httpBaseAddress) };
                hostElection = new ServiceHost(Election, baseAddress);
                hostElection.Open();
            }
            {
                Storage = new StorageService();
                string httpBaseAddress = Properties.Settings.Default.StorageBaseAddress;
                Uri[] baseAddress = new Uri[] { new Uri(httpBaseAddress) };
                hostStorage = new ServiceHost(Storage, baseAddress);
                hostStorage.Open();              
            }

            do
            {
                var service = menuServices();
                if (service != null)
                    menuConfig(service);
                else break;
            }
            while (true);

            hostAuth.Close();
            hostElection.Close();
            hostStorage.Close();
        }

        private static ServiceBase menuServices()
        {
            ServiceBase service = null;

            Console.WriteLine("Dostępne serwisy:");
            Console.WriteLine("\t1. Uwierzytelnianie");
            Console.WriteLine("\t2. Wyniki wyborów");
            Console.WriteLine("\t3. Przechowywanie zdjęć");
            Console.WriteLine();
            
            Console.Write("Wybierz serwis lub 'q' aby zatrzymać serwer: ");

            do
            {
                var key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.D1: service = Authentication; break;
                    case ConsoleKey.D2: service = Election; break;
                    case ConsoleKey.D3: service = Storage; break;
                    case ConsoleKey.Q: return null;
                }
            }
            while (service == null);

            Console.WriteLine();
            Console.WriteLine();

            return service;
        }

        private static void menuConfig(ServiceBase service)
        {
            Console.WriteLine("Dostępne parametry:");
            Console.WriteLine(service.GetMenu());
            Console.WriteLine("Podaj nową wartość w formacie 'parametr = wartość'");
            Console.WriteLine("Naciśnij ENTER, kiedy chcesz zrezygnować");
            Console.WriteLine();
            
            Console.Write(">> ");

            string line = Console.ReadLine();
            if (!String.IsNullOrWhiteSpace(line))
            {
                string[] parts = line.Split('=');
                if (parts.Length == 2)
                {
                    service.SetParam(parts[0], parts[1]);
                    
                    bool success = (service.GetParam(parts[0]) == parts[1]);
                    Console.WriteLine(success ? "Zapisano" : "Błąd");
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("Błąd");
                    Console.WriteLine();
                }
            }
        }
    }
}
