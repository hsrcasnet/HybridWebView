using System.IO;
using System.Threading.Tasks;
using CustomRenderer;
using CustomRenderer.iOS;
using Foundation;
using UIKit;
using WebKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer (typeof(HybridWebView), typeof(HybridWebViewRenderer))]
namespace CustomRenderer.iOS
{
	public class HybridWebViewRenderer : ViewRenderer<HybridWebView, WKWebView>, IWKScriptMessageHandler
	{
	    const string SizeWindowScript = @"var meta = document.createElement('meta'); meta.setAttribute('name', 'viewport'); meta.setAttribute('content', 'width=device-width'); document.getElementsByTagName('head')[0].appendChild(meta);";
        const string JavaScriptFunction = "function invokeCSharpAction(data){window.webkit.messageHandlers.invokeAction.postMessage(data);}";
		WKUserContentController userController;

		protected override void OnElementChanged (ElementChangedEventArgs<HybridWebView> e)
		{
			base.OnElementChanged (e);

			if (Control == null) {
				userController = new WKUserContentController ();
				userController.AddUserScript (new WKUserScript(new NSString(SizeWindowScript), WKUserScriptInjectionTime.AtDocumentEnd, false));
				userController.AddUserScript (new WKUserScript(new NSString(JavaScriptFunction), WKUserScriptInjectionTime.AtDocumentEnd, false));
				userController.AddScriptMessageHandler (this, "invokeAction");

				var config = new WKWebViewConfiguration { UserContentController = userController };
				var webView = new WKWebView (Frame, config);
				SetNativeControl (webView);
			}
			if (e.OldElement != null) {
				userController.RemoveAllUserScripts ();
				userController.RemoveScriptMessageHandler ("invokeAction");

				var hybridWebView = e.OldElement;
				hybridWebView.Cleanup();
			}
			if (e.NewElement != null) {
			    var hybridWebView = e.NewElement;
			    hybridWebView.RegisterSearchTextUpdate(UpdateSearchText);

                string fileName = Path.Combine (NSBundle.MainBundle.BundlePath, $"Content/{Element.Uri}");
				Control.LoadRequest (new NSUrlRequest (new NSUrl (fileName, false)));
            }
		}

	    private async Task UpdateSearchText(string searchText)
	    {
	        var js = (NSString)"javascript:searchHighlight('" + searchText + "');";
	        await this.Control.EvaluateJavaScriptAsync(js);
        }

        public void DidReceiveScriptMessage(WKUserContentController userContentController, WKScriptMessage message)
		{
			Element.CallbackAction (message.Body.ToString());
		}
	}
}
