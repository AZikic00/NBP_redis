using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using baze_redis.Models;
using baze_redis.Repositories;

namespace baze_redis.Controllers
{
    [ApiController]
    [Route("/")]
    public class UtakmicaController
    {
        private readonly IUtakmicaRepository _utakmicaRepository;
        public UtakmicaController(IUtakmicaRepository utakmicaRepository)
        {
            _utakmicaRepository = utakmicaRepository;
        }

        [Route("/utakmice/{naziv}")]
        [HttpGet]
        public Task<List<Utakmica>> Utakmice([FromRoute(Name = "naziv")] string naziv)
        {
            return Task.FromResult(_utakmicaRepository.Utakmice(naziv));
        }

        [Route("/vesti/{naziv}")]
        [HttpGet]
        public Task<List<Vest>> Vesti([FromRoute(Name = "naziv")] string naziv)
        {
            return Task.FromResult(_utakmicaRepository.Vesti(naziv));
        }

        [Route("/register/{username}/{password}/{klub}")]
        [HttpPost]
        public bool Register([FromRoute(Name = "username")] string username,[FromRoute(Name = "password")] string password,[FromRoute(Name = "klub")] string klub)
        {
            return _utakmicaRepository.Register(username,password,klub);
        }

        [Route("/login/{username}/{password}")]
        [HttpGet]
        public bool Login([FromRoute(Name = "username")] string username, [FromRoute(Name = "password")] string password)
        {
            return _utakmicaRepository.Login(username, password);
        }

        [Route("/sviKlubovi")]
        [HttpGet]
        public Task<List<String>> sviKlubovi()
        {
            return Task.FromResult(_utakmicaRepository.sviKlubovi());
        }

        [Route("/tabela")]
        [HttpGet]
        public Task<List<Klub>> Tabela()
        {
            return Task.FromResult(_utakmicaRepository.Tabela());
        }

        [Route("/omiljeniKlub/{username}")]
        [HttpGet]
        public Task<String> omiljeniKlub([FromRoute(Name = "username")] string username)
        {
            return Task.FromResult(_utakmicaRepository.omiljeniKlub(username));
        }

        [Route("/promeniklub/{klub}/{password}/{user}")]
        [HttpPut]
        public bool promeniKlub([FromRoute(Name = "klub")] string klub, [FromRoute(Name = "password")] string password, [FromRoute(Name = "user")] string user)
        {
            return _utakmicaRepository.promeniKlub(klub,password,user);
        }

        [Route("/promenisifru/{novasifra}/{starasifra}/{user}")]
        [HttpPut]
        public bool promeniSifru([FromRoute(Name = "novasifra")] string novasifra, [FromRoute(Name = "starasifra")] string starasifra, [FromRoute(Name = "user")] string user)
        {
            return _utakmicaRepository.promeniSifru(novasifra, starasifra,user);
        }

        [Route("/obrisikorisnika/{user}/{sifra}")]
        [HttpDelete]
        public bool obrisiKorisnika([FromRoute(Name = "user")] string user, [FromRoute(Name = "sifra")] string sifra)
        {
            return _utakmicaRepository.obrisiKorisnika(user,sifra);
        }
    }
}

