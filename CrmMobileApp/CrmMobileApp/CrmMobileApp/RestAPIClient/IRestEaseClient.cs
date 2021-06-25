using CrmMobileApp.Models;
using RestEase;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CrmMobileApp.RestAPIClient
{
    public interface IRestEaseClient
    {
        [Post("newlead")]
        Task<Lead> AddLeadAsync([Body] Lead lead, [Header("Authorization")] string authorization);

        [Post("newlead")]
        Task<Lead> AddLeadAsyncNew([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> data, [Header("Authorization")] string authorization);
    }
}
