using System.ComponentModel;
using Android.Webkit;
using CustomRenderer;
using CustomRenderer.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer (typeof(HybridWebView), typeof(HybridWebViewRenderer))]
namespace CustomRenderer.Droid
{
	public class HybridWebViewRenderer : ViewRenderer<HybridWebView, Android.Webkit.WebView>
	{
		private readonly string JavaScriptFunction = $"function invokeCSharpAction(data){{{JSBridge.Name}.{nameof(JSBridge.InvokeAction)}(data);}}";

		protected override void OnElementChanged (ElementChangedEventArgs<HybridWebView> e)
		{
			base.OnElementChanged (e);

			if (Control == null) {
				var webView = new Android.Webkit.WebView (Forms.Context);
				webView.Settings.JavaScriptEnabled = true;
				SetNativeControl (webView);
			}
			if (e.OldElement != null) {
				Control.RemoveJavascriptInterface (JSBridge.Name);

				var hybridWebView = e.OldElement;
				hybridWebView.Cleanup();
			}
			if (e.NewElement != null) {
			    var hybridWebView = e.NewElement;
			    hybridWebView.RegisterSearchTextUpdate(this.UpdateSearchText);

				Control.AddJavascriptInterface (new JSBridge (this), JSBridge.Name);
				Control.LoadUrl ($"file:///android_asset/Content/{Element.Uri}");
				InjectJs (JavaScriptFunction);
			}
		}

	    private void UpdateSearchText(string s)
	    {
	        this.Control.LoadUrl("javascript:searchHighlight('" + s + "');");
        }

	    private void InjectJs (string script)
		{
			if (Control != null) {
				Control.LoadUrl ($"javascript: {script}");
			}
		}
	}
}
