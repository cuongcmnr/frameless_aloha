using Plotly.NET;
namespace FL_Aloha
{
    class Program
    {
        static void Main(string[] args)
        {
            int numberOfStation = 10;
            int time = 60000;
            int count = 0;
            // Init stations
            Station[] stations = new Station[numberOfStation];
            for (int i = 0; i < numberOfStation; i++)
            {
                stations[i] = new Station(i);
            }
            SIC applicableObj = new SIC();
            // Repeat until all data packets are transmitted
            for (int i = 0; i < time / 500; i++)
            {
                Thread.Sleep(500);
                count++;
                // Each transmitter station will broadcast a data packet with a certain probability
                Random rand = new Random();
                foreach (var station in stations)
                {
                    if (rand.NextDouble() < 0.1)
                    {
                        station.SendPacket(true);
                    }
                }

                // Conflict Handling
                foreach (var station in stations)
                {
                    if (station.IsPacketSent)
                    {
                        if (applicableObj.IsCollision(stations))
                        {
                            if (station.GetNumberOfRetrans() == 3)
                                station.ReceivePacket(false);
                            else
                                station.FNumberOfRetrans();
                        }
                        else
                        {
                            station.ReceivePacket(true);
                        }
                    }
                }
                foreach (var station in stations)
                {
                    station.SendPacket(false);
                }
            }

            // Packets Sent 
            int totalPacketsSent = 0;
            foreach (var station in stations)
            {
                totalPacketsSent += station.PacketsSent;
            }
            // Packet Rêcived
            int totalPacketsReceivedSuccessfully = 0;
            foreach (var station in stations)
            {
                totalPacketsReceivedSuccessfully += station.PacketsReceived;
            }
            // Réult
            Console.WriteLine("Total Sent Package : " + totalPacketsSent);
            Console.WriteLine("Total Successful Received Package  : " + totalPacketsReceivedSuccessfully);
            Console.WriteLine(count);
            var stationsArray = Enumerable.Range(1, numberOfStation).ToArray();
            var packetsSentData = stations.Select(s => (double)s.PacketsSent).ToArray();
            var successfulTransmissionsData = stations.Select(s => (double)s.PacketsReceived).ToArray();

            // Create line charts for packets sent and received successfully
            var packetsSentChart = Chart2D.Chart.Line<int, double, string>(
                stationsArray,
                packetsSentData,
                Name: "Total Packets Sent"
            );

            var successfulTransmissionsChart = Chart2D.Chart.Line<int, double, string>(
                stationsArray,
                successfulTransmissionsData,
                Name: "Total Packets Received Successfully"
            );

            // Combine the charts
            var combinedChart = Chart.Combine(new[] { packetsSentChart, successfulTransmissionsChart })
                .WithTitle("Packets Sent and Received Successfully")
                .WithXAxisStyle(Title.init("Number of Stations"))
                .WithYAxisStyle(Title.init("Number of Packets"));

            // Display the chart
            combinedChart.Show();

        }
    }
}
