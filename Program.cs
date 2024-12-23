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
            // Khởi tạo các trạm phát và trạm nhận
            Station[] stations = new Station[numberOfStation];
            for (int i = 0; i < numberOfStation; i++)
            {
                stations[i] = new Station(i);
            }
            SIC applicableObj = new SIC();
            // Lặp lại cho đến khi tất cả các gói dữ liệu được truyền đi
            for (int i = 0; i < time / 500; i++)
            {
                Thread.Sleep(500);
                count++;
                // Mỗi trạm phát sẽ phát một gói dữ liệu với một xác suất nhất định
                Random rand = new Random();
                foreach (var station in stations)
                {
                    if (rand.NextDouble() < 0.1)
                    {
                        station.SendPacket(true);
                    }
                }

                // Xử lý xung đột
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

            // số gói phát đi 
            int totalPacketsSent = 0;
            foreach (var station in stations)
            {
                totalPacketsSent += station.PacketsSent;
            }
            // số gói nhận được
            int totalPacketsReceivedSuccessfully = 0;
            foreach (var station in stations)
            {
                totalPacketsReceivedSuccessfully += station.PacketsReceived;
            }
            // In kết quả
            Console.WriteLine("Tổng số gói gửi đi là : " + totalPacketsSent);
            Console.WriteLine("Tổng số gói nhận được là  : " + totalPacketsReceivedSuccessfully);
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
