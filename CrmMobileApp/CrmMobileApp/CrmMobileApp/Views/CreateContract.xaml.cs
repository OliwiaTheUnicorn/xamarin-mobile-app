using CrmMobileApp.Models;
using CrmMobileApp.ServicesHandler;
using Rg.Plugins.Popup.Services;
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
    public partial class CreateContract : ContentPage
    {
        private Contract contract = new Contract();
        private ContractService ContractService = new ContractService();

        public CreateContract()
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
                    await DisplayAlert("Validation Error", "Full Name and Contract Value are required", "Ok");
                    return;
                }

                try
                {
                    contract.Author = Application.Current.Properties["UserId"].ToString();
                    await PopupNavigation.Instance.PushAsync(new BusyPopupPage(), true);
                    int contractId = await ContractService.createContract(contract);
                    await PopupNavigation.Instance.PopAllAsync();
                    if (contractId > 0)
                    {
                        string successMessage = "Contract has been created with id: " + contractId;
                        await DisplayAlert("Success", successMessage, "Ok");
                        //DetailsViewModel.GetLeads();
                    }
                    else
                    {
                        await DisplayAlert("Oops Something happened", "Creation of new contract has failed", "Ok");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", ex.Message, "Ok");
                }


                await Navigation.PopAsync();
            };

            entFullName.TextChanged += (object sender, TextChangedEventArgs e) =>
            {
                contract.Name = e.NewTextValue;
            };

            entLeadId.TextChanged += (object sender, TextChangedEventArgs e) =>
            {
                contract.LeadId = e.NewTextValue;
            };

            entContractValue.TextChanged += (object sender, TextChangedEventArgs e) =>
            {
                contract.ContractValue = e.NewTextValue;
            };

            entNotes.TextChanged += (object sender, TextChangedEventArgs e) =>
            {
                contract.Notes = e.NewTextValue;
            };
        }

        private bool IsNotValide()
        {
            return string.IsNullOrWhiteSpace(contract.Name) || string.IsNullOrWhiteSpace(contract.ContractValue);
        }

    }
}