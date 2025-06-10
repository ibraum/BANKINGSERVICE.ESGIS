using BANKING_SYSTEM.Models;
using BANKING_SYSTEM.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BANKING_SYSTEM.Controllers
{
  
        [ApiController]
        [Route("Api/[controller]")]
        public class ClientController : ControllerBase
        {
            private readonly BanqueService _service = new BanqueService();

            [HttpGet]
            public IActionResult GetClients()
            {
                return Ok(_service.AfficherClients());
            }

            [HttpPost]
            public IActionResult AddClient([FromBody] Client client)
            {
                _service.AjouterClient(client);
                return Ok();
            }

            [HttpGet("{id}")]
            public IActionResult getClientByID(int id)
            {
                return Ok(_service.RechercherClient(id));
            }

            [HttpPut("{id}")]
            public IActionResult UpdateClient([FromBody] Client client, int id)
            {
            _service.ModifierClient(client);
                return Ok();
            }

            [HttpDelete("{id}")]
            public IActionResult DeleteClient(int id)
            {
                _service.SupprimerClient(id);
                return Ok();
            }
        }

        //private readonly BanqueService _service = new();
        //[HttpGet]

        //public IActionResult GetClients()=> Ok(_service.RechercheClientParNom(""));

        //[HttpPost]
        //public IActionResult AddClient([FromBody] Client client)
        //{
        //    _service.AjouterClient(client); return Ok();
        //}

        //__________________________________________________________

        //// GET: ClientController1
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //// GET: ClientController1/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: ClientController1/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: ClientController1/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: ClientController1/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: ClientController1/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: ClientController1/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: ClientController1/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}


    
}
