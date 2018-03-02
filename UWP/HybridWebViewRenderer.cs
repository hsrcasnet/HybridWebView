using System;
using Windows.UI.Xaml.Controls;
using CustomRenderer;
using CustomRenderer.UWP;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(HybridWebView), typeof(HybridWebViewRenderer))]

namespace CustomRenderer.UWP
{
    public class HybridWebViewRenderer : ViewRenderer<HybridWebView, WebView>
    {
        private const string JavaScriptFunction = "function invokeCSharpAction(data){window.external.notify(data);}";

        protected override void OnElementChanged(ElementChangedEventArgs<HybridWebView> e)
        {
            base.OnElementChanged(e);

            if (Control == null) SetNativeControl(new WebView());

            if (e.OldElement != null)
            {
                Control.NavigationCompleted -= OnWebViewNavigationCompleted;
                Control.ScriptNotify -= OnWebViewScriptNotify;

                var hybridWebView = e.OldElement;
                hybridWebView.Cleanup();
            }

            if (e.NewElement != null)
            {
                var hybridWebView = e.NewElement;
                hybridWebView.RegisterSearchTextUpdate(UpdateSearchText);

                Control.NavigationCompleted += OnWebViewNavigationCompleted;
                Control.ScriptNotify += OnWebViewScriptNotify;
                Control.Source = new Uri(string.Format("ms-appx-web:///Content//{0}", Element.Uri));
            }
        }

        private async void UpdateSearchText(string s)
        {
            await Control.InvokeScriptAsync("searchHighlight", new[] {s});
        }

        private async void OnWebViewNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            if (args.IsSuccess)
            {
                await Control.InvokeScriptAsync("eval", new[] {JavaScriptFunction});
            }
        }

        private void OnWebViewScriptNotify(object sender, NotifyEventArgs e)
        {
            Element.CallbackAction(e.Value);
        }
    }
}