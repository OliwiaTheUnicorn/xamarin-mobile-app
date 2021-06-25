using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CrmMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReminderDetailPage : ContentPage
    {
        public ReminderDetailPage()
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
            NotificationCenter.Current.NotificationTapped += Current_NotificationTapped;

            btnSave.Clicked += async (object sender, EventArgs e) =>
            {
                await Navigation.PushAsync(new CreateReminder());
            };
        }

        private void Current_NotificationTapped(NotificationTappedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                DisplayAlert("Notification tapped", e.Request.ReturningData, "OK");
            });
        }

    }
}