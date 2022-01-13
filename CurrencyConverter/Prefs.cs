using System;
using Windows.Storage;

namespace CurrencyConverter
{
    public class Prefs
    {
        private static ApplicationDataContainer prefs = null;

        public static bool getAds()
        {
            return getValue<bool>("data", false);
        }
        public static bool setAds(bool value)
        {
            return setValue("data", value);
        }

        public static long getStamp()
        {
            return getValue<long>("stamp", 0);
        }
        public static bool setStamp(long stamp)
        {
            return setValue("stamp", stamp);
        }

        public static string getBase()
        {
            return getValue<string>("base", "USD");
        }
        public static bool setBase(string value)
        {
            return setValue("base", value);
        }

        public static string getFrom()
        {
            return getValue<string>("fr", "USD");
        }
        public static bool setFrom(string value)
        {
            return setValue("fr", value);
        }

        public static string getTo()
        {
            return getValue<string>("to", "EUR");
        }
        public static bool setTo(string value)
        {
            return setValue("to", value);
        }

        public static string getFavs()
        {
            return getValue<string>("favs", "");
        }
        public static bool setFavs(string value)
        {
            return setValue("favs", value);
        }

        public static double getRate(string key, double defValue)
        {
            return getValue<double>(key, defValue);
        }
        public static bool setRate(string key, double value)
        {
            return setValue(key, value);
        }

        public static double getInput()
        {
            return getValue<double>("input", 0);
        }
        public static bool setInput(double value)
        {
            return setValue("input", value);
        }

        public static bool setValue(string key, object value)
        {
            bool valueIsChanged = false;

            try
            {
                if(prefs == null)
                    prefs = ApplicationData.Current.LocalSettings;  

                if (value != prefs.Values[key])
                {
                    prefs.Values[key] = value;
                    valueIsChanged = true;
                }
            }
            catch(Exception)
            {

            }

            return valueIsChanged;
        }

        public static T getValue<T>(string key, T defaultValue)
        {
            T value = defaultValue;

            try
            {
                if (prefs == null)
                    prefs = ApplicationData.Current.LocalSettings;

                if (prefs.Values.ContainsKey(key))
                {
                    value = (T)prefs.Values[key];
                }
                else
                {
                    prefs.Values[key] = value;
                }
            }
            catch (Exception)
            {

            }

            return value;
        }
    }
}
