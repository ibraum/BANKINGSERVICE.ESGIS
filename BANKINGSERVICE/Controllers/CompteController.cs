using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using BANKING_SYSTEM.Models;
using BANKING_SYSTEM.Services;

namespace BANKING_SYSTEM.Controllers
{
    [Route("Api/[controller]")]
    public class CompteController : ControllerBase
    {
        private readonly BanqueService _service = new BanqueService();
        
        [HttpGet]
        public IActionResult GetComptes() {

            return Ok(_service.AfficherComptes());
                
        }

        [HttpPost]
        public IActionResult AddCompte([FromBody] Compte compte)
        {
            _service.AjouterCompte(compte);
            return Created();
        }

        [HttpPut("{Id}")]
        public IActionResult UpdateCompte([FromBody] Compte compte, int Id)
        {
            _service.ModifierCompte(compte, Id);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult SupprimerCompte(int id)
        {
            var compte = _service.RechercherCompte(id);
            if (compte == null)
            {
                return NotFound($"Aucun compte trouvé avec l'id {id}");
            }
            _service.SupprimerCompte(id);
            return NoContent();
        }

        [HttpGet("{id}")]
        public ActionResult<Compte> RechercherCompteParId(int id)
        {
            var compte = _service.RechercherCompte(id);
            if (compte == null)
            {
                return NotFound($"Aucun compte trouvé avec l'id {id}");
            }
            return Ok(compte);
        }

        [HttpGet("Client/{clientId}")]
        public ActionResult<IEnumerable<Compte>> RechercherComptesClient(int clientId)
        {
            var comptes = _service.AfficherComptes();
            if (comptes == null || !comptes.Any())
            {
                return NotFound($"Aucun compte(s) trouvé(s) avec l'id {clientId}");
            }
            return Ok(comptes);
        }

        [HttpPost("Virement")]
        public IActionResult Virement([FromBody] int source, [FromBody] int destination, [FromBody] decimal montant)
        {
            if (_service.Virement(source, destination, montant))
            {
                return Ok("virement réussi");
            }
            return BadRequest("Echec de virement");
        }
    }
}

//using BANKING_SYSTEME.Models;
//using BANKING_SYSTEME.Services;
//using Microsoft.AspNetCore.Mvc;

//namespace BANKING_SYSTEM.Controllers
//{
//    [Route("Api/[controller]")]
//    public class CompteController : ControllerBase
//    {
//        public readonly BanqueService _service = new();
//        [HttpGet]
//        public IActionResult GetCompte() => Ok(_service.AfficherComptes());


//        [HttpPost("virement")]

//        public IActionResult virement([FromQuery] int source, [FromQuery] int destination, [FromQuery] int montant)
//        {
//            if (_service.Virement(source, destination, montant))
//            {
//                return Ok("Virement effectué avec succès");
//            }
//            return BadRequest("Erreur lors du virement");
//        }

//        [HttpDelete("{id}")]
//        public IActionResult DeleteCompte(int id)
//        {
//            var compte = _service.RechercherCompte(id);
//            if (compte != null)
//            {
//                return NotFound($"Aucun compte trouvé avec l'id{id}");
//            }
//            _service.SupprimerClient(id);
//            return NotFound();
//        }

//        [HttpGet("{id}")]
//        public ActionResult<Compte> RechercheCompte(int id)
//        {
//            var compte = _service.RechercherCompte(id);
//            if (compte == null)
//            {
//                return NotFound($"Aucun compte trouvé avec l'id{id}");
//            }
//            return Ok(compte);
//        }    

//        [HttpGet("client/{Clientid}")]
//        public ActionResult<IEnumerable<Compte>> RechercheCompteClient(int client)
//        {
//            var compte = _service.RechercherCompte(ClientId);
//            if (compte == null || !compte.Any())
//            {
//                return NotFound($"Aucun compte trouvé avec l'id{clientId}");
//            }
//            return Ok(compte);
//        }
//    }
//}
