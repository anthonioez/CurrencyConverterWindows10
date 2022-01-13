using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Data.Json;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace CurrencyConverter
{
    public class DataModel
    {
        protected static string TAG = "DataModel";

        public event RoutedEventHandler Started;
        public event RoutedEventHandler Loaded;
        public event RoutedEventHandler Errored;

        private HttpWebRequest request = null;

        public string Error { get; set; }
        public bool Loading { get; set; }
        
        public DataModel()
        {
            Error = null;
        }

        public bool isLoaded()
        {
            return (Error == null);
        }

        public async void start()
        {
            started();

            await Task.Delay(500).ContinueWith((a) => { load(); });
        }

        private void load()
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                errored("No internet!");
                return;
            }

            Error = null;

            string url = string.Format("https://openexchangerates.org/api/latest.json?app_id={0}", App.API_KEY);

            try
            {
                request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
                request.Method = "GET";

                request.BeginGetResponse(new AsyncCallback(dataCallback), request);
            }
            catch (Exception)
            {
                errored("I/O error!");
            }
        }

        private void dataCallback(IAsyncResult result)
        {
            HttpWebResponse response = null;

            try
            {
                response = (HttpWebResponse)request.EndGetResponse(result);
                Stream stream = response.GetResponseStream();
                if (stream != null)
                {
                    

                    StreamReader reader = new StreamReader(stream);
                    string str = reader.ReadToEnd();

                    JsonObject json; // = new JsonObject();
                    if (JsonObject.TryParse(str, out json))
                    {
                        if (json != null && json.ContainsKey("base"))
                        {
                            string root = json.GetNamedString("base");
                            Prefs.setBase(root);

                            if (json.ContainsKey("rates"))
                            {
                                JsonObject rates = json["rates"].GetObject();    //.GetObject("rates");
                                if (rates != null)
                                {
                                    Debug.Write("rates: " + rates.Count);

                                    ICollection<string> keys = rates.Keys;

                                    foreach (string key in keys)
                                    {
                                        double rate = rates.GetNamedNumber(key);

                                        //Debug.Write("\ncode: " + key + ": " + rate);

                                        if (key == root) continue;

                                        Prefs.setRate(key.ToUpper(), rate);
                                    }

                                    //Debug.Write("\ndone");

                                    Prefs.setStamp(CurrencyConverter.getStamp());

                                    finished();
                                    return;
                                }

                            }
                        }
                    }

                    Error = "Invalid data!";
                }
                else
                {
                    Error = "No data!";
                }

                errored(Error);
            }
            catch (Exception)
            {                
                errored("Data error!");
            }
            finally
            {
                if (request != null)
                    request.Abort();

                request = null;
            }
        }

        public void stop()
        {
            Loading = false;

            if (request != null)
            {
                request.Abort();
            }
        }

        protected async void started()
        {
            Loading = true;

            var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Started(this, null);
            });
        }

        protected async void finished()
        {
            Loading = false;

            var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Loaded(this, null);
            });
        }

        protected async void errored(string msg)
        {
            Error = msg;
            Loading = false;

            var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Errored(this, null);
            });
        }
    }
}
