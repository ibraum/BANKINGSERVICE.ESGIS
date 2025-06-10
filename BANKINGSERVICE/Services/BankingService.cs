using BANKING_SYSTEM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BANKING_SYSTEM.Services
{
    public class BanqueService
    {
        private static AppDBContext context = new AppDBContext();
        private static List<Client> clients = context.Clients.ToList();
        private static List<Compte> comptes = context.Comptes.ToList();

        public bool AjouterClient(Client client)
        {
            bool exists = clients.Where(c => c.Nom.Equals(client.Nom)).Any();
            if (!exists)
            {
                using (var clientContext = new AppDBContext())
                {
                    if (!clientContext.Clients.Where(c => c.Nom.Equals(client.Nom)).Any())
                    {
                        clients.Add(client);
                        clientContext.Clients.Add(client);
                        clientContext.SaveChanges();
                        return true;
                    }
                }
            }
            return false;
        }

        public bool ModifierClient(Client updateClient, int id)
        {
            bool result = false;
            clients.ForEach(c => {
                if (c.Id == id)
                {
                    using (var clientContext = new AppDBContext())
                    {
                        using (var clientTrans = clientContext.Database.BeginTransaction())
                        {
                            try
                            {
                                var client = clientContext.Clients.FirstOrDefault(cc => cc.Id == id);
                                if (client != null)
                                {
                                    c.Nom = updateClient.Nom;
                                    c.Prenom = updateClient.Prenom;
                                    c.Adresse = updateClient.Adresse;
                                    c.CodePostal = updateClient.CodePostal;
                                    c.Telephone = updateClient.Telephone;
                                    c.Ville = updateClient.Ville;

                                    client.Nom = updateClient.Nom;
                                    client.Prenom = updateClient.Prenom;
                                    client.Adresse = updateClient.Adresse;
                                    client.CodePostal = updateClient.CodePostal;
                                    client.Telephone = updateClient.Telephone;
                                    client.Ville = updateClient.Ville;

                                    clientContext.SaveChanges();

                                    clientTrans.Commit();
                                    result = true;
                                }
                            } catch
                            {
                                clientTrans.Rollback();
                            }
                        }
                    }
                }
            });
            return result;
        }

        public bool SupprimerClient(int Id)
        {
            bool result = false;
            Client? deleteClient = this.RechercherClient(Id);

            if (deleteClient != null)
            {
                using (var clientContext = new AppDBContext())
                {
                    using (var clientTrans = clientContext.Database.BeginTransaction())
                    {
                        try
                        {
                            var client = clientContext.Clients.FirstOrDefault(c => c.Nom.Equals(deleteClient.Nom));
                            if (client != null)
                            {
                                var clientComptes = AfficherComptesClient(Id);
                                if (clientComptes != null)
                                {
                                   clientComptes.ForEach(cc => SupprimerCompte(cc.Id));
                                }
                                clients.Remove(deleteClient);
                                clientContext.Clients.Remove(client);
                                clientContext.SaveChanges();
                                result = true;
                            }
                            clientTrans.Commit();
                        }
                        catch
                        {
                            clientTrans.Rollback();
                        }
                    }
                }
            }
            return result;
        }

        public Client? RechercherClient(int Id)
        {
            var client = clients.FirstOrDefault(c => c.Id == Id);
            if (client == null)
            {
                using (var clientContext = new AppDBContext())
                {
                    var cc = clientContext.Clients.FirstOrDefault(c => c.Id == Id);
                    if (cc != null)
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
                return client;
            }
        }

        public Client? RechercherClientParNom(string Nom)
        {
            var client = clients.FirstOrDefault(c => c.Nom.StartsWith(Nom));
            if (client != null)
            {
                using (var clientContext = new AppDBContext())
                {
                    var cc = clientContext.Clients.FirstOrDefault(c => c.Nom.StartsWith(Nom));
                    if (cc != null)
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
                return client;
            }
        }

        public List<Client>? RechercherLesClientsParNom(string Nom)
        {
            var clientsAll = clients.Where(c => c.Nom.StartsWith(Nom)).ToList();
            if (!clientsAll.Any())
            {
                using (var clientContext = new AppDBContext())
                {
                    var cc = clientContext.Clients.Where(c => c.Nom.StartsWith(Nom)).ToList();
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
                return clientsAll;
            }
        }

        public List<Client> AfficherClients()
        {
            if (!clients.Any())
            {
                using (var clientContext = new AppDBContext())
                {
                    return clientContext.Clients.ToList();
                }
            }
            else
            {
                return clients;
            }
        }


        //COMPTES

        public bool AjouterCompte(Compte compte)
        {
            var exists = comptes.FirstOrDefault(c => c.NumAcc.Equals(compte.NumAcc));
            if (exists == null)
            {
                using (var compteContext = new AppDBContext())
                {
                    comptes.Add(compte);
                    
                    compteContext.Comptes.Add(compte);
                    compteContext.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public bool ModifierCompte(Compte updateCompte, int Id)
        {
            comptes.ForEach(c => {
                if (c.Id == Id)
                {
                    using (var compteContext = new AppDBContext())
                    {
                        using (var compteTrans = compteContext.Database.BeginTransaction())
                        {
                            try
                            {
                                var compte = compteContext.Comptes.FirstOrDefault(cc => cc.Id == updateCompte.Id);
                                if (compte != null)
                                {
                                    //list
                                    c.NumAcc = updateCompte.NumAcc;
                                    c.Libelle = updateCompte.Libelle;
                                    c.MontantDecouvert = updateCompte.MontantDecouvert;
                                    c.DateOuverture = updateCompte.DateOuverture;
                                    c.AutorisationDecouvert = updateCompte.AutorisationDecouvert;
                                    c.Solde = updateCompte.Solde;

                                    //context
                                    compte.NumAcc = updateCompte.NumAcc;
                                    compte.Libelle = updateCompte.Libelle;
                                    compte.MontantDecouvert = updateCompte.MontantDecouvert;
                                    compte.DateOuverture = updateCompte.DateOuverture;
                                    compte.AutorisationDecouvert = updateCompte.AutorisationDecouvert;
                                    compte.Solde = updateCompte.Solde;

                                    compteTrans.Commit();
                                    compteContext.SaveChanges();
                                }
                            } catch
                            {
                                compteTrans.Rollback();
                            }
                        }
                    }
                }
            });

            return false;
        }

        public bool SupprimerCompte(int Id)
        {
            Compte? deleteCompte = RechercherCompte(Id);

            if (deleteCompte != null)
            {
                using (var compteContext = new AppDBContext())
                {
                    using (var compteTrans = compteContext.Database.BeginTransaction())
                    {
                        try
                        {
                            var compte = compteContext.Comptes.FirstOrDefault(c => c.Id == Id);
                            if (compte != null)
                            {
                                comptes.Remove(deleteCompte);
                                compteContext.Comptes.Remove(compte);
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

        public Compte? RechercherCompte(int Id)
        {
            var compte = comptes.FirstOrDefault(c => c.Id == Id);
            if (compte != null)
            {
                using (var compteContext = new AppDBContext())
                {
                    var cc = compteContext.Comptes.FirstOrDefault(c => c.Id == Id);
                    if (cc != null)
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
                return compte;
            }
        }

        public List<Compte>? AfficherComptesClient(int Id)
        {
            var client = RechercherClient(Id);
            if (client != null)
            {
                var comptesClient = comptes.Where(cc => cc.ClientId == Id).ToList();
                if (!comptesClient.Any())
                {
                    using (var comptesContext = new AppDBContext())
                    {
                        return comptesContext.Comptes.Where(cc => cc.ClientId == Id).ToList();
                    }
                }
                else
                {
                    return comptesClient;
                }
            }
            return null;
        }

        public List<Compte> AfficherComptes()
        {
            if (!comptes.Any())
            {
                using (var compteContext = new AppDBContext())
                {
                    return compteContext.Comptes.ToList();
                }
            }
            else
            {
                return comptes;
            }
        }

        public bool Virement(int sourceId, int destinationId, decimal montant)
        {
            Compte? source = RechercherCompte(sourceId);
            Compte? destination = RechercherCompte(destinationId);

            if (source != null && destination != null && source.Debit(montant))
            {
                using (var compteContext = new AppDBContext())
                {
                    using (var compteTrans = compteContext.Database.BeginTransaction())
                    {
                        try
                        {
                            source.Debit(montant);
                            destination.Credit(montant);
                            compteTrans.Commit();
                        } catch
                        {
                            compteTrans.Rollback();
                        }
                    }
                }

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
