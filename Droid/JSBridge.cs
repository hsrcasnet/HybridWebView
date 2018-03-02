using System;
using Android.Webkit;
using CustomRenderer.Droid;
using Java.Interop;

namespace CustomRenderer.Droid
{
	public class JSBridge : Java.Lang.Object
	{
	    public const string Name = "jsBridge";

        readonly WeakReference<HybridWebViewRenderer> hybridWebViewRenderer;

		public JSBridge (HybridWebViewRenderer hybridRenderer)
		{
			hybridWebViewRenderer = new WeakReference <HybridWebViewRenderer> (hybridRenderer);
		}

		[JavascriptInterface]
		[Export (nameof(InvokeAction))]
		public void InvokeAction (string data)
		{
		    if (hybridWebViewRenderer != null && hybridWebViewRenderer.TryGetTarget (out var hybridRenderer)) {
				hybridRenderer.Element.CallbackAction (data);
			}
		}
	}
}

