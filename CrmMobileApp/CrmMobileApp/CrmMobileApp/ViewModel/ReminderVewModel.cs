using CrmMobileApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CrmMobileApp.ViewModel
{
    public class ReminderVewModel
    {
        public IList<ReminderStatus> ReminderStatusList { get; set; }

        public ReminderVewModel()
        {
            try
            {
                ReminderStatusList = new ObservableCollection<ReminderStatus>();
                ReminderStatusList.Add(new ReminderStatus { Name = "New" });
                ReminderStatusList.Add(new ReminderStatus { Name = "InProgress" });
                ReminderStatusList.Add(new ReminderStatus { Name = "Done" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
