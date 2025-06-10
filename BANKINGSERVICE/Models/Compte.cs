using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Web;

// TPH
// Mvc Core

namespace BANKING_SYSTEM.Models
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
    [JsonDerivedType(typeof(Livret), "livret")]
    [JsonDerivedType(typeof(PEL), "pel")]
    [JsonDerivedType(typeof(CompteCourant), "comptecourant")]
    public abstract class Compte
    {
        public int Id { get; set; }
        public string? Libelle { get; set; }
        public DateTime DateOuverture { get; set; }
        public decimal MontantDecouvert { get; set; }
        public bool AutorisationDecouvert { get; set; }
        public decimal Solde { get; set; }
        public int ClientId { get; set; }
        public Client? Client { get; set; }
        public string NumAcc { get; set; }

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