using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CrmMobileApp.Models;
using System.Diagnostics;
using Xamarin.Forms;
using System.Net.Http.Headers;

namespace CrmMobileApp.RestAPIClient
{
    public class RestClient<T>
    {
        private const string MainWebServiceUrl = "https://odlcrm.azurewebsites.net/"; // Put your main host url here
        private const string LoginWebServiceUrl = MainWebServiceUrl + "api/login"; // put your api extension url/uri here
        private const string LeadsWebServiceUrl = MainWebServiceUrl + "api/leads";
        private const string ContractsWebServiceUrl = MainWebServiceUrl + "api/contracts";
        private const string RemindersWebServiceUrl = MainWebServiceUrl + "api/reminders";
        private const string NewLeadWebServiceUrl = MainWebServiceUrl + "api/newlead";
        private const string NewContractWebServiceUrl = MainWebServiceUrl + "api/newcontract";
        private const string NewReminderWebServiceUrl = MainWebServiceUrl + "api/newreminder";

        // This code matches the HTTP Request that we included in our api controller
        public async Task<bool> checkLogin(string username, string password)
        {
            if (IsNotLoggedIn())
            {
                var dict = new Dictionary<string, string>();
                dict.Add("email", username);
                dict.Add("password", password);
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, LoginWebServiceUrl) { Content = new FormUrlEncodedContent(dict) };
                var response = await client.SendAsync(request);
                bool isSuccess = response.IsSuccessStatusCode;
                if (isSuccess)
                {
                    try
                    {
                        var resp = await response.Content.ReadAsStringAsync();
                        UserDetailCredentials userCreds = JsonConvert.DeserializeObject<UserDetailCredentials>(resp);
                        String token = userCreds.Token;
                        if (String.IsNullOrEmpty(token))
                        {
                            isSuccess = false;
                        }
                        else
                        {
                            Application.Current.Properties["IsLoggedIn"] = true;
                            Application.Current.Properties["Name"] = userCreds.user.Name;
                            Application.Current.Properties["UserId"] = userCreds.user.Id;
                            Application.Current.Properties["Token"] = token;
                        }
                    }
                    catch (Exception e)
                    {
                        Trace.WriteLine(e);
                        isSuccess = false;
                    }

                }
                return isSuccess; // return either true or false 
            }
            return true;
        }

        private bool IsNotLoggedIn()
        {
            try {
                return (!(bool)Application.Current.Properties["IsLoggedIn"]
                    || string.IsNullOrWhiteSpace(Application.Current.Properties["Token"].ToString())
                    || string.IsNullOrWhiteSpace(Application.Current.Properties["Name"].ToString())
                    || string.IsNullOrWhiteSpace(Application.Current.Properties["UserId"].ToString()));
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            return true;
        }

        public async Task<List<Lead>> getLeads()
        {
            List<Lead> leads = new List<Lead>();
            var token = Application.Current.Properties["Token"].ToString();
            var authHeader = new AuthenticationHeaderValue("Bearer", token);
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = authHeader;

            var response = await client.GetAsync(LeadsWebServiceUrl);
            bool isSuccess = response.IsSuccessStatusCode;
            if (isSuccess)
            {
                try
                {
                    var resp = await response.Content.ReadAsStringAsync();
                    leads = JsonConvert.DeserializeObject<List<Lead>>(resp);
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e);
                    isSuccess = false;
                }

            }
            return leads; // return either true or false 
        }

        public async Task<List<Contract>> getContracts()
        {
            List<Contract> contracts = new List<Contract>();
            var token = Application.Current.Properties["Token"].ToString();
            var authHeader = new AuthenticationHeaderValue("Bearer", token);
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = authHeader;

            var response = await client.GetAsync(ContractsWebServiceUrl);
            bool isSuccess = response.IsSuccessStatusCode;
            if (isSuccess)
            {
                try
                {
                    var resp = await response.Content.ReadAsStringAsync();
                    contracts = JsonConvert.DeserializeObject<List<Contract>>(resp);
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e);
                    isSuccess = false;
                }

            }
            return contracts; // return either true or false 
        }

        public async Task<List<Reminder>> getReminders()
        {
            List<Reminder> reminders = new List<Reminder>();
            var token = Application.Current.Properties["Token"].ToString();
            var authHeader = new AuthenticationHeaderValue("Bearer", token);
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = authHeader;

            var response = await client.GetAsync(RemindersWebServiceUrl);
            bool isSuccess = response.IsSuccessStatusCode;
            if (isSuccess)
            {
                try
                {
                    var resp = await response.Content.ReadAsStringAsync();
                    reminders = JsonConvert.DeserializeObject<List<Reminder>>(resp);
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e);
                    isSuccess = false;
                }

            }
            return reminders; // return either true or false 
        }

        public async Task<int> createLead(Lead lead)
        {
            int leadId = -1;
            var token = Application.Current.Properties["Token"].ToString();
            var authHeader = new AuthenticationHeaderValue("Bearer", token);
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = authHeader;

            string json = JsonConvert.SerializeObject(lead);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(NewLeadWebServiceUrl, content);
            bool isSuccess = response.IsSuccessStatusCode;
            if (isSuccess)
            {
                try
                {
                    var resp = await response.Content.ReadAsStringAsync();
                    Lead newLead = JsonConvert.DeserializeObject<Lead>(resp);

                    if (newLead == null)
                    {
                        isSuccess = false;
                    }
                    else
                    {
                        leadId = newLead.Id;
                    }
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e);
                    isSuccess = false;
                }

            }
            return leadId; // return either true or false 
        }

        public async Task<int> createReminder(Reminder reminder)
        {
            int reminderId = -1;
            var token = Application.Current.Properties["Token"].ToString();
            var authHeader = new AuthenticationHeaderValue("Bearer", token);
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = authHeader;

            string json = JsonConvert.SerializeObject(reminder);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(NewReminderWebServiceUrl, content);
            bool isSuccess = response.IsSuccessStatusCode;
            if (isSuccess)
            {
                try
                {
                    var resp = await response.Content.ReadAsStringAsync();
                    Reminder newReminder = JsonConvert.DeserializeObject<Reminder>(resp);

                    if (newReminder == null)
                    {
                        isSuccess = false;
                    }
                    else
                    {
                        reminderId = newReminder.Id;
                    }
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e);
                    isSuccess = false;
                }

            }
            return reminderId; // return either true or false 
        }

        public async Task<int> createContract(Contract contract)
        {
            int contractId = -1;
            var token = Application.Current.Properties["Token"].ToString();
            var authHeader = new AuthenticationHeaderValue("Bearer", token);
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = authHeader;

            string json = JsonConvert.SerializeObject(contract);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(NewContractWebServiceUrl, content);
            bool isSuccess = response.IsSuccessStatusCode;
            if (isSuccess)
            {
                try
                {
                    var resp = await response.Content.ReadAsStringAsync();
                    Contract newContract = JsonConvert.DeserializeObject<Contract>(resp);

                    if (newContract == null)
                    {
                        isSuccess = false;
                    }
                    else
                    {
                        contractId = newContract.Id;
                    }
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e);
                    isSuccess = false;
                }

            }
            return contractId; // return either true or false 
        }
    }
}
