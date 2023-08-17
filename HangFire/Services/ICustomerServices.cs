using HangFire.Models;
using System.Data;

namespace HangFire.Services
{
    public interface ICustomerServices
    {
        void AddCustReview(Details detail);
        DataTable SyncData();
        List<Details> GetReviews();
    }
}
