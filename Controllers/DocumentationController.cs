using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NWBackendAPI.Models;

namespace NWBackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentationController : ControllerBase
    {
        //SUOJAA TIETOKANTAYHTEYTTÄ. EI VOI ESIM TOISESTA TIEDOSTOSTA SOTKEMAAN
        //private readonly northwindOriginalContext db = new northwindOriginalContext();
        private readonly string savedKeycode = "bond007";
        //DEPENDENCY INJECTION TYYLI
        private readonly northwindOriginalContext db;
        public DocumentationController(northwindOriginalContext dbparametri)
        {
            db = dbparametri;
        }
        //TARVITAAN KEY eli keycode
        [HttpGet("{keycode}")]
        // TAI [Route ("{id}")]
        //PALAUTETAAN STATUS KOODI JA MUKANA KULKEE JOKU VIESTI
        public ActionResult GetAllDocumentation(string keycode)
        {
            if (keycode == savedKeycode)
            {
                var docs = db.Documentations.ToList();
                return Ok(docs);
            }
            else
            {
                return Unauthorized("keycode is not valid");
            }

            //List<Documentation> documents = db.Documentations.ToList();
            //return Ok(documents)
        }
       

        [HttpPost]
        
        public ActionResult AddNewDocumentation([FromBody] Documentation documents)
        {
            db.Documentations.Add(documents);
            db.SaveChanges();
            //TÄMÄ ON FRONT SOVELLUKSELLE PALAUTETTEVA KUITTAUSVIESTI
            return Ok($"Lisätty uusi dokumentaatio: {documents.Description}");
        }

    }
}
