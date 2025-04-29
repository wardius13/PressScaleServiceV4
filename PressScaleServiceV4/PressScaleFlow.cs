using S7.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PlcReader.Services;
using PressScaleServiceV4.Services;


namespace PressScaleServiceV4
{
    public class PressScaleFlow
    {
        public PlcService PlcService;
        private CancellationTokenSource tokenSource;
        private PrintToTxtService printToTxtService;
        private SqlService sqlService;


        public float gewicht;
        public int aantalBalen;
        public bool magVerwerken;
        public int materiaalID;
        public float balenGewicht;
        private bool _isPrinting = false;  // Onthoudt of we al geprint hebben


        public PressScaleFlow()
        {
            ConfigureServices();
        }



        private void ConfigureServices()
        {

            PlcService = new PlcService("192.168.0.1");
            printToTxtService = new PrintToTxtService(); // standaard logbestand
            sqlService = new SqlService();
        }

        public void Start()
        {
            PlcService.Connect();
            tokenSource = new CancellationTokenSource();
            Task.Run(() => PressScaleLoop(tokenSource.Token));

        }

        public void Stop()
        {
            tokenSource?.Cancel(); // zorgt ervoor dat de loop stopt
            PlcService.Disconnect();
        }

        private async Task PressScaleLoop(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    if (PlcService.IsConnected())
                    {
                        (gewicht, materiaalID, aantalBalen, magVerwerken) = PlcService.LeesData();

                        if (magVerwerken && !_isPrinting)
                        {
                            await printToTxtService.WriteDataAsync(gewicht, materiaalID, aantalBalen, magVerwerken);
                            await sqlService.SaveDataAsync(gewicht, aantalBalen, materiaalID);
                            _isPrinting = true;  // Zet vlag op true na printen
                        }

                        if (!magVerwerken)
                        {
                            _isPrinting = false;  // Reset vlag als magVerwerken false is
                        }
                    }
                    else
                    {
                        Console.WriteLine("PLC is niet verbonden. Poging tot herverbinden...");
                        PlcService.Connect();
                    }
                }
                catch (ObjectDisposedException ex)
                {
                    Console.WriteLine($"PLC netwerkfout (verbinding plots weg): {ex.Message}");
                    PlcService.Disconnect();
                    await Task.Delay(1000); // korte wachttijd
                    PlcService.Connect();   // probeer opnieuw te verbinden
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Algemene PLC fout: {ex.Message}");
                }


                try
                {
                    await Task.Delay(150, token);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
            }
        }

//        private bool TryPrintWeight(string weight, out string errorMessage)
//        {

//            var printParameters = new Dictionary<string, string>
//            {
//                { "T1", materiaalID.ToString()},
//                { "T2", aantalBalen.ToString() },
//                { "T3", gewicht.ToString() },
//                { "T4", DateTime.Now.ToShortDateString() },
//                { "T5", DateTime.Now.ToShortTimeString() }
//            };

//            //var result = printService.Print(printParameters);

//            //errorMessage = result;

//            //return errorMessage == null;
//        }
    }
}
