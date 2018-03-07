using System;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CustomRenderer
{
    public class HybridWebView : View
    {
        public static readonly BindableProperty UriProperty = BindableProperty.Create(
            "Uri",
            typeof(string),
            typeof(HybridWebView),
            default(string));

        public static readonly BindableProperty SearchTextProperty = BindableProperty.Create(
            "SearchText",
            typeof(string),
            typeof(HybridWebView),
            default(string));

        private Action<string> callbackAction;
        private Func<string, Task> searchTextUpdateTask;

        public string Uri
        {
            get => (string) GetValue(UriProperty);
            set => SetValue(UriProperty, value);
        }

        public string SearchText
        {
            get => (string) GetValue(SearchTextProperty);
            set => SetValue(SearchTextProperty, value);
        }

        public void RegisterSearchTextUpdate(Func<string, Task> callback)
        {
            searchTextUpdateTask = callback;
        }

        public void CallbackAction(string data)
        {
            if (callbackAction == null || data == null)
            {
                return;
            }
            callbackAction.Invoke(data);
        }

        public void RegisterCallbackAction(Action<string> callback)
        {
            callbackAction = callback;
        }

        public async Task UpdateSearchText(string searchText)
        {
            if (searchTextUpdateTask == null || searchText == null)
            {
                return;
            }

            await searchTextUpdateTask(searchText);
        }

        public void Cleanup()
        {
            searchTextUpdateTask = null;
            callbackAction = null;
        }
    }
}