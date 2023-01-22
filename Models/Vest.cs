namespace baze_redis.Models
{
    public class Vest
    {
        public string Klub { get; }
        public string Tekst { get; }

        public Vest(string klub, string tekst)
        {
            Klub = klub;
            Tekst= tekst;
        }
    }
}
