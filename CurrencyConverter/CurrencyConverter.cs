using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter
{
    public class CurrencyConverter
    {
        public static void init()
        {
            App.dataModel = new DataModel();

            loadCurrencies();
            loadFavorites();
            loadSlots();

            Prefs.setRate("USD", 1);
        }

        public static void loadSlots()
        {
            Currency currency;

            currency = find(Prefs.getFrom());
            if (currency != null)
            {
                App.slotFr = currency;
            }

            currency = find(Prefs.getTo());
            if (currency != null)
            {
                App.slotTo = currency;
            }
        }

        public static void saveSlots()
        {
            Prefs.setFrom(App.slotFr.code);
            Prefs.setTo(App.slotTo.code);
        }

        public static void loadFavorites()
        {
            App.favorites.Clear();

            string favs = Prefs.getFavs();

            string[] list = favs.Split(',');
            foreach (string name in list)
            {
                Currency currency = CurrencyConverter.find(name);
                if (currency == null) continue;

                App.favorites.Add(currency);
            }
        }

        public static void saveFavorites()
        {
            string fav = "";

            foreach (Currency currency in App.favorites)
            {
                fav = fav + (fav.Length == 0 ? "" : ",") + currency.code;
            }

            Prefs.setFavs(fav);
        }

        public static void loadCurrencies()
        {
            App.currencies.Clear();

            App.currencies.Add(new Currency("د.إ",  "AED", "United Arab Emirates Dirham"));
            App.currencies.Add(new Currency("؋",    "AFN", "Afghan Afghani"));
            App.currencies.Add(new Currency("Lek",  "ALL", "Albanian Lek"));
            App.currencies.Add(new Currency("դր",   "AMD", "Armenian Dram"));
            App.currencies.Add(new Currency("NAƒ",  "ANG", "Netherlands Antillean Guilder"));
            App.currencies.Add(new Currency("Kz",   "AOA", "Angolan Kwanza"));
            App.currencies.Add(new Currency("$",    "ARS", "Argentine Peso"));
            App.currencies.Add(new Currency("$",    "AUD", "Australian Dollar"));
            App.currencies.Add(new Currency("ƒ",    "AWG", "Aruban Florin"));
            App.currencies.Add(new Currency("ман",  "AZN", "Azerbaijani Manat"));
            App.currencies.Add(new Currency("KM",   "BAM", "Bosnia-Herzegovina Convertible Mark"));
            App.currencies.Add(new Currency("$",    "BBD", "Barbadian Dollar"));
            App.currencies.Add(new Currency("Tk",   "BDT", "Bangladeshi Taka"));
            App.currencies.Add(new Currency("лв",   "BGN", "Bulgarian Lev"));
            App.currencies.Add(new Currency(".د.ب", "BHD", "Bahraini Dinar"));
            App.currencies.Add(new Currency("FBu",  "BIF", "Burundian Franc"));
            App.currencies.Add(new Currency("$",    "BMD", "Bermudan Dollar"));
            App.currencies.Add(new Currency("$",    "BND", "Brunei Dollar"));
            App.currencies.Add(new Currency("$b",   "BOB", "Bolivian Boliviano"));
            App.currencies.Add(new Currency("R$",   "BRL", "Brazilian Real"));
            App.currencies.Add(new Currency("$",    "BSD", "Bahamian Dollar"));
            //App.currencies.Add(new Currency("", "BTC", "Bitcoin"));
            App.currencies.Add(new Currency("Nu.",  "BTN", "Bhutanese Ngultrum"));
            App.currencies.Add(new Currency("P",    "BWP", "Botswanan Pula"));
            App.currencies.Add(new Currency("p.",   "BYN", "Belarusian Ruble"));
            App.currencies.Add(new Currency("p.",   "BYR", "Belarusian Ruble (pre-2016)"));
            App.currencies.Add(new Currency("BZ$",  "BZD", "Belize Dollar"));
            App.currencies.Add(new Currency("$",    "CAD", "Canadian Dollar"));
            App.currencies.Add(new Currency("CDFr", "CDF", "Congolese Franc"));
            App.currencies.Add(new Currency("CHF",  "CHF", "Swiss Franc"));
            //App.currencies.Add(new Currency("", "CLF", "Chilean Unit of Account (UF)"));
            App.currencies.Add(new Currency("$",    "CLP", "Chilean Peso"));
            App.currencies.Add(new Currency("¥",    "CNY", "Chinese Yuan"));
            App.currencies.Add(new Currency("$",    "COP", "Colombian Peso"));
            App.currencies.Add(new Currency("₡",    "CRC", "Costa Rican Colón"));
            App.currencies.Add(new Currency("₱",    "CUC", "Cuban Convertible Peso"));
            App.currencies.Add(new Currency("₱",    "CUP", "Cuban Peso"));
            App.currencies.Add(new Currency("Esc",  "CVE", "Cape Verdean Escudo"));
            App.currencies.Add(new Currency("Kč",   "CZK", "Czech Republic Koruna"));
            App.currencies.Add(new Currency("Fdj",  "DJF", "Djiboutian Franc"));
            App.currencies.Add(new Currency("kr",   "DKK", "Danish Krone"));
            App.currencies.Add(new Currency("RD$",  "DOP", "Dominican Peso"));
            App.currencies.Add(new Currency("دج",   "DZD", "Algerian Dinar"));
            App.currencies.Add(new Currency("EEK",  "EEK", "Estonian Kroon"));
            App.currencies.Add(new Currency("£",    "EGP", "Egyptian Pound"));
            App.currencies.Add(new Currency("Nfk",  "ERN", "Eritrean Nakfa"));
            App.currencies.Add(new Currency("Br",   "ETB", "Ethiopian Birr"));
            App.currencies.Add(new Currency("€",    "EUR", "Euro"));
            App.currencies.Add(new Currency("$",    "FJD", "Fijian Dollar"));
            App.currencies.Add(new Currency("£",    "FKP", "Falkland Islands Pound"));
            App.currencies.Add(new Currency("£",    "GBP", "British Pound Sterling"));
            App.currencies.Add(new Currency("₾",    "GEL", "Georgian Lari"));
            App.currencies.Add(new Currency("£",    "GGP", "Guernsey Pound"));
            App.currencies.Add(new Currency("¢",    "GHS", "Ghanaian Cedi"));
            App.currencies.Add(new Currency("£",    "GIP", "Gibraltar Pound"));
            App.currencies.Add(new Currency("D",    "GMD", "Gambian Dalasi"));
            App.currencies.Add(new Currency("FG",   "GNF", "Guinean Franc"));
            App.currencies.Add(new Currency("Q",    "GTQ", "Guatemalan Quetzal"));
            App.currencies.Add(new Currency("$",    "GYD", "Guyanaese Dollar"));
            App.currencies.Add(new Currency("$",    "HKD", "Hong Kong Dollar"));
            App.currencies.Add(new Currency("L",    "HNL", "Honduran Lempira"));
            App.currencies.Add(new Currency("kn",   "HRK", "Croatian Kuna"));
            App.currencies.Add(new Currency("G",    "HTG", "Haitian Gourde"));
            App.currencies.Add(new Currency("Ft",   "HUF", "Hungarian Forint"));
            App.currencies.Add(new Currency("Rp",   "IDR", "Indonesian Rupiah"));
            App.currencies.Add(new Currency("₪",    "ILS", "Israeli New Sheqel"));
            App.currencies.Add(new Currency("£",    "IMP", "Manx pound"));
            App.currencies.Add(new Currency("₹",    "INR", "Indian Rupee"));
            App.currencies.Add(new Currency("ع.د",  "IQD", "Iraqi Dinar"));
            App.currencies.Add(new Currency("﷼",    "IRR", "Iranian Rial"));
            App.currencies.Add(new Currency("kr",   "ISK", "Icelandic Króna"));
            App.currencies.Add(new Currency("£",    "JEP", "Jersey Pound"));
            App.currencies.Add(new Currency("J$",   "JMD", "Jamaican Dollar"));
            App.currencies.Add(new Currency("JD",   "JOD", "Jordanian Dinar"));
            App.currencies.Add(new Currency("¥",    "JPY", "Japanese Yen"));
            App.currencies.Add(new Currency("KSh",  "KES", "Kenyan Shilling"));
            App.currencies.Add(new Currency("лв",   "KGS", "Kyrgystani Som"));
            App.currencies.Add(new Currency("៛",    "KHR", "Cambodian Riel"));
            App.currencies.Add(new Currency("KMF",  "KMF", "Comorian Franc"));
            App.currencies.Add(new Currency("₩",    "KPW", "North Korean Won"));
            App.currencies.Add(new Currency("₩",    "KRW", "South Korean Won"));
            App.currencies.Add(new Currency("د.ك",  "KWD", "Kuwaiti Dinar"));
            App.currencies.Add(new Currency("$",    "KYD", "Cayman Islands Dollar"));
            App.currencies.Add(new Currency("KZT",  "KZT", "Kazakhstani Tenge"));
            App.currencies.Add(new Currency("₭",    "LAK", "Laotian Kip"));
            App.currencies.Add(new Currency("£",    "LBP", "Lebanese Pound"));
            App.currencies.Add(new Currency("₨",    "LKR", "Sri Lankan Rupee"));
            App.currencies.Add(new Currency("$",    "LRD", "Liberian Dollar"));
            App.currencies.Add(new Currency("L",    "LSL", "Lesotho Loti"));
            App.currencies.Add(new Currency("Lt",   "LTL", "Lithuanian Litas"));
            App.currencies.Add(new Currency("Ls",   "LVL", "Latvian Lats"));
            App.currencies.Add(new Currency("ل.د",  "LYD", "Libyan Dinar"));
            App.currencies.Add(new Currency("د.م",  "MAD", "Moroccan Dirham"));
            App.currencies.Add(new Currency("MDL",  "MDL", "Moldovan Leu"));
            App.currencies.Add(new Currency("MGA",  "MGA", "Malagasy Ariary"));
            App.currencies.Add(new Currency("ден",  "MKD", "Macedonian Denar"));
            App.currencies.Add(new Currency("K",    "MMK", "Myanma Kyat"));
            App.currencies.Add(new Currency("₮",    "MNT", "Mongolian Tugrik"));
            App.currencies.Add(new Currency("$",    "MOP", "Macanese Pataca"));
            App.currencies.Add(new Currency("UM",   "MRO", "Mauritanian Ouguiya"));
            App.currencies.Add(new Currency("Lm",   "MTL", "Maltese Lira"));
            App.currencies.Add(new Currency("₨",    "MUR", "Mauritian Rupee"));
            App.currencies.Add(new Currency("Rf",   "MVR", "Maldivian Rufiyaa"));
            App.currencies.Add(new Currency("MK",   "MWK", "Malawian Kwacha"));
            App.currencies.Add(new Currency("$",    "MXN", "Mexican Peso"));
            App.currencies.Add(new Currency("RM",   "MYR", "Malaysian Ringgit"));
            App.currencies.Add(new Currency("MT",   "MZN", "Mozambican Metical"));
            App.currencies.Add(new Currency("$",    "NAD", "Namibian Dollar"));
            App.currencies.Add(new Currency("₦",    "NGN", "Nigerian Naira"));
            App.currencies.Add(new Currency("C$",   "NIO", "Nicaraguan Córdoba"));
            App.currencies.Add(new Currency("kr",   "NOK", "Norwegian Krone"));
            App.currencies.Add(new Currency("₨",    "NPR", "Nepalese Rupee"));
            App.currencies.Add(new Currency("$",    "NZD", "New Zealand Dollar"));
            App.currencies.Add(new Currency("ر.ع.", "OMR", "Omani Rial"));
            App.currencies.Add(new Currency("B/.",  "PAB", "Panamanian Balboa"));
            App.currencies.Add(new Currency("S/.",  "PEN", "Peruvian Nuevo Sol"));
            App.currencies.Add(new Currency("K",    "PGK", "Papua New Guinean Kina"));
            App.currencies.Add(new Currency("₱",    "PHP", "Philippine Peso"));
            App.currencies.Add(new Currency("₨",    "PKR", "Pakistani Rupee"));
            App.currencies.Add(new Currency("zł",   "PLN", "Polish Zloty"));
            App.currencies.Add(new Currency("Gs",   "PYG", "Paraguayan Guarani"));
            App.currencies.Add(new Currency("﷼",    "QAR", "Qatari Rial"));
            App.currencies.Add(new Currency("lei",  "RON", "Romanian Leu"));
            App.currencies.Add(new Currency("Дин.", "RSD", "Serbian Dinar"));
            App.currencies.Add(new Currency("₽",    "RUB", "Russian Ruble"));
            App.currencies.Add(new Currency("RF",   "RWF", "Rwandan Franc"));
            App.currencies.Add(new Currency("﷼",    "SAR", "Saudi Riyal"));
            App.currencies.Add(new Currency("$",    "SBD", "Solomon Islands Dollar"));
            App.currencies.Add(new Currency("SR",   "SCR", "Seychellois Rupee"));
            App.currencies.Add(new Currency("£",    "SDG", "Sudanese Pound"));
            App.currencies.Add(new Currency("kr",   "SEK", "Swedish Krona"));
            App.currencies.Add(new Currency("$",    "SGD", "Singapore Dollar"));
            App.currencies.Add(new Currency("£",    "SHP", "Saint Helena Pound"));
            App.currencies.Add(new Currency("Le",   "SLL", "Sierra Leonean Leone"));
            App.currencies.Add(new Currency("S",    "SOS", "Somali Shilling"));
            App.currencies.Add(new Currency("$",    "SRD", "Surinamese Dollar"));
            App.currencies.Add(new Currency("Db",   "STD", "São Tomé and Príncipe Dobra"));
            App.currencies.Add(new Currency("₡",    "SVC", "Salvadoran Colón"));
            App.currencies.Add(new Currency("£",    "SYP", "Syrian Pound"));
            App.currencies.Add(new Currency("SZL",  "SZL", "Swazi Lilangeni"));
            App.currencies.Add(new Currency("฿",    "THB", "Thai Baht"));
            App.currencies.Add(new Currency("TJS",  "TJS", "Tajikistani Somoni"));
            App.currencies.Add(new Currency("T",    "TMT", "Turkmenistani Manat"));
            App.currencies.Add(new Currency("د.ت",  "TND", "Tunisian Dinar"));
            App.currencies.Add(new Currency("T$",   "TOP", "Tongan Paʻanga"));
            App.currencies.Add(new Currency("₺",    "TRY", "Turkish Lira"));
            App.currencies.Add(new Currency("TT$",  "TTD", "Trinidad and Tobago Dollar"));
            App.currencies.Add(new Currency("NT$",  "TWD", "New Taiwan Dollar"));
            App.currencies.Add(new Currency("x",    "TZS", "Tanzanian Shilling"));
            App.currencies.Add(new Currency("₴",    "UAH", "Ukrainian Hryvnia"));
            App.currencies.Add(new Currency("USh",  "UGX", "Ugandan Shilling"));
            App.currencies.Add(new Currency("$",    "USD", "United States Dollar"));
            App.currencies.Add(new Currency("$U",   "UYU", "Uruguayan Peso"));
            App.currencies.Add(new Currency("лв",   "UZS", "Uzbekistan Som"));
            App.currencies.Add(new Currency("Bs",   "VEF", "Venezuelan Bolívar Fuerte"));
            App.currencies.Add(new Currency("₫",    "VND", "Vietnamese Dong"));
            App.currencies.Add(new Currency("Vt",   "VUV", "Vanuatu Vatu"));
            App.currencies.Add(new Currency("WS$",  "WST", "Samoan Tala"));
            App.currencies.Add(new Currency("franc","XAF", "CFA Franc BEAC"));
            App.currencies.Add(new Currency("$",    "XCD", "East Caribbean Dollar"));
            App.currencies.Add(new Currency("franc","XOF", "CFA Franc BCEAO"));
            App.currencies.Add(new Currency("CFP",  "XPF", "CFP Franc"));
            App.currencies.Add(new Currency("﷼",    "YER", "Yemeni Rial"));
            App.currencies.Add(new Currency("R",    "ZAR", "South African Rand"));
            App.currencies.Add(new Currency("ZMK",  "ZMK", "Zambian Kwacha (pre-2013)"));
            App.currencies.Add(new Currency("ZMK",  "ZMW", "Zambian Kwacha"));
            App.currencies.Add(new Currency("Z$",   "ZWL", "Zimbabwean Dollar"));
        }

        // Returns the exchange rate to `target` currency from `base` currency
        public static double rate(string from, string to)
        {
            string root = Prefs.getBase();
            double toRate = Prefs.getRate(to, 0);
            double fromRate = Prefs.getRate(from, 0);

            // Throw an error if either rate isn't in the rates array
            //if ( !rates[to] || !rates[from] ) throw "fx error";

            // If `from` currency === fx.base, return the basic exchange rate for the `to` currency
            /*if (from == "USD")
            {
                return 1;
            }
            */

            // If `to` currency === fx.base, return the basic inverse rate of the `from` currency
            if (to == root)
            {
                if (fromRate == 0)
                    return 0;
                else
                    return 1 / fromRate;
            }

            // Otherwise, return the `to` rate multipled by the inverse of the `from` rate to get the
            // relative exchange rate between the two currencies
            if (fromRate == 0)
                return 0;
            else
                return toRate * (1 / fromRate);
        }

        public static Currency find(string code)
        {
            foreach (Currency item in App.currencies)
            {
                if (item.code == code)
                {
                    return item;
                }
            }

            return null;    // new Currency("", "", "");
        }

        public static long getStamp()
        {
            return (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }

    }
}
