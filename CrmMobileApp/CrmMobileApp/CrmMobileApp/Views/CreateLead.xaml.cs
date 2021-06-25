using CrmMobileApp.Models;
using CrmMobileApp.ServicesHandler;
using CrmMobileApp.ViewModel;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CrmMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateLead : ContentPage
    {
        private Lead lead = new Lead();
        private LeadService LeadService = new LeadService();
        public CreateLead()
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
                
                if (IsNotValide())
                {
                    await DisplayAlert("Validation Error", "Full Name, Valid Email and Phone Number are required", "Ok");
                    return;
                }

                try
                {
                    lead.Author = Application.Current.Properties["UserId"].ToString();
                    await PopupNavigation.Instance.PushAsync(new BusyPopupPage(), true);
                    lead.LocationCooridinates = await GetCurrentLocation();
                    int leadId = await LeadService.createLead(lead);
                    await PopupNavigation.Instance.PopAllAsync();
                    if (leadId > 0)
                    {
                        string successMessage = "Lead has been created with id: " + leadId;
                        await DisplayAlert("Success", successMessage, "Ok");
                        //DetailsViewModel.GetLeads();
                    } else
                    {
                        await DisplayAlert("Oops Something happened", "Creation of new lead has failed", "Ok");
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
                lead.Name = e.NewTextValue;
            };

            entEmail.TextChanged += (object sender, TextChangedEventArgs e) =>
            {
                lead.Email = e.NewTextValue;
            };

            entPhoneNumber.TextChanged += (object sender, TextChangedEventArgs e) =>
            {
                lead.Phone = e.NewTextValue;
            };

            entNotes.TextChanged += (object sender, TextChangedEventArgs e) =>
            {
                lead.Notes = e.NewTextValue;
            };
        }

        private bool IsNotValide()
        {
            return string.IsNullOrWhiteSpace(lead.Name) || IsNotValidEmail(lead.Email) || string.IsNullOrWhiteSpace(lead.Phone);
        }

        private bool IsNotValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return true;
            }

            try
            {
                return !(Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)));
            }
            catch (RegexMatchTimeoutException)
            {
                return true;
            }
        }

        async Task<string> GetCurrentLocation()
        {
            string location_coordinates = "0,0,0";
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);

                if (location != null)
                {
                    location_coordinates = location.Latitude + "," + location.Longitude + "," + location.Altitude;
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                }
            }
            catch (Exception ex)
            {
                // Unable to get location
                Console.WriteLine("Error while fetching location", ex);
            }

            return location_coordinates;
        }

        CancellationTokenSource cts;

        protected override void OnDisappearing()
        {
            if (cts != null && !cts.IsCancellationRequested)
                cts.Cancel();
            base.OnDisappearing();
        }

    }
}