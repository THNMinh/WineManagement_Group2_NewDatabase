using BusinessObjects;
using BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class RequestDetailDAO : IRequestDetailRepository
    {
        public RequestDetail GetRequestDetailById(int id)
        {
            using (var db = new WineManagement2Context())
            {
                return db.RequestDetails.Find(id);
            }
        }

        public IEnumerable<RequestDetail> GetAllRequestDetails()
        {
            using (var db = new WineManagement2Context())
            {
                return db.RequestDetails.ToList();
            }
        }

        public void AddRequestDetail(RequestDetail requestDetail)
        {
            using (var db = new WineManagement2Context())
            {
                db.RequestDetails.Add(requestDetail);
                db.SaveChanges();
            }
        }

        public void UpdateRequestDetail(RequestDetail requestDetail)
        {
            using (var db = new WineManagement2Context())
            {
                db.RequestDetails.Update(requestDetail);
                db.SaveChanges();
            }
        }

        //public void DeleteRequestDetail(int id)
        //{
        //    using (var db = new WineManagement2Context())
        //    {
        //        var requestDetail = db.RequestDetails.Find(id);
        //        if (requestDetail != null)
        //        {
        //            db.RequestDetails.Remove(requestDetail);
        //            db.SaveChanges();
        //        }
        //    }
        //}

        public void DeleteRequestDetail(RequestDetail requestDetail)
        {
            using (var db = new WineManagement2Context())
            {
                db.RequestDetails.Remove(requestDetail);
                db.SaveChanges();
            }
        }
    }

}
