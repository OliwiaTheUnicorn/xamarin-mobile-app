using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using CrmMobileApp.Models;
using CrmMobileApp.Views;
using CrmMobileApp.ServicesHandler;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Services;
using Plugin.LocalNotification;
using System.Globalization;
using System.Windows.Input;

namespace CrmMobileApp.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ICommand RefreshCommand { get; }

        public MainViewModel()
        {
            RefreshCommand = new Xamarin.Forms.Command(ExecuteRefreshCommand);
            MenuList = GetMenus();
            setOfferings();
        }

        bool isRefreshing;
        public bool IsRefreshing
        {
            get => isRefreshing;
            set
            {
                isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        void ExecuteRefreshCommand()
        {
            Offerings.Clear();
            setOfferings();
            IsRefreshing = false;
        }

        string username = Xamarin.Forms.Application.Current.Properties["Name"].ToString();
        public string Username
        {
            get => username;
            set 
            {
                if (username == value)
                {
                    return;
                }
                username = value;
                OnPropertyChanged(nameof(Username));
                OnPropertyChanged(nameof(DisplayName));
                OnPropertyChanged(nameof(HelloName));
            }
        }

        public string DisplayName => $"Username: {Username}";
        public string HelloName => $"Hello {Username}";
        public event PropertyChangedEventHandler PropertyChanged;
        
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private Offering selectedOffering;

        public Offering SelectedOffering
        {
            get { return selectedOffering; }
            set { selectedOffering = value; }
        }

        private ObservableCollection<Menu> menuList;
        public ObservableCollection<Menu> MenuList
        {
            get { return menuList; }
            set { menuList = value; }
        }

        private ObservableCollection<Offering> offerings;
        public ObservableCollection<Offering> Offerings
        {
            get => offerings;
            set
            {
                if (offerings == value)
                {
                    return;
                }
                offerings = value;
                OnPropertyChanged(nameof(Offerings));
                OnPropertyChanged(nameof(CrmOfferings));
            }
        }

        public ObservableCollection<Offering> CrmOfferings => Offerings;

        public void ShowDetails()
        {
            if (SelectedOffering.IsContract)
            {
                var page = new DetailsPage() { BindingContext = new DetailsViewModel(SelectedOffering.IsContract, SelectedOffering.IsLead, SelectedOffering.IsReminder) { SelectedOffering = SelectedOffering } };
                App.Current.MainPage.Navigation.PushAsync(page);
            }
            if (SelectedOffering.IsLead)
            {
              var page = new LeadDetailsPage() { BindingContext = new DetailsViewModel(SelectedOffering.IsContract, SelectedOffering.IsLead, SelectedOffering.IsReminder) { SelectedOffering = SelectedOffering } };
              App.Current.MainPage.Navigation.PushAsync(page);
            }
            if (SelectedOffering.IsReminder)
            {
                var page = new ReminderDetailPage() { BindingContext = new DetailsViewModel(SelectedOffering.IsContract, SelectedOffering.IsLead, SelectedOffering.IsReminder) { SelectedOffering = SelectedOffering } };
                App.Current.MainPage.Navigation.PushAsync(page);
            }
            //var page = new DetailsPage() { BindingContext = new DetailsViewModel { SelectedOffering = SelectedOffering } };

        }

        private ObservableCollection<Menu> GetMenus()
        {
            return new ObservableCollection<Menu>
            {
                new Menu { Icon = "logout.png", Name = "Logout"}
            };
        }

        private async void setOfferings()
        {
            Offerings =  await GetOfferings();
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

        private async Task<ObservableCollection<Offering>> GetOfferings()
        {
            double totalContracts = 0;
            double totalLeads = 0;
            double totalReminders = 0;
            ContractService contractService = new ContractService();
            LeadService leadService = new LeadService();
            ReminderService reminderService = new ReminderService();
            List<Contract> contracts = new List<Contract>();
            List<Lead> leads = new List<Lead>();
            List<Reminder> reminders = new List<Reminder>();
            if ((bool)Xamarin.Forms.Application.Current.Properties["IsLoggedIn"])
            {
                await PopupNavigation.Instance.PushAsync(new BusyPopupPage(), true);
                contracts = await contractService.getContracts();
                leads = await leadService.getLeads();
                reminders = await reminderService.getReminders();
                await PopupNavigation.Instance.PopAsync();
                if (contracts != null && contracts.Count > 0)
                {
                    totalContracts = contracts.Count;
                }

                if (leads != null && leads.Count > 0)
                {
                    totalLeads = leads.Count;
                }

                if (reminders != null && reminders.Count > 0)
                {
                    AddNotifications(reminders);
                    totalReminders = reminders.Count;
                }
            }

            return new ObservableCollection<Offering>
            {
                new Offering { Name = "Contracts", Total = totalContracts, Image = "contract.png", IsContract = true, IsLead = false, IsReminder = false, Contracts = contracts, Leads = null, Reminders = null},
                new Offering { Name = "Leads", Total = totalLeads, Image = "leads.png", IsContract = false, IsLead = true, IsReminder = false, Contracts = null, Leads = leads, Reminders = null},
                new Offering { Name = "Reminder", Total = totalReminders, Image = "reminder.png", IsContract = false, IsLead = false, IsReminder = true, Contracts = null, Leads = null, Reminders = reminders}
            };
        }
    }
}
