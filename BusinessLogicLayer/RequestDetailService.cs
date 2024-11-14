using BusinessObjects.Entities;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class RequestDetailService : IRequestDetailService
    {
        private IRequestDetailRepository requestDetailRepository;

        public RequestDetailService()
        {
            requestDetailRepository = new RequestDetailDAO(); // Khởi tạo trực tiếp DAO
        }

        public RequestDetail GetRequestDetailById(int id)
        {
            return requestDetailRepository.GetRequestDetailById(id);
        }

        public IEnumerable<RequestDetail> GetAllRequestDetails()
        {
            return requestDetailRepository.GetAllRequestDetails();
        }

        public void CreateRequestDetail(RequestDetail requestDetail)
        {
            requestDetailRepository.AddRequestDetail(requestDetail);
        }

        public void UpdateRequestDetail(RequestDetail requestDetail)
        {
            requestDetailRepository.UpdateRequestDetail(requestDetail);
        }

        //public void DeleteRequestDetail(int id)
        //{
        //    requestDetailRepository.DeleteRequestDetail(id);
        //}

        public void DeleteRequestDetail(RequestDetail requestDetail)
        {
            requestDetailRepository.DeleteRequestDetail(requestDetail);
        }
    }

}
