using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BANKING_SYSTEME.Models
{
    public class Client
    {
        public int id {get; set;}
        public string nom {get; set;}
        public string prenom {get; set;}
        public string adresse {get; set;}
        public string code_postal {get; set;}
        public string ville  {get; set;}
        public string telephone  {get; set;}
        public ICollection<Compte> Comptes { get; set; }

    }
}