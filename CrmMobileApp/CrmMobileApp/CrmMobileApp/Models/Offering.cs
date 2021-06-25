using System;
using System.Collections.Generic;
using System.Text;

namespace CrmMobileApp.Models
{
    public class Offering
    {
        public string Name { get; set; }
        public double Total { get; set; }
        public string Image { get; set; }
        public bool IsLead { get; set; }
        public bool IsContract { get; set; }
        public bool IsReminder { get; set; }
        public List<Reminder> Reminders { get; set; }
        public List<Lead> Leads { get; set; }
        public List<Contract> Contracts { get; set; }
    }
}