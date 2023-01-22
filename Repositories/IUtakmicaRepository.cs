using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using baze_redis.Models;
using ServiceStack.Redis;
using ServiceStack.Text;

namespace baze_redis.Repositories
{
    public interface IUtakmicaRepository
    {
        List<Utakmica> Utakmice(string naziv);
        bool Register(string username, string password, string klub);
        List<String> sviKlubovi();
        bool Login(string username, string password);
        List<Klub> Tabela();
        List<Vest> Vesti(string naziv);

        String omiljeniKlub(string naziv);
        bool promeniKlub(string klub, string sifra,string user);
        bool promeniSifru(string novasifra, string starasifra,string user);
        bool obrisiKorisnika(string user, string sifra);
    }
    public class UtakmicaRepositories : IUtakmicaRepository
    {
        readonly RedisClient redis = new RedisClient(Config.SingleHost);

        public List<Utakmica> Utakmice(string naziv)
        {
            List<Utakmica> utakmice = new List<Utakmica>();
            List<string> s = new List<string>();


            List<string> s1 = redis.GetAllKeys();
            List<string> s2 = new List<string>();
            foreach (string str in s1)
            {
                if (str.IndexOf("utakmica:") != -1)
                {
                    s2.Add(str);
                }
            }

            foreach (string j in s2)
            {
                foreach (string jsonVisitorString in redis.GetHashValues(j))
                {
                    s.Add(jsonVisitorString);
                }
                Utakmica u = new Utakmica(s[0], int.Parse(s[1]), s[2], int.Parse(s[3]), int.Parse(s[4]));
                if (u.Domacin == naziv || u.Gost==naziv)
                {
                    utakmice.Add(u);
                }
                s = new List<string>();
            }

            return utakmice;
        }

        public List<Vest> Vesti(string naziv)
        {
            List<Vest> vesti = new List<Vest>();
            List<string> s = new List<string>();


            List<string> s1 = redis.GetAllKeys();
            List<string> s2 = new List<string>();
            foreach (string str in s1)
            {
                if (str.IndexOf("vest:") != -1)
                {
                    s2.Add(str);
                }
            }

            foreach (string j in s2)
            {
                foreach (string jsonVisitorString in redis.GetHashValues(j))
                {
                    s.Add(jsonVisitorString);
                }
                Vest v = new Vest(s[0], s[1]);
                if (v.Klub == naziv)
                {
                    vesti.Add(v);
                }
                s = new List<string>();
            }

            return vesti;
        }

        public bool Register(string username,string password,string klub)
        {
            int i =1;
            List<string> s1 = redis.GetAllKeys();
            List<string> s2 = new List<string>();
            foreach (string str in s1)
            {
                if (str.IndexOf("user:") != -1)
                {
                    s2.Add(str);
                }
            }
            while (s2.IndexOf("user:" + i.ToString()) != -1) 
            {
                i++;
            }
            List<string> s = new List<string>();
            foreach (string str in s2)
            {
                foreach (string jsonVisitorString in redis.GetHashValues(str))
                {
                    s.Add(jsonVisitorString);
                }
                if (s[0] == username)
                {
                    return false;
                }
                s = new List<string>();
            }

            redis.SetEntryInHash("user:"+i.ToString(),"username",username);
            redis.SetEntryInHash("user:"+i.ToString(), "password", password);
            redis.SetEntryInHash("user:"+ i.ToString(), "klub", klub);
            return true;
        }

        public bool Login(string username, string password)
        {
            List<string> s1 = redis.GetAllKeys();
            List<string> s2 = new List<string>();
            foreach (string str in s1)
            {
                if (str.IndexOf("user:") != -1)
                {
                    s2.Add(str);
                }
            }
            s2.Sort();
            List<string> s = new List<string>();

            foreach (string str in s2)
            {
                foreach (string jsonVisitorString in redis.GetHashValues(str))
                {
                    s.Add(jsonVisitorString);
                }
                if (s[0] == username && s[1] == password)
                {
                    return true;
                }
                s = new List<string>();
            }
            return false;
        }

        public List<String> sviKlubovi()
        {
            List<string> s = new List<string>();
            s = redis.GetAllItemsFromSortedSetDesc("klubovi");
            return s;
        }

        public List<Klub> Tabela()
        {
            IDictionary<string, double> dict;
            double j;
            dict = redis.GetAllWithScoresFromSortedSet("klubovi");
            List<string> s = redis.GetAllItemsFromSortedSetDesc("klubovi");
            List<Klub> lista = new List<Klub>();
            for (int i =0;i<dict.Count;i++)
            {
                dict.TryGetValue(s[i], out j);
                Klub k = new Klub(s[i], ((int)j));
                lista.Add(k);
            }
            return lista;
            
        }

        public String omiljeniKlub(string Naziv)
        {
            List<string> s1 = redis.GetAllKeys();
            List<string> s2 = new List<string>();
            foreach (string str in s1)
            {
                if (str.IndexOf("user:") != -1)
                {
                    s2.Add(str);
                }
            }

            List<String> s = new List<string>();
            foreach (string j in s2)
            {
                foreach (string jsonVisitorString in redis.GetHashValues(j))
                {
                    s.Add(jsonVisitorString);
                }
                if (s[0] == Naziv)
                {
                    return s[2];
                }
                s = new List<string>();
            }
            return "";
        }

        public bool promeniKlub(string klub, string sifra, string user)
        {
            List<string> s1 = redis.GetAllKeys();
            List<string> s2 = new List<string>();
            foreach (string str in s1)
            {
                if (str.IndexOf("user:") != -1)
                {
                    s2.Add(str);
                }
            }
            List<string> s = new List<string>();
            foreach (string str in s2)
            {
                foreach (string jsonVisitorString in redis.GetHashValues(str))
                {
                    s.Add(jsonVisitorString);
                }
                if (s[0] == user && s[1] == sifra)
                {
                    redis.SetEntryInHash(str, "klub", klub);
                    return true;
                }
                s = new List<string>();
            }
            return false;
        }

        public bool promeniSifru(string novasifra, string starasifra, string user)
        {
            List<string> s1 = redis.GetAllKeys();
            List<string> s2 = new List<string>();
            foreach (string str in s1)
            {
                if (str.IndexOf("user:") != -1)
                {
                    s2.Add(str);
                }
            }
            List<string> s = new List<string>();
            foreach (string str in s2)
            {
                foreach (string jsonVisitorString in redis.GetHashValues(str))
                {
                    s.Add(jsonVisitorString);
                }
                if (s[0] == user && s[1] == starasifra)
                {
                    redis.SetEntryInHash(str, "password", novasifra);
                    return true;
                }
                s = new List<string>();
            }
            return false;
        }

        public bool obrisiKorisnika(string user, string sifra)
        {
            List<string> s1 = redis.GetAllKeys();
            List<string> s2 = new List<string>();
            foreach (string str in s1)
            {
                if (str.IndexOf("user:") != -1)
                {
                    s2.Add(str);
                }
            }
            List<string> s = new List<string>();
            foreach (string str in s2)
            {
                foreach (string jsonVisitorString in redis.GetHashValues(str))
                {
                    s.Add(jsonVisitorString);
                }
                if (s[0] == user && s[1] == sifra)
                {
                    redis.Remove(str);
                    return true;
                }
                s = new List<string>();
            }
            return false;
        }
    }
}
