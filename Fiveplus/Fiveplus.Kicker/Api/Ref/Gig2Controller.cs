using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Fiveplus.Data.DbContexts;
using Fiveplus.Data.Models;
using Microsoft.AspNet.Identity;

namespace Fiveplus.Kicker.Api
{
 
    public class Gig2Controller : ApiController
    {
        private FiveplusContext db = new FiveplusContext();

        // GET: api/Gig2Controller
        public IQueryable<Gig> GetGigs()
        {
            return db.Gigs.Include(c => c.AddonServices);
        }

        // GET: api/Gig2Controller/5
        [ResponseType(typeof(Gig))]
        public async Task<IHttpActionResult> GetGig(int id)
        {
            User.Identity.GetUserId();

            Gig gig = await db.Gigs.FindAsync(id);
            if (gig == null)
            {
                return NotFound();
            }

            return Ok(gig);
        }

        // PUT: api/Gig2Controller/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutGig(int id, Gig gig)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != gig.Id)
            {
                return BadRequest();
            }

            db.Entry(gig).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GigExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

          //  return StatusCode(HttpStatusCode.OK);
            return Ok(gig);

        }

        // POST: api/Gig2Controller
        [ResponseType(typeof(Gig))]
        public async Task<IHttpActionResult> PostGig(Gig gig)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Gigs.Add(gig);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = gig.Id }, gig);
        }

        // DELETE: api/Gig2Controller/5
        [ResponseType(typeof(Gig))]
        public async Task<IHttpActionResult> DeleteGig(int id)
        {
            Gig gig = await db.Gigs.FindAsync(id);
            if (gig == null)
            {
                return NotFound();
            }

            db.Gigs.Remove(gig);
            await db.SaveChangesAsync();

            return Ok(gig);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GigExists(int id)
        {
            return db.Gigs.Count(e => e.Id == id) > 0;
        }
    }
}