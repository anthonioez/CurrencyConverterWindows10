using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter
{
    public class CurrencySource : IIncrementalSource<Currency>
    {
        public CurrencySource()
        {
        }

        public async Task<IEnumerable<Currency>> getPage(bool all, string term, int pageIndex)
        {
            return await Task.Run<IEnumerable<Currency>>(() =>
            {
                Debug.Write("\npage: " + pageIndex);

                IEnumerable<Currency> result = null;
                if (all)
                {
                    if (term.Length > 0)
                    {
                        result = from c in App.currencies
                                 where (c.Name.ToLower().Contains(term) ||
                                    c.Code.ToLower().Contains(term) ||
                                    c.Symbol.ToLower().Contains(term))
                                 select c;
                    }
                    else
                    {
                        result = from c in App.currencies
                                 select c;
                    }
                }
                else
                {
                    result = from c in App.favorites
                              select c;
                }

                if (result != null)
                    return result = result.Skip(pageIndex * 20).Take(20);
                else
                    return result;
            });
        }
    }
}
