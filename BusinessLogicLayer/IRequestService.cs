using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Entities;
namespace BusinessLogicLayer
{
    public interface IRequestService
    {
        Request GetRequestById(int id);
        IEnumerable<Request> GetAllRequests();
        void CreateRequest(Request request);
        void UpdateRequest(Request request);
        void DeleteRequest(int id);
    }

}
