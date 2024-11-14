using BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IRequestDetailRepository
    {
        RequestDetail GetRequestDetailById(int id);
        IEnumerable<RequestDetail> GetAllRequestDetails();
        void AddRequestDetail(RequestDetail requestDetail);
        void UpdateRequestDetail(RequestDetail requestDetail);
        //void DeleteRequestDetail(int id);
        void DeleteRequestDetail(RequestDetail requestDetail);
    }
}
