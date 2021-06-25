using CrmMobileApp.Views;
using Plugin.SharedTransitions;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CrmMobileApp
{
    public partial class App : Application
    {
        public App()
        {
            Xamarin.Forms.DataGrid.DataGridComponent.Init();
            InitializeComponent();
            Device.SetFlags(new[] { "Shapes_Experimental" });
            EnableLogin();
            //DisableLogin();
        }

        private void EnableLogin()
        {
            Application.Current.Properties["Token"] = "";
            Application.Current.Properties["IsLoggedIn"] = false;
            Application.Current.Properties["Name"] = "";
            Application.Current.Properties["UserId"] = "";
            MainPage = new SharedTransitionNavigationPage(new MainPage());
        }

        private void DisableLogin()
        {
            Application.Current.Properties["Token"] = "90|B8ri04wuMYuP72EP8iGpUSqXUuAePBuVcr4O3xTi";
            Application.Current.Properties["IsLoggedIn"] = true;
            Application.Current.Properties["Name"] = "testUser";
            Application.Current.Properties["UserId"] = "1";
            MainPage = new SharedTransitionNavigationPage(new Dashboard());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
