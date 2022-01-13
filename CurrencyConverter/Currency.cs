using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyConverter
{
    public class Currency
    {
        public string symbol;
        public string code;
        public string name;

        public Currency()
        {
            this.symbol = "";
            this.code = "";
            this.name = "";
        }

        public Currency(string symbol, string code, string name)
        {
            this.symbol = symbol;
            this.code = code;
            this.name = name;
        }

        public string Code
        {
            get
            {
                return code;
            }
        }

        public string Symbol
        {
            get
            {
                return symbol;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public string Amount
        {
            get
            {
                if (App.value == 0)
                    return "";
                else
                    return symbol + Value.ToString("N2");
            }
        }

        public string Flag
        {
            get
            {
                return "ms-appx:///Flags/" + code.ToLower() + ".png";
            }
        }

        public double Value
        {
            get
            {
                return (CurrencyConverter.rate(App.slotFr.code, code) * App.value);
            }
        }

    }
}
