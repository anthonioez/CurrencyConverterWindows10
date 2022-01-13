using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CurrencyConverter
{
    public class CurrencyModel : INotifyPropertyChanged
    {
        private CurrencyCollection<CurrencySource, Currency> items = null;

        private bool all = false;
        private bool loading = false;

        public event PropertyChangedEventHandler PropertyChanged;

        public CurrencyModel(bool all)
        {
            this.items = new CurrencyCollection<CurrencySource, Currency>(this, all);

            this.all = all;
        }

        public bool Loading
        {
            get
            {
                return loading;
            }
            set
            {
                loading = value;
                NotifyPropertyChanged("Empty");
            }
        }

        public CurrencyCollection<CurrencySource, Currency> Items
        {
            get
            {
                return items;
            }
            set
            {
                items = value;
                NotifyPropertyChanged("Items");
                NotifyPropertyChanged("Empty");
            }
        }

        public string Empty
        {
            get
            {
                return items.Count == 0 ? (loading ? "" : "No currency found!") : "";
            }
        }

        public void reset(string term)
        {
            items.reset(term);
        }

        public void reload()
        {
            items.reset("");
        }

        public void deleted()
        {
            NotifyPropertyChanged("Empty");
        }

        private void NotifyPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}