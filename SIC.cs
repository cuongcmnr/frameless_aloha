namespace FL_Aloha
{
    public class SIC
    {
        public bool IsCollision(Station[] stations)
        {
            for (int i = 0; i < stations.Length - 1; i++)
            {
                for (int j = i + 1; j < stations.Length; j++)
                {
                    if (stations[i].IsPacketSent && stations[j].IsPacketSent)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
