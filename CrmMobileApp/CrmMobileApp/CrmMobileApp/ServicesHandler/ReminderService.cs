using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CrmMobileApp.Models;
using CrmMobileApp.RestAPIClient;

namespace CrmMobileApp.ServicesHandler
{
    public class ReminderService
    {
        // fetch the RestClient<T>
        RestClient<Reminder> _restClient = new RestClient<Reminder>();

        // Boolean function with the following parameters of username & password.
        public async Task<List<Reminder>> getReminders()
        {
            List<Reminder> reminders = await _restClient.getReminders();

            return reminders;
        }

        public async Task<int> createReminder(Reminder reminder)
        {
            int reminderId = await _restClient.createReminder(reminder);
            return reminderId;
        }
    }
}
