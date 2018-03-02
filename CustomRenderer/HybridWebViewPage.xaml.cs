using System;
using Xamarin.Forms;

namespace CustomRenderer
{
    public partial class HybridWebViewPage : ContentPage
    {
        public HybridWebViewPage()
        {
            this.InitializeComponent();

            this.SearchBar.SearchCommand = new Command(OnSearch);
            this.HybridWebView.RegisterCallbackAction(data => this.DisplayAlert("CallbackAction", "data= " + data, "OK"));
            this.HybridWebView.SearchText = "";
        }

        private void OnSearch()
        {
            this.HybridWebView.UpdateSearchText(this.SearchBar.Text);
        }

        private void FindNextButtonClicked(object sender, EventArgs e)
        {
            this.OnSearch();
        }
    }
}