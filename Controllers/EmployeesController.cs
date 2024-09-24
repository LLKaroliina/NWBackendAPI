using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NWBackendAPI.Models;

namespace NWBackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly northwindOriginalContext db = new northwindOriginalContext();
        public EmployeesController(northwindOriginalContext dbparametri)
        {
            db = dbparametri;
        }
        //HAKEE KAIKKI TYÖNTEKIJÄT
        [HttpGet]
        public ActionResult GetAllEmployees()
        {
            var työntekijät = db.Employees.ToList();
            return Ok(työntekijät);
            //List<Employee> työntekijät = db.Employees.ToList();
            //return Ok(työntekijät);
        }
        //HAKU  ID MUKAAN
        [HttpGet("{id}")]
        public ActionResult GetEmployeeById(int id)
        {

            try
            {
                var työntekijä = db.Employees.Find(id);

                if (työntekijä == null) //JOS ID:TÄ EI LÖYDY
                {

                    return NotFound($"Työntekijää id:llä {id} ei löytynyt");
                }
                // RETURN VOI KUMOTA ELSEN. EI OLE PAKKO LAITTAA
                return Ok(työntekijä);
            }
            catch (Exception ex)
            {
                return BadRequest($"Tapahtui virhe. Lue lisää: {ex.Message}");
            }

        }
        //HAKU ETUNIMELLÄ
        [HttpGet("lastname/{lname}")]
        public ActionResult SearchEmployeeByLastName(string lname)
        {

            var työntekijät = db.Employees.Where(e => e.LastName.Contains(lname));
            return Ok(työntekijät);

        }
        //UUDEN EMPLOYEE LISÄÄMINEN
        [HttpPost]
        public ActionResult AddNewEmployee([FromBody] Employee employee)
        {

            db.Employees.Add(employee);
            db.SaveChanges();
            //TÄMÄ ON FRONT SOVELLUKSELLE PALAUTETTEVA KUITTAUSVIESTI
            return Ok($"Lisätty uusi työntekijä: {employee.FirstName} {employee.LastName}");
        }
        //EMPLOYEE POISTAMINEN
        [HttpDelete("{id}")]
        public ActionResult RemoveEmployeeById(int id)
        {

            try
            {

                var työntekijä = db.Employees.Find(id);

                if (työntekijä != null)
                {

                    db.Employees.Remove(työntekijä);
                    db.SaveChanges();

                    return Ok($"Poistettiin työntekijä: {työntekijä.FirstName} {työntekijä.LastName}");


                }
                else
                {
                    return NotFound($"Työntekijää id:llä {id} ei löytynyt poistettavaksi");

                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Tapahtui virhe. Lue lisää: {ex.Message}");
            }

        }
        //TYÖNTEKIJÄN TIETOJEN MUOKKAUS
        [HttpPut("{id})")]

        public ActionResult EditEmployee(int id, [FromBody] Employee employee)
        {

            var työntekijä = db.Employees.Find(id);
            if (työntekijä != null)
            {

                työntekijä = employee;
                db.SaveChanges();
                return Ok($"Muokattu työntekijän {työntekijä.FirstName} {työntekijä.LastName} tietoja");
            }
            else
            {
                return NotFound("Työntekijää ei löytynyt id:llä" + id);
            }

        }

    }
}
