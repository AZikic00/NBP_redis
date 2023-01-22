namespace baze_redis.Models
{
    public class Klub
    {
        public string Naziv { get; }

        public int Bodovi { get; }

        public Klub(string naziv, int bodovi) 
        { 
            Naziv = naziv;
            Bodovi = bodovi;
        }
    }
}
