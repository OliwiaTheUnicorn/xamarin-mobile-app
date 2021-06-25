using CrmMobileApp.Models;
using CrmMobileApp.RestAPIClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CrmMobileApp.ServicesHandler
{
    class LoginService
    {
        // fetch the RestClient<T>
        RestClient<UserDetailCredentials> _restClient = new RestClient<UserDetailCredentials>();

        // Boolean function with the following parameters of username & password.
        public async Task<bool> CheckLoginIfExists(string username, string password)
        {
            var check = await _restClient.checkLogin(username, password);

            return check;
        }
    }
}
