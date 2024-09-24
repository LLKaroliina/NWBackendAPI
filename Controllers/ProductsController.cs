using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NWBackendAPI.Models;

namespace NWBackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly northwindOriginalContext db = new northwindOriginalContext();

        public ProductsController(northwindOriginalContext dbparametri)
        {
            db = dbparametri;
        }
        //HAKEE KAIKKI TUOTTEET
        [HttpGet]
        public ActionResult GetAllProducts()
        {

            List<Product> tuotteet = db.Products.ToList();
            return Ok(tuotteet);
        }
        //hAKU TUOTE ID MUKAAN
        [HttpGet("{id}")]
        public ActionResult GetProductById(int id)
        {

            try
            {
                var tuote = db.Products.Find(id);

                if (tuote == null) //JOS ID:TÄ EI LÖYDY
                {

                    return NotFound($"Tuotetta id:llä {id} ei löytynyt");
                }
                // RETURN VOI KUMOTA ELSEN. EI OLE PAKKO LAITTAA
                return Ok(tuote);
            }
            catch (Exception ex)
            {
                return BadRequest($"Tapahtui virhe. Lue lisää: {ex.Message}");
            }

        }
        //HAKU TUOTENIMELLÄ
        [HttpGet("productname/{pname}")]
        public ActionResult SearchProductByProductName(string pname)
        {

            var tuotteet = db.Products.Where(p => p.ProductName.Contains(pname));
            return Ok(tuotteet);

        }
        //UUDEN TUOTTEEN LISÄÄMINEN
        [HttpPost]
        public ActionResult AddNewProduct([FromBody] Product product)
        {

            db.Products.Add(product);
            db.SaveChanges();
            //TÄMÄ ON FRONT SOVELLUKSELLE PALAUTETTEVA KUITTAUSVIESTI
            return Ok($"Lisätty uusi tuote: {product.ProductName}");
        }
        //TUOTTEEN POISTAMINEN
        [HttpDelete("{id}")]
        public ActionResult RemoveProductById(int id)
        {

            try
            {

                var tuote = db.Products.Find(id);

                if (tuote != null)
                {

                    db.Products.Remove(tuote);
                    db.SaveChanges();

                    return Ok($"Poistettiin tuote: {tuote.ProductName}");


                }
                else
                {
                    return NotFound($"Tuotetta id:llä {id} ei löytynyt poistettavaksi");

                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Tapahtui virhe. Lue lisää: {ex.Message}");
            }

        }
        //TUOTTEEN MUOKKAUS
        [HttpPut("{id})")]

        public ActionResult EditProduct(int id, [FromBody] Product product)
        {

            var tuote = db.Products.Find(id);
            if (tuote != null)
            {

                tuote = product;
                db.SaveChanges();
                return Ok("Muokattu tuotetta " + tuote.ProductName);
            }
            else
            {
                return NotFound("Tuotetta ei löytynyt id:llä" + id);
            }

        }
    }
}
