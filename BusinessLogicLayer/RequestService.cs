using BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class RequestService : IRequestService
    {
        private IRequestRepository requestRepository;

        public RequestService()
        {
            requestRepository = new RequestDAO(); // Khởi tạo trực tiếp DAO
        }

        public Request GetRequestById(int id)
        {
            return requestRepository.GetRequestById(id);
        }

        public IEnumerable<Request> GetAllRequests()
        {
            return requestRepository.GetAllRequests();
        }

        public void CreateRequest(Request request)
        {
            requestRepository.AddRequest(request);
        }

        public void UpdateRequest(Request request)
        {
            requestRepository.UpdateRequest(request);
        }

        public void DeleteRequest(int id)
        {
            requestRepository.DeleteRequest(id);
        }
    }

}
