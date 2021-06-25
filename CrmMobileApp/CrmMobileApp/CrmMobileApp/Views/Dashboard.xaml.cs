using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Plugin.SharedTransitions;
using System.ComponentModel;
using Rg.Plugins.Popup.Services;

namespace CrmMobileApp.Views
{
    [DesignTimeVisible(false)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Dashboard : ContentPage
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void OpenMenu()
        {
            MenuGrid.IsVisible = true;

            Action<double> callback = input => MenuView.TranslationX = input;
            MenuView.Animate("anim", callback, -260, 0, 16, 300, Easing.CubicInOut);
        }

        private void CloseMenu()
        {
            Action<double> callback = input => MenuView.TranslationX = input;
            MenuView.Animate("anim", callback, 0, -260, 16, 300, Easing.CubicInOut);

            MenuGrid.IsVisible = false;
        }


        private void MenuTapped(object sender, EventArgs e)
        {
            OpenMenu();
        }

        private void OverlayTapped(object sender, EventArgs e)
        {
            CloseMenu();
        }

        private void OfferingSelected(object sender, SelectionChangedEventArgs e)
        {
            SharedTransitionNavigationPage.SetTransitionSelectedGroup(this, vm.SelectedOffering.Name);
            vm.ShowDetails();
        }

        public async void Logout(object sender, EventArgs e)
        {
            Application.Current.Properties["Token"] = "";
            Application.Current.Properties["IsLoggedIn"] = false;
            Application.Current.Properties["Name"] = "";
            Application.Current.Properties["UserId"] = "";
            await Navigation.PopAsync();
            await Navigation.PushAsync(new MainPage());
        }
    }
}