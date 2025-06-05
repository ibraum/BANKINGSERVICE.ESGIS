using BANKING_SYSTEM.Models;
using BANKING_SYSTEME.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BANKING_SYSTEME.Services
{
    public class BanqueService
    {
        private List<Client> Clients = new List<Client>();
        private List<Compte> Comptes = new List<Compte>();

        public bool AjouterClient(Client client)
        {
            bool exists = Clients.Where(c => c.nom.Equals(client.nom)).Any();
            if (!exists)
            {
                using (var clientContext = new AppDBContext())
                {
                    if (!clientContext.Clients.Where(c => c.nom.Equals(client.nom)).Any())
                    {
                        Clients.Add(client);
                        clientContext.Clients.Add(client);
                        clientContext.SaveChanges();
                        return true;
                    }
                }
            }
            return false;
        }

        public bool ModifierClient(Client updateClient)
        {
            Clients.ForEach(c => {
                if (c.id == updateClient.id)
                {
                    using (var clientContext = new AppDBContext())
                    {
                        Client client = clientContext.Clients.Where(cc => cc.nom.Equals(updateClient.nom)).FirstOrDefault();
                        if (client != null)
                        {
                            //list
                            c.nom = updateClient.nom;
                            c.prenom = updateClient.prenom;
                            c.adresse = updateClient.adresse;
                            c.code_postal = updateClient.code_postal;
                            c.telephone = updateClient.telephone;
                            c.ville = updateClient.ville;

                            //context
                            client.nom = updateClient.nom;
                            client.prenom = updateClient.prenom;
                            client.adresse = updateClient.adresse;
                            client.code_postal = updateClient.code_postal;
                            client.telephone = updateClient.telephone;
                            client.ville = updateClient.ville;
                            clientContext.Clients.Add(client);
                            clientContext.SaveChanges();

                        }
                    }
                }
            });
            return false;
        }

        public bool SupprimerClient(int id)
        {
            Client deleteClient = this.RechercherClient(id);

            if (deleteClient != null)
            {
                using (var clientContext = new AppDBContext())
                {
                    using (var clientTrans = clientContext.Database.BeginTransaction())
                    {
                        try
                        {
                            var client = clientContext.Clients.Where(c => c.nom.Equals(deleteClient.nom));
                            if (client.Any())
                            {
                                var clientComptes = AfficherComptesClient(id);
                                clientComptes.ForEach(cc => SupprimerCompte(cc.id));
                                Clients.Remove(deleteClient);
                                clientContext.Clients.Remove(client.FirstOrDefault());
                                clientContext.SaveChanges();
                                return true;
                            }
                            clientTrans.Commit();
                        }
                        catch
                        {
                            clientTrans.Rollback();
                        }
                    }
                }
                return true;
            }
            return false;
        }

        public Client RechercherClient(int id)
        {
            var client = Clients.Where(c => c.id == id);
            if (!client.Any())
            {
                using (var clientContext = new AppDBContext())
                {
                    var cc = clientContext.Clients.Where(c => c.id == id);
                    if (!cc.Any())
                    {
                        return cc.FirstOrDefault();
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                return client.FirstOrDefault();
            }
        }

        public Client RechercherClientParnom(string nom)
        {
            var client = Clients.Where(c => c.nom.StartsWith(nom));
            if (!client.Any())
            {
                using (var clientContext = new AppDBContext())
                {
                    var cc = clientContext.Clients.Where(c => c.nom.StartsWith(nom));
                    if (!cc.Any())
                    {
                        return cc.FirstOrDefault();
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                return client.FirstOrDefault();
            }
        }

        public List<Client> RechercherLesClientsParnom(string nom)
        {
            var clients = Clients.Where(c => c.nom.StartsWith(nom)).ToList();
            if (!clients.Any())
            {
                using (var clientContext = new AppDBContext())
                {
                    var cc = clientContext.Clients.Where(c => c.nom.StartsWith(nom)).ToList();
                    if (!cc.Any())
                    {
                        return cc;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                return clients;
            }
        }

        public List<Client> AfficherClients()
        {
            if (!Clients.Any())
            {
                using (var clientContext = new AppDBContext())
                {
                    return clientContext.Clients.ToList();
                }
            }
            else
            {
                return Clients;
            }
        }

        //COMPTES

        public bool AjouterCompte(Compte compte)
        {
            bool exists = Comptes.Where(c => c.NumAcc.Equals(compte.NumAcc)).Any();
            if (!exists)
            {
                Comptes.Add(compte);
                return true;
            }
            return false;
        }

        public bool ModifierCompte(Compte updateCompte)
        {
            Comptes.ForEach(c => {
                if (c.id == updateCompte.id)
                {
                    c.NumAcc = updateCompte.NumAcc;
                    c.Libelle = updateCompte.Libelle;
                    c.MontantDecouvert = updateCompte.MontantDecouvert;
                    c.DateOuverture = updateCompte.DateOuverture;
                    c.AutorisationDecouvert = updateCompte.AutorisationDecouvert;
                    c.Solde = updateCompte.Solde;
                }
            });

            return false;
        }

        public bool SupprimerCompte(int id)
        {
            Compte deleteCompte = RechercherCompte(id);

            if (deleteCompte != null)
            {
                using (var compteContext = new AppDBContext())
                {
                    using (var compteTrans = compteContext.Database.BeginTransaction())
                    {
                        try
                        {
                            var compte = compteContext.Comptes.Where(c => c.id == id);
                            if (compte.Any())
                            {
                                Comptes.Remove(deleteCompte);
                                compteContext.Comptes.Remove(compte.FirstOrDefault());
                                compteContext.SaveChanges();
                                return true;
                            }
                            compteTrans.Commit();
                        }
                        catch
                        {
                            compteTrans.Rollback();
                        }
                    }
                }
                return true;
            }
            return false;
        }

        public Compte RechercherCompte(int id)
        {
            return Comptes.FirstOrDefault(c => c.id == id);
        }

        public List<Compte> AfficherComptesClient(int id)
        {
            var client = RechercherClient(id);
            if (client != null)
            {
                var comptesClient = Comptes.Where(cc => cc.Clientid == id).ToList();
                if (!comptesClient.Any())
                {
                    using (var comptesContext = new AppDBContext())
                    {
                        return comptesContext.Comptes.Where(cc => cc.Clientid == id).ToList();
                    }
                }
                else
                {
                    return comptesClient;
                }
            }
            return Comptes.Where(c => c.Clientid == id).ToList();
        }

        public List<Compte> AfficherComptes()
        {
            return Comptes;
        }

        public bool Virement(int sourceid, int destinationid, decimal montant)
        {
            Compte source = RechercherCompte(sourceid);
            Compte destination = RechercherCompte(destinationid);

            if (source != null && destination != null && source.Debit(montant))
            {
                destination.Credit(montant);

                return true;
            }

            return false;
        }
    }
}

//using BANKING_SYSTEME.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace BANKINGSERVICE.Services
//{
//    public class BankingService
//    {
//        private List<Client> Clients = new List<Client>();
//        private List<Compte> Comptes = new List<Compte>();

//        public bool AjouterClient(Client ClientAjouter)
//        {
//            //bool existe = clients.Any(c => c.id == client.id);
//            bool existe = Clients.Where(o => o.nom.Equals(ClientAjouter.nom)).Any();
//            if (!existe)
//            {
//                Clients.Add(ClientAjouter);
//                return true;
//            }
//            return false;
//        }

//        public bool ModifierClient(Client ClientModifier)
//        {
//           //Client clientTrouver = RechercherClient(ClientModifier.id);
//           // if (clientTrouver != null)
//           // {
//           //     clientTrouver.nom = ClientModifier.nom;
//           //     clientTrouver.prenom = ClientModifier.prenom;
//           //     clientTrouver.adresse = ClientModifier.adresse;
//           //     clientTrouver.code_postall = ClientModifier.code_postall;
//           //     clientTrouver.ville = ClientModifier.ville;
//           //     clientTrouver.telephone = ClientModifier.telephone;


//           //     return true;
//           // }

//            foreach (Client Clientr in Clients)
//            {
//                if (Clientr.id == ClientModifier.id)
//                {
//                    Clientr.nom = ClientModifier.nom;
//                    Clientr.prenom = ClientModifier.prenom;
//                    Clientr.adresse = ClientModifier.adresse;
//                    Clientr.code_postall = ClientModifier.code_postall;
//                    Clientr.ville = ClientModifier.ville;
//                    Clientr.telephone = ClientModifier.telephone;

//                }
//            }
//            return false;
//        }

//        public Client RechercherClient(int id)
//        {
//            return Clients.FirstOrDefault(c => c.id == id);
//        }
//        public bool SupprimerClient(int id)
//        {
//            Client client = RechercherClient(id);
//            if (client != null)
//            {
//                Clients.Remove(client);
//                return true;
//            }
//            return false;
//        }

//        public List<Client> AfficherClients()
//        {
//            return Clients;
//        }

//        public bool AjouterCompte(Compte compteAjouter)
//        {

//                Comptes.Add(compteAjouter);
//                return true;


//        }
//    }
//}
