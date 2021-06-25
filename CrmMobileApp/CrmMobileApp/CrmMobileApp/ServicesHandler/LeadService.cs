using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CrmMobileApp.Models;
using CrmMobileApp.RestAPIClient;

namespace CrmMobileApp.ServicesHandler
{
    public class LeadService
    {
        // fetch the RestClient<T>
        RestClient<Lead> _restClient = new RestClient<Lead>();

        // Boolean function with the following parameters of username & password.
        public async Task<List<Lead>> getLeads()
        {
            List<Lead> leads = await _restClient.getLeads();

            return leads;
        }

        public async Task<int> createLead(Lead lead)
        {
            int leadId = await _restClient.createLead(lead);

            return leadId;
        }
    }
}
