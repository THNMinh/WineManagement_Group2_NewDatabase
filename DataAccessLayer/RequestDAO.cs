using BusinessObjects;
using BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class RequestDAO : IRequestRepository
    {
        public Request GetRequestById(int id)
        {
            using (var db = new WineManagement2Context())
            {
                return db.Requests.Find(id);
            }
        }

        public IEnumerable<Request> GetAllRequests()
        {
            using (var db = new WineManagement2Context())
            {
                return db.Requests.ToList();
            }
        }

        public void AddRequest(Request request)
        {
            using (var db = new WineManagement2Context())
            {
                db.Requests.Add(request);
                db.SaveChanges();
            }
        }

        public void UpdateRequest(Request request)
        {
            using (var db = new WineManagement2Context())
            {
                db.Requests.Update(request);
                db.SaveChanges();
            }
        }

        public void DeleteRequest(int id)
        {
            using (var db = new WineManagement2Context())
            {
                var request = db.Requests.Find(id);
                if (request != null)
                {
                    db.Requests.Remove(request);
                    db.SaveChanges();
                }
            }
        }
    }

}
