using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace CurrencyConverter
{
    public interface IIncrementalSource<T>
    {
        Task<IEnumerable<T>> getPage(bool all, string search, int pageIndex);
    }

    public class CurrencyCollection<T, I> : ObservableCollection<I>,
         ISupportIncrementalLoading
         where T : IIncrementalSource<I>, new()
    {
        private T source;
        private bool hasMoreItems;
        private int currentPage;
        private CurrencyModel model;
        private bool all;
        private string term;

        public CurrencyCollection(CurrencyModel model, bool all)
        {
            this.model = model;
            this.all = all;
            this.source = new T();
            this.hasMoreItems = true;
            this.term = "";
        }

        public bool HasMoreItems
        {
            get { return hasMoreItems; }
        }

        public void reset(string term)
        {
            this.term = term;
            this.Clear();
            hasMoreItems = true;
            currentPage = 0;
        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint counts)
        {
            var dispatcher = Window.Current.Dispatcher;

            return Task.Run<LoadMoreItemsResult>(async () =>
            {
                await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    model.Loading = true;
                });

                uint resultCount = 0;
                var result = await source.getPage(all, term, currentPage++);

                if (result == null || result.Count() == 0)
                {
                    hasMoreItems = false;

                    await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        model.Loading = false;
                    });
                }
                else
                {
                    resultCount = (uint)result.Count();

                    await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        foreach (I item in result)
                            this.Add(item);
                    });
                }

                return new LoadMoreItemsResult() { Count = resultCount };

            }).AsAsyncOperation<LoadMoreItemsResult>();
        }
    }
}
