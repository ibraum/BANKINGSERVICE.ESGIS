using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BANKING_SYSTEM.Models
{
    public class Livret:Compte
    {
        public double TauxDeRenumeration { get; set; }

        public void AjouterInteret()
        {
            var interet = Solde * (decimal)(TauxDeRenumeration / 100);
            Credit(interet);
        }
        public override bool Debit(decimal montant) => false; // interdit le découvert
    }
}