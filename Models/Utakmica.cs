using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;

namespace baze_redis.Models
{
    public class Utakmica
    {
        public string Domacin { get; }
        public int Domacin_golovi { get; }
        public string Gost { get; }
        public int Gost_golovi { get; }
        public int Kolo { get; }

        public Utakmica(string domacin, int domacin_golovi, string gost, int gost_golovi, int kolo)
        {
            Domacin = domacin;
            Domacin_golovi = domacin_golovi;
            Gost = gost;
            Gost_golovi = gost_golovi;
            Kolo = kolo;
        }

        public void sortiraj(List<Utakmica> utakmice)
        {
            Utakmica temp;
            for (int i=0;i<utakmice.Count;i++)
                for (int j = 0; j < utakmice.Count; j++)
                {
                    if (utakmice[i].Kolo > utakmice[i+1].Kolo) 
                    {
                        temp = utakmice[i + 1];
                        utakmice[i + 1] = utakmice[i];
                        utakmice[i] = temp;
                    }
                }
        }
    }
}
