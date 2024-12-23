namespace FL_Aloha
{
    public class Packet
    {
        private int count = 0;

        public int GetCount()
        {
            return count;
        }

        public int NumberOfRetrans()
        {
            return count++;
        }
    }
}
