using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BANKING_SYSTEME.Models
{
  
    public abstract class Compte
    {
        public int id { get; set; }
        public string Libelle { get; set; }
        public DateTime DateOuverture { get; set; }
        public decimal MontantDecouvert { get; set; }
        public bool AutorisationDecouvert { get; set; }
        public decimal Solde { get; set; }
        public string TypeCompte { get; set; }
        public int Clientid { get; set; }
        public Client Client { get; set; }
        public int NumAcc { get; set; }

        public virtual void Credit(decimal montant) => Solde += montant;

        public virtual bool Debit (decimal montant)
        {
            if (Solde - montant >= (AutorisationDecouvert ? -MontantDecouvert : 0))
            {
                Solde -= montant;
                return true;
            }
            return false;
        }

    }
}