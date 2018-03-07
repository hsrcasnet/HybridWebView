using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CustomRenderer
{
    public partial class HybridWebViewPage : ContentPage
    {
        public HybridWebViewPage()
        {
            this.InitializeComponent();

            this.HybridWebView.RegisterCallbackAction(data => this.DisplayAlert("CallbackAction", "data= " + data, "OK"));
            this.HybridWebView.SearchText = "";
        }

        private async Task OnSearch()
        {
            try
            {
                await this.HybridWebView.UpdateSearchText(this.SearchBar.Text);
            }
            catch (Exception ex)
            {
                await this.DisplayAlert("UpdateSearchText", $"Exception: {ex}", "OK");
            }
        }

        private async void FindNextButtonClicked(object sender, EventArgs e)
        {
            await this.OnSearch();
        }
    }
}