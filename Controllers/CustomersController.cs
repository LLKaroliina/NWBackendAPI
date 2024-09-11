using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NWBackendAPI.Models;

namespace NWBackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        //LUODAAN INSTANSSI TIETOKANTAKONTEKSTILUOKASTA. MYÖS DB= NEW()
        NorthwindOriginalContext db = new NorthwindOriginalContext();
        //HAKEE KAIKKI ASIAKKAAT
        [HttpGet]
        //PALAUTETAAN STATUS KOODI JA MUKANA KULKEE JOKU VIESTI
        public ActionResult GetAllCustomers()
        {
            //var asiakkaat = db.Customers.ToList();
            //return Ok(asiakkaat);
            List<Customer> asiakkaat = db.Customers.ToList();
            return Ok(asiakkaat);
        }
        //HAKEE ASIAKKAAN PÄÄAVAIMELLE ELI CUSTOMER ID:LLÄ
        [HttpGet("{id}")]
        public ActionResult GetCustomerById(string id) //TARKISTA TAULUKOHTAINEN ID TIETOTYYPPI
        {
            //TRY CATCH VIRHEENKÄSITTELY
            try
            {


                //FIND ON PÄÄAVAINTA VARTEN
                var asiakas = db.Customers.Find(id);
                //WHERE VOI KÄYTTÄÄ MISSÄ TILANTEESSA VAIN 
                //var asiakas = db.Customers.Where(c => c.CustomerId == id);
                if (asiakas == null) //JOS ID:TÄ EI LÖYDY
                {
                    //return NotFound("Asiakasta id:llä" + id + " ei löytynyt");
                    //TAI STRING INTERPOTATION TYYLI. $ LIITTÄÄ MUUTTUJAN ARVOJA MERKKIJONOON
                    return NotFound($"Asiakasta id:llä {id} ei löytynyt");
                }
                // RETURN VOI KUMOTA ELSEN. EI OLE PAKKO LAITTAA
                return Ok(asiakas);
            }
            catch (Exception ex)
            { 
                return BadRequest($"Tapahtui virhe. Lue lisää: {ex.Message}");
            }

        }
        //UUDEN LISÄÄMINEN
        [HttpPost]
        public ActionResult AddNewCustomer([FromBody] Customer customer)
        {
            //TALLENNETAAN UUSI ASIAKAS KAHDELLA AO RIVEILLÄ
            db.Customers.Add(customer);
            db.SaveChanges();
            //TÄMÄ ON FRONT SOVELLUKSELLE PALAUTETTEVA KUITTAUSVIESTI
            return Ok($"Lisätty uusi asiakas: {customer.CompanyName}");
        }
        //ASIAKKAAN POISTAMINEN URRL PARAMETRINA ANNETTAVAN ID:N PERUSTEELLA
        [HttpDelete("{id}")]
        public ActionResult RemoveCustomerById(string id) 
        {
            //TRY CATCH VIRHEENKÄSITTELY
            try
            {

                var asiakas = db.Customers.Find(id);
                
                if (asiakas != null) //JOS ASIAKAS EI OLE NULL
                {
                    //ASIAKKAAN POISTO TIETOKANNASTA
                    db.Customers.Remove(asiakas);
                    db.SaveChanges();
                    //VIESTI FRONTTISOVELLUKSELLE
                    return Ok($"Poistettiin asiakas: {asiakas.CompanyName}");

                   
                }
                else
                {
                    return NotFound($"Asiakasta id:llä {id} ei löytynyt poistettavaksi");
                    
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Tapahtui virhe. Lue lisää: {ex.Message}");
            }

        }

    }
}
