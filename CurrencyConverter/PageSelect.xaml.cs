using System;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace CurrencyConverter
{
    public partial class PageSelect : Page
    {
        private string slot = "fr";
        private bool isLoaded = false;
        private string lastSearch = "";
        private CurrencyModel allModel = null;

        public PageSelect()
        {
            this.DataContext = null;
            this.InitializeComponent();

            this.Loaded += onLoaded;
            this.Unloaded += onUnloaded;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string value = e.Parameter as string;
            if (value != null && value != "")
            {
                slot = value;
            }
        }

        #region DataModel
        private void onDataStarted(object sender, RoutedEventArgs e)
        {
            this.buttonRefresh.Visibility = Visibility.Collapsed;
            this.progressRing.IsActive = true;

        }

        private void onDataLoaded(object sender, RoutedEventArgs e)
        {
            this.buttonRefresh.Visibility = Visibility.Visible;
            this.progressRing.IsActive = false;

            updateStamp();

            allModel.reload();
        }

        private void onDataErrored(object sender, RoutedEventArgs e)
        {
            this.buttonRefresh.Visibility = Visibility.Visible;
            this.progressRing.IsActive = false;

            Utils.message("Refresh", App.dataModel.Error, null);
        }
        #endregion

        private void onUnloaded(object sender, RoutedEventArgs e)
        {
            App.dataModel.Started -= onDataStarted;
            App.dataModel.Loaded -= onDataLoaded;
            App.dataModel.Errored -= onDataErrored;

            SystemNavigationManager.GetForCurrentView().BackRequested -= onBackRequested;
        }

        private void onLoaded(object sender, RoutedEventArgs e)
        {
            App.dataModel.Started += onDataStarted;
            App.dataModel.Loaded += onDataLoaded;
            App.dataModel.Errored += onDataErrored;

            SystemNavigationManager.GetForCurrentView().BackRequested += onBackRequested;

            adControl.Visibility = App.purchased ? Visibility.Collapsed : Visibility.Visible;

            updateStamp();

            if (isLoaded) return;

            if (allModel == null)
            {
                allModel = new CurrencyModel(true);
                this.DataContext = allModel;
            }

            this.listSelect.SelectedIndex = -1;
            isLoaded = true;
        }

        private void onItemSelected(object sender, SelectionChangedEventArgs e)
        {
            if (!isLoaded) return;

            if (listSelect.SelectedIndex == -1) return;
            Currency item = (Currency)listSelect.SelectedItem;
            listSelect.SelectedIndex = -1;

            if (item != null)
            {
                if(slot == "fr")
                {
                    if (App.slotFr.code == item.code || App.slotTo.code == item.code)
                    {
                        Utils.message("Currency", "Currency already set!", null);
                        return;
                    }

                    App.slotFr = item;
                }
                else
                {
                    if (App.slotTo.code == item.code || App.slotFr.code == item.code)
                    {
                        Utils.message("Currency", "Currency already set!", null);
                        return;
                    }

                    App.slotTo = item;
                }

                CurrencyConverter.saveSlots();
                back();
            }
        }

        private void onRefreshClick(object sender, RoutedEventArgs e)
        {
            if (App.dataModel.Loading)
                App.dataModel.stop();

            App.dataModel.start();
        }

        private void onRightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            if (senderElement == null) return;

            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);
            flyoutBase.ShowAt(senderElement);
        }

        private void onHolding(object sender, HoldingRoutedEventArgs e)
        {
            if (e.HoldingState != HoldingState.Started) return;

            FrameworkElement senderElement = sender as FrameworkElement;
            if (senderElement == null) return;

            FlyoutBase.ShowAttachedFlyout(senderElement);
        }

        private void onFlyoutAdd(object sender, RoutedEventArgs e)
        {
            MenuFlyoutItem item = sender as MenuFlyoutItem;
            if (item != null)
            {
                Currency entry = item.DataContext as Currency;
                if (entry != null)
                {
                    addFav(entry);
                }
            }
        }

        private void onBackRequested(object sender, BackRequestedEventArgs e)
        {
            if (back())
            {
                e.Handled = true;
            }
        }

        private void onBackClick(object sender, RoutedEventArgs e)
        {
            back();
        }

        private void onSearchClick(object sender, RoutedEventArgs e)
        {
            search(textSearch.Text.Trim());
        }

        private bool back()
        { 
            if(this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
                return true;
            }
            return false;
        }

        private void updateStamp()
        {
            long stamp = Prefs.getStamp();
            if (stamp != 0)
            {
                DateTime datetime = (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddMilliseconds(stamp);
                textStamp.Text = string.Format("Last updated: {0} at {1}", datetime.ToString("dd/MM/yyyy"), datetime.ToString("HH:mm"));
            }
            else
            {
                textStamp.Text = "";
            }
        }

        private void addFav(Currency entry)
        {
            string fav = Prefs.getFavs();

            if (fav.Contains(entry.code))
            {
                Utils.message("Favorites", "Currency already added", null);
            }
            else
            {
                fav = fav + (fav.Length == 0 ? "" : ",") + entry.code;
                Prefs.setFavs(fav);

                Utils.message("Favorites", "Currency added", null);

                CurrencyConverter.loadFavorites();
            }
        }

        private void onSearchKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                search(textSearch.Text.Trim());
            }
        }

        private async void onSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            string search = textSearch.Text.Trim();
            if (lastSearch == search) return;

            allModel.reset(search);

            await listSelect.LoadMoreItemsAsync();

            lastSearch = search;
        }

        private async void search(string term)
        {
            allModel.reset(term);

            await listSelect.LoadMoreItemsAsync();

            lastSearch = term;
        }

        private void onFavoriteClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Currency currency = button.DataContext as Currency;
            if(currency != null)
            {
                addFav(currency);
            }
        }
    }
}
