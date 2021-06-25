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
    public partial class BusyPopupPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        public BusyPopupPage()
        {
            InitializeComponent();
        }
    }
}