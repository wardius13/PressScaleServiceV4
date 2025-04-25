using S7.Net;
using System;

namespace PlcReader.Services
{
    public class PlcService
    {
        private Plc plc;

        public PlcService(string ipAddress)
        {
            plc = new Plc(CpuType.S71500, ipAddress, 0, 1); // Rack 0, Slot 1 meestal standaard
        }

        public void Connect()
        {
            if (!plc.IsConnected)
                plc.Open();
        }

        public void Disconnect()
        {
            if (plc.IsConnected)
                plc.Close();
        }

        public (float gewicht, int materiaalID, int aantalBalen, bool magVerwerken) LeesData()
        {
            Connect();

            // Gewicht: REAL (DBD0 → 4 bytes → UInt32 → float)
            uint rawGewicht = (uint)plc.Read("DB1.DBD0");
            float gewicht = BitConverter.ToSingle(BitConverter.GetBytes(rawGewicht), 0);

            // MateriaalID & AantalBalen: INT (DBW4, DBW6 → short)
            short materiaalID = Convert.ToInt16(plc.Read("DB1.DBW4"));
            short aantalBalen = Convert.ToInt16(plc.Read("DB1.DBW6"));

            // MagVerwerken: BOOL (DBX8.0)
            bool magVerwerken = Convert.ToBoolean(plc.Read("DB1.DBB8"));


            return (gewicht, materiaalID, aantalBalen, magVerwerken);
        }

        public bool IsConnected()
        {
                return plc.IsConnected;
        }

        internal void LeesData(float v1, object gewicht, int v2, object materiaalID, int v3, object aantalBalen, bool v4, object magVerwerken)
        {
            throw new NotImplementedException();
        }

        internal void LeesData(float gewicht, int materiaalID, int aantalBalen, bool magVerwerken)
        {
            throw new NotImplementedException();
        }
    }
}
