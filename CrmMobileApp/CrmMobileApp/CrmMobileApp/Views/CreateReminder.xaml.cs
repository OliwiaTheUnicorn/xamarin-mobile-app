using CrmMobileApp.Models;
using CrmMobileApp.ServicesHandler;
using Plugin.LocalNotification;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CrmMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateReminder : ContentPage
    {
        private Reminder reminder = new Reminder();
        private ReminderService reminderService = new ReminderService();
        public CreateReminder()
        {
            InitializeComponent();
            IntializeEvents();
        }

        private void BackTapped(object sender, EventArgs e)
        {
            this.Navigation.PopAsync();
        }

        private void IntializeEvents()
        {
            btnSave.Clicked += async (object sender, EventArgs e) =>
            {
                reminder.Date = entDate.Date.ToString("yyyy'-'MM'-'dd");
                reminder.Time = entTime.Time.ToString();
                reminder.Status = ((ReminderStatus)entStatus.SelectedItem).Name;
                if (IsNotValidate())
                {
                    await DisplayAlert("Validation Error", "Full Name, Status, Valid Date and Time are required.", "Ok");
                    return;
                }

                try
                {
                    reminder.Author = Application.Current.Properties["UserId"].ToString();
                    await PopupNavigation.Instance.PushAsync(new BusyPopupPage(), true);
                    int reminderId = await reminderService.createReminder(reminder);
                    await PopupNavigation.Instance.PopAllAsync();
                    if (reminderId > 0)
                    {
                        string successMessage = "Reminder has been created with id: " + reminderId;
                        await DisplayAlert("Success", successMessage, "Ok");
                        CreateNotification(reminderId);
                    }
                    else
                    {
                        await DisplayAlert("Oops Something happened", "Creation of new reminder has failed", "Ok");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", ex.Message, "Ok");
                }


                await Navigation.PopAsync();
                //await Navigation.PushAsync(new Dashboard());
            };

            entFullName.TextChanged += (object sender, TextChangedEventArgs e) =>
            {
                reminder.Name = e.NewTextValue;
            };

            entStatus.PropertyChanged += (object sender, PropertyChangedEventArgs e) =>
            {
                if (entStatus.SelectedItem != null)
                {
                    reminder.Status = ((ReminderStatus)entStatus.SelectedItem).Name;
                }
            };

            entDate.DateSelected += (object sender, DateChangedEventArgs e) =>
            {
                reminder.Date = e.NewDate.Date.ToString("yyyy'-'MM'-'dd");
            };

            entTime.PropertyChanged += (object sender, PropertyChangedEventArgs e) =>
            {
                reminder.Time = entTime.Time.ToString();
            };

            entNotes.TextChanged += (object sender, TextChangedEventArgs e) =>
            {
                reminder.Notes = e.NewTextValue;
            };
        }

        private void CreateNotification(int id)
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
                        Title = "New Reminder: " + id + " from Auther: " + reminder.Author,
                        ReturningData = "Reimnder Sent",
                        NotificationId = id,
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

        private bool IsNotValidate()
        {
            return string.IsNullOrWhiteSpace(reminder.Name) || string.IsNullOrWhiteSpace(reminder.Date) || string.IsNullOrWhiteSpace(reminder.Time) || string.IsNullOrWhiteSpace(reminder.Status);
        }
    }
}