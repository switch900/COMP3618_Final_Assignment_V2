using System.Data.Entity.Infrastructure;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using IMDbDotNetAPI.Models;
using IMDbDotNetInfrastructure;

namespace IMDbDotNetAPI.Controllers
{
    public class titlebasicsController : ApiController
    {
        private UnitOfWork unitOfWork = new UnitOfWork(new IMDbEntities1());

        // GET: api/titlebasics
        //public IHttpActionResult Gettitlebasics(int startindex = 12, int pagesize = 12)
        //{
        //    var titlebasics = unitOfWork.Repository<titlebasic>().Reads(startindex, pagesize);
        //    if (titlebasics == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(titlebasics);
        //}


        // GET: api/titlebasics/5?startindex=12&pagesize=12
        [ResponseType(typeof(titlebasic))]
        public IHttpActionResult Gettitlebasic(string id, int startindex, int pagesize)
        {
            var titlebasics = unitOfWork.Repository<titlebasic>().Reads(id, startindex, pagesize);
            if (titlebasics == null)
            {
                return NotFound();
            }
            return Ok(titlebasics);
        }

        // GET: api/titlebasics
        [ResponseType(typeof(string))]
        public IHttpActionResult Gettitlebasic(string id)
        {
            var titlebasicCount = unitOfWork.Repository<titlebasic>().Reads(id);
            if (titlebasicCount == null)
            {
                return NotFound();
            }

            return Ok(titlebasicCount);
        }

        // PUT: api/titlebasics/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttitlebasic(string id, titlebasic titlebasic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != titlebasic.tconst)
            {
                return BadRequest();
            }

            unitOfWork.Repository<titlebasic>().Update(titlebasic);

            try
            {
                unitOfWork.Repository<titlebasic>().SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                titlebasic check = unitOfWork.Repository<titlebasic>().Read(x => x.tconst == titlebasic.tconst);
                if (check == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/titlebasics
        [ResponseType(typeof(titlebasic))]
        public IHttpActionResult Posttitlebasic(titlebasic titlebasic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            unitOfWork.Repository<titlebasic>().Create(titlebasic);
            

            try
            {
                unitOfWork.Repository<titlebasic>().SaveChanges();
            }
            catch (DbUpdateException)
            {
                titlebasic check = unitOfWork.Repository<titlebasic>().Read(x => x.tconst == titlebasic.tconst);
                if (check != null)
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = titlebasic.tconst }, titlebasic);
        }

        // DELETE: api/titlebasics/5
        [ResponseType(typeof(titlebasic))]
        public IHttpActionResult Deletetitlebasic(string id)
        {
            titlebasic titlebasic = unitOfWork.Repository<titlebasic>().Read(x => x.tconst == id);
            if (titlebasic == null)
            {
                return NotFound();
            }

            unitOfWork.Repository<titlebasic>().Delete(titlebasic);
            unitOfWork.Repository<titlebasic>().SaveChanges();

            return Ok(titlebasic);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}