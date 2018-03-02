using System;
using Xamarin.Forms;

namespace CustomRenderer
{
	public class HybridWebView : View
	{
		Action<string> searchTextUpdateAction;
		Action<string> callbackAction;

		public static readonly BindableProperty UriProperty = BindableProperty.Create (
			propertyName: "Uri",
			returnType: typeof(string),
			declaringType: typeof(HybridWebView),
			defaultValue: default(string));
		
		public string Uri {
			get { return (string)GetValue (UriProperty); }
			set { SetValue (UriProperty, value); }
		}

	    public static readonly BindableProperty SearchTextProperty = BindableProperty.Create(
	        propertyName: "SearchText",
	        returnType: typeof(string),
	        declaringType: typeof(HybridWebView),
	        defaultValue: default(string));

	    public string SearchText
        {
	        get { return (string)GetValue(SearchTextProperty); }
	        set { SetValue(SearchTextProperty, value); }
	    }

	    public void RegisterSearchTextUpdate(Action<string> callback)
	    {
	        searchTextUpdateAction = callback;
	    }

        public void CallbackAction (string data)
		{
			if (callbackAction == null || data == null) {
				return;
			}
		    callbackAction.Invoke(data);
		}

	    public void RegisterCallbackAction(Action<string> callback)
	    {
	        callbackAction = callback;
	    }

        public void UpdateSearchText(string searchText)
	    {
	        if (searchTextUpdateAction == null || searchText == null)
	        {
	            return;
	        }
	        searchTextUpdateAction.Invoke(searchText);
        }


	    public void Cleanup()
	    {
	        searchTextUpdateAction = null;
	        callbackAction = null;
	    }
    }
}
