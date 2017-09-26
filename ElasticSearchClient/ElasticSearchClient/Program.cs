using Nancy.Hosting.Self;
using System;
using Nancy;
using System.Configuration;

namespace ElasticSearchClient
{
    class Program
    {
        private readonly NancyHost _nancy;

        public Program()
        {
            var uri = new Uri(ConfigurationSettings.AppSettings["NancyAddress"]);
            var hostConfigs = new HostConfiguration { UrlReservations = { CreateAutomatically = true } };
            _nancy = new NancyHost(uri, new DefaultNancyBootstrapper(), hostConfigs);
        }

        private void Start()
        {
            _nancy.Start();
            Console.WriteLine($"Started listennig address {ConfigurationSettings.AppSettings["NancyAddress"]}");
            Console.ReadKey();
            _nancy.Stop();
        }

        static void Main(string[] args)
        {
            var p = new Program();
            p.Start();
        }
    }
}
