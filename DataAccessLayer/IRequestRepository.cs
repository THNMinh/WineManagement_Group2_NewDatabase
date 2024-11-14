using BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IRequestRepository
    {
        Request GetRequestById(int id);
        IEnumerable<Request> GetAllRequests();
        void AddRequest(Request request);
        void UpdateRequest(Request request);
        void DeleteRequest(int id);
    }
}
