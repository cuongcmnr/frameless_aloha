namespace FL_Aloha
{
    public class Station
    {
        private int id;
        private Packet pk = new Packet();
        public bool IsPacketSent { get; private set; }
        public int PacketsSent { get; private set; }
        public int PacketsReceived { get; private set; }

        public Station(int id)
        {
            this.id = id;
            this.IsPacketSent = false;
            this.PacketsSent = 0;
            this.PacketsReceived = 0;
        }

        public void SendPacket(bool check)
        {
            if (check)
            {
                this.IsPacketSent = true;
                this.PacketsSent++;
            }
            else
            {
                this.IsPacketSent = false;
            }
        }

        public void ReceivePacket(bool success)
        {
            if (success)
            {
                this.PacketsReceived++;
                Console.WriteLine("Station " + this.id + " receives packet successfully");
            }
            else
            {
                Console.WriteLine("Station " + this.id + " receives packet with collision");
            }
        }

        public int FNumberOfRetrans()
        {
            return pk.NumberOfRetrans();
        }

        public int GetNumberOfRetrans()
        {
            return pk.GetCount();
        }

        public int GetPacketsReceived()
        {
            return this.PacketsReceived;
        }

        public int GetId()
        {
            return this.id;
        }
    }
}