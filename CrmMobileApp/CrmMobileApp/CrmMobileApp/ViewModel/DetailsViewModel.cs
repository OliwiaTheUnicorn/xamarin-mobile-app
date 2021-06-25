using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CrmMobileApp.Models;
using System.Windows.Input;
using System.Threading.Tasks;
using Xamarin.Forms;
using CrmMobileApp.ServicesHandler;
using Rg.Plugins.Popup.Services;
using CrmMobileApp.Views;
using Plugin.LocalNotification;
using System.Globalization;

namespace CrmMobileApp.ViewModel
{
    public class DetailsViewModel : INotifyPropertyChanged
    {

        private Offering selectedOffering;

        public Offering SelectedOffering
        {
            get => selectedOffering;
            set
            {
                if (selectedOffering == value)
                {
                    return;
                }
                selectedOffering = value;
                OnPropertyChanged(nameof(SelectedOffering));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        private List<Contract> contracts;
        private Contract selectedContract;
        private List<Lead> leads;
        private Lead selectedLeads;
        private List<Reminder> reminders;
        private Reminder selectedReminders;

        private bool _isRefreshing;

        public List<Contract> Contracts
        {
            get
            {
                return contracts;
            }
            set
            {
                contracts = value;
                OnPropertyChanged(nameof(Contracts));
            }
        }
        public Contract SelectedContract
        {
            get
            {
                return selectedContract;
            }
            set
            {
                selectedContract = value;
                OnPropertyChanged(nameof(SelectedContract));
            }
        }

        public List<Lead> Leads
        {
            get
            {
                return leads;
            }
            set
            {
                leads = value;
                OnPropertyChanged(nameof(Leads));
            }
        }
        public Lead SelectedLead
        {
            get
            {
                return selectedLeads;
            }
            set
            {
                selectedLeads = value;
                OnPropertyChanged(nameof(SelectedLead));
            }
        }

        public List<Reminder> Reminders
        {
            get
            {
                return reminders;
            }
            set
            {
                reminders = value;
                OnPropertyChanged(nameof(Reminders));
            }
        }
        public Reminder SelectedReminder
        {
            get
            {
                return selectedReminders;
            }
            set
            {
                selectedReminders = value;
                OnPropertyChanged(nameof(SelectedReminder));
            }
        }

        public bool IsRefreshing
        {
            get
            {
                return _isRefreshing;
            }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        public ICommand RefreshCommand { get; }

        public DetailsViewModel(bool IsContract, bool IsLead, bool IsReminder)
        {
            if (IsContract)
            {
                GetContracts();
            }
            if (IsLead)
            {
                GetLeads();
            }
            if (IsReminder)
            {
                GetReminders();
            }

            RefreshCommand = new Command(CmdRefresh);
        }

        public DetailsViewModel()
        {

            if (this.SelectedOffering !=null && this.SelectedOffering.IsContract)
            {
                GetContracts();
            }
            if (SelectedOffering != null && SelectedOffering.IsLead)
            {
                GetLeads();
            }
            if (SelectedOffering != null && SelectedOffering.IsReminder)
            {
                GetReminders();
            }

            RefreshCommand = new Command(CmdRefresh);
        }

        private void CmdRefresh()
        {
            if (this.SelectedOffering != null && this.SelectedOffering.IsContract)
            {
                Contracts.Clear();
                GetContracts();
            }
            if (SelectedOffering != null && SelectedOffering.IsLead)
            {
                Leads.Clear();
                GetLeads();
            }
            if (SelectedOffering != null && SelectedOffering.IsReminder)
            {
                Reminders.Clear();
                GetReminders();
            }
            IsRefreshing = false;
        }

        public void RefreshContracts()
        {
            Contracts.Clear();
            GetContracts();
        }

        private async void GetContracts()
        {
            ContractService contractService = new ContractService();
            if ((bool)Xamarin.Forms.Application.Current.Properties["IsLoggedIn"])
            {
                await PopupNavigation.Instance.PushAsync(new BusyPopupPage(), true);
                Contracts = await contractService.getContracts();
                await PopupNavigation.Instance.PopAllAsync();
            }
        }

        private async void GetLeads()
        {
            LeadService LeadService = new LeadService();
            if ((bool)Xamarin.Forms.Application.Current.Properties["IsLoggedIn"])
            {
                await PopupNavigation.Instance.PushAsync(new BusyPopupPage(), true);
                Leads = await LeadService.getLeads();
                await PopupNavigation.Instance.PopAllAsync();
            }
        }

        private async void GetReminders()
        {
            ReminderService reminderService = new ReminderService();
            if ((bool)Xamarin.Forms.Application.Current.Properties["IsLoggedIn"])
            {
                await PopupNavigation.Instance.PushAsync(new BusyPopupPage(), true);
                Reminders = await reminderService.getReminders();
                AddNotifications(Reminders);
                await PopupNavigation.Instance.PopAllAsync();
            }
        }

        private void AddNotifications(List<Reminder> reminders)
        {
            foreach (Reminder reminder in reminders)
            {
                CreateNotification(reminder);
            }
        }

        private void CreateNotification(Reminder reminder)
        {
            string date = reminder.Date;
            string time = reminder.Time;
            if (!string.IsNullOrWhiteSpace(date) && !string.IsNullOrWhiteSpace(time))
            {
                try
                {
                    string[] timeArray = time.Split(":");
                    string[] dateArray = date.Split("-");
                    int hour = int.Parse(timeArray[0], NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite);
                    int minute = int.Parse(timeArray[1], NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite);
                    int year = int.Parse(dateArray[0], NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite);
                    int month = int.Parse(dateArray[1], NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite);
                    int day = int.Parse(dateArray[2], NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite);

                    DateTime dateTime = new DateTime(year, month, day, hour, minute, 0);
                    var schedule = new NotificationRequestSchedule
                    {
                        NotifyTime = dateTime
                    };

                    var notification = new NotificationRequest
                    {
                        BadgeNumber = 1,
                        Description = reminder.Notes,
                        Title = "New Reminder: " + reminder.Id + " from Auther: " + reminder.Author,
                        ReturningData = "Reimnder Sent",
                        NotificationId = reminder.Id,
                        Schedule = schedule
                    };

                    _ = NotificationCenter.Current.Show(notification);
                }
                catch (Exception ex)
                {
                    // TODO
                    Console.WriteLine(ex);
                }
            }

        }
    }
}
