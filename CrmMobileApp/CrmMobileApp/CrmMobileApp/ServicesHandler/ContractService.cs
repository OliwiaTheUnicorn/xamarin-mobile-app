using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CrmMobileApp.Models;
using CrmMobileApp.RestAPIClient;

namespace CrmMobileApp.ServicesHandler
{
    public class ContractService
    {
        // fetch the RestClient<T>
        RestClient<Contract> _restClient = new RestClient<Contract>();

        // Boolean function with the following parameters of username & password.
        public async Task<List<Contract>> getContracts()
        {
            List<Contract> contracts = await _restClient.getContracts();

            return contracts;
        }

        public async Task<int> createContract(Contract contract)
        {
            int contractId = await _restClient.createContract(contract);

            return contractId;
        }
    }
}
