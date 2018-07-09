/*
 * Copyright (c) 2018 PayGate (Pty) Ltd
 *
 * Author: App Inlet (Pty) Ltd
 * 
 * Released under the GNU General Public License
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

/// <summary>
/// Summary description for GlobalUtility
/// </summary>
/// 

public static class GlobalUtility
{
    public static string generateReference()
    {
        return "pgtest_" + DateTime.Now.ToString("yyyyMMddHHmmss");
    }

    public static string[] getUriParts()
    {
        string[] url = (HttpContext.Current.Request.Url.AbsoluteUri).Split('/');
        return url;
    }

    public static string getScheme()
    {
        Uri uri = new Uri(HttpContext.Current.Request.Url.AbsoluteUri);
        return uri.Scheme;
    }

    public static string getHost()
    {
        Uri uri = new Uri(HttpContext.Current.Request.Url.AbsoluteUri);
        return uri.Authority;
    }

    public static string generateCountrySelectOptions()
    {
        //string options = "";
        StringBuilder options = new StringBuilder();
        string country = "ZAF";

        Dictionary<string, string> mostUsedCountryArray = new Dictionary<string, string>()
        {
            {"DEU","Germany"},
            {"ZAF","South Africa"},
            {"USA","United States"}
        };

        Dictionary<string, string> countryArray = new Dictionary<string, string>()
        {
            {"AFG","Afghanistan"},
            {"ALB","Albania"},
            {"DZA","Algeria"},
            {"ASM","American Samoa"},
            {"AND","Andorra"},
            {"AGO","Angola"},
            {"AIA","Anguilla"},
            {"ATA","Antarctica"},
            {"ATG","Antigua and Barbuda"},
            {"ARG","Argentina"},
            {"ARM","Armenia"},
            {"ABW","Aruba"},
            {"AUS","Australia"},
            {"AUT","Austria"},
            {"AZE","Azerbaijan"},
            {"BHS","Bahamas"},
            {"BHR","Bahrain"},
            {"BGD","Bangladesh"},
            {"BRB","Barbados"},
            {"BLR","Belarus"},
            {"BEL","Belgium"},
            {"BLZ","Belize"},
            {"BEN","Benin"},
            {"BMU","Bermuda"},
            {"BTN","Bhutan"},
            {"BOL","Bolivia"},
            {"BIH","Bosnia and Herzegovina"},
            {"BWA","Botswana"},
            {"BVT","Bouvet Island"},
            {"BRA","Brazil"},
            {"IOT","British Indian Ocean Territory"},
            {"VGB","British Virgin Islands"},
            {"BRN","Brunei Darussalam"},
            {"BGR","Bulgaria"},
            {"BFA","Burkina Faso"},
            {"BDI","Burundi"},
            {"KHM","Cambodia"},
            {"CMR","Cameroon"},
            {"CAN","Canada"},
            {"CPV","Cape Verde"},
            {"CYM","Cayman Islands"},
            {"CAF","Central African Republic"},
            {"TCD","Chad"},
            {"CHL","Chile"},
            {"CHN","China"},
            {"CXR","Christmas Island"},
            {"CCK","Cocos (Keeling) Islands"},
            {"COL","Colombia"},
            {"COM","Comoros"},
            {"COG","Congo"},
            {"COD","Congo The Democratic Republic of The"},
            {"COK","Cook Islands"},
            {"CRI","Costa Rica"},
            {"CIV","Cote D'ivoire"},
            {"CHRV","Croatia"},
            {"CUB","Cuba"},
            {"CYP","Cyprus"},
            {"CZE","Czech Republic"},
            {"DNK","Denmark"},
            {"DJI","Djibouti"},
            {"DMA","Dominica"},
            {"DOM","Dominican Republic"},
            {"ECU","Ecuador"},
            {"EGY","Egypt"},
            {"SLV","El Salvador"},
            {"GNQ","Equatorial Guinea"},
            {"ERI","Eritrea"},
            {"EST","Estonia"},
            {"ETH","Ethiopia"},
            {"FLK","Falkland Islands (Malvinas)"},
            {"FRO","Faroe Islands"},
            {"FJI","Fiji"},
            {"FIN","Finland"},
            {"FRA","France"},
            {"GUF","French Guiana"},
            {"FXX","French Metropolitan"},
            {"PYF","French Polynesia"},
            {"ATF","French Southern Territories"},
            {"GAB","Gabon"},
            {"GMB","Gambia"},
            {"GEO","Georgia"},
            {"DEU","Germany"},
            {"GHA","Ghana"},
            {"GIB","Gibraltar"},
            {"GRC","Greece"},
            {"GRL","Greenland"},
            {"GRD","Grenada"},
            {"GLP","Guadeloupe"},
            {"GUM","Guam"},
            {"GTM","Guatemala"},
            {"GIN","Guinea"},
            {"GNB","Guinea-bissau"},
            {"GUY","Guyana"},
            {"HTI","Haiti"},
            {"HMD","Heard Island and Mcdonald Islands"},
            {"VAT","Holy See (Vatican City State)"},
            {"HND","Honduras"},
            {"HKG","Hong Kong"},
            {"HUN","Hungary"},
            {"ISL","Iceland"},
            {"IND","India"},
            {"IDN","Indonesia"},
            {"IRN","Iran Islamic Republic of"},
            {"IRQ","Iraq"},
            {"IRL","Ireland"},
            {"ISR","Israel"},
            {"ITA","Italy"},
            {"JAM","Jamaica"},
            {"JPN","Japan"},
            {"JOR","Jordan"},
            {"KAZ","Kazakhstan"},
            {"KEN","Kenya"},
            {"KIR","Kiribati"},
            {"PRK","Korea Democratic People's Republic of"},
            {"KOR","Korea Republic of"},
            {"KWT","Kuwait"},
            {"KGZ","Kyrgyzstan"},
            {"LAO","Lao People's Democratic Republic"},
            {"LVA","Latvia"},
            {"LBN","Lebanon"},
            {"LSO","Lesotho"},
            {"LBR","Liberia"},
            {"LBY","Libyan Arab Jamahiriya"},
            {"LIE","Liechtenstein"},
            {"LTU","Lithuania"},
            {"LUX","Luxembourg"},
            {"MAC","Macau China"},
            {"MKD","Macedonia The Former Yugoslav Republic of"},
            {"MDG","Madagascar"},
            {"MWI","Malawi"},
            {"MYS","Malaysia"},
            {"MDV","Maldives"},
            {"MLI","Mali"},
            {"MLT","Malta"},
            {"MHL","Marshall Islands"},
            {"MTQ","Martinique"},
            {"MRT","Mauritania"},
            {"MUS","Mauritius"},
            {"MYT","Mayotte"},
            {"MEX","Mexico"},
            {"FSM","Micronesia Federated States of"},
            {"MDA","Moldova Republic of"},
            {"MCO","Monaco"},
            {"MNG","Mongolia"},
            {"MSR","Montserrat"},
            {"MAR","Morocco"},
            {"MOZ","Mozambique"},
            {"MMR","Myanmar"},
            {"NAM","Namibia"},
            {"NRU","Nauru"},
            {"NPL","Nepal"},
            {"NLD","Netherlands"},
            {"ANT","Netherlands Antilles"},
            {"NCL","New Caledonia"},
            {"NZL","New Zealand"},
            {"NIC","Nicaragua"},
            {"NER","Niger"},
            {"NGA","Nigeria"},
            {"NIU","Niue"},
            {"NFK","Norfolk Island"},
            {"MNP","Northern Mariana Islands"},
            {"NOR","Norway"},
            {"OMN","Oman"},
            {"PAK","Pakistan"},
            {"PLW","Palau"},
            {"PAN","Panama"},
            {"PNG","Papua New Guinea"},
            {"PRY","Paraguay"},
            {"PER","Peru"},
            {"PHL","Philippines"},
            {"PCN","Pitcairn"},
            {"POL","Poland"},
            {"PRT","Portugal"},
            {"PRI","Puerto Rico"},
            {"QAT","Qatar"},
            {"REU","Reunion"},
            {"ROM","Romania"},
            {"RUS","Russian Federation"},
            {"RWA","Rwanda"},
            {"SHN","Saint Helena"},
            {"KNA","Saint Kitts and Nevis"},
            {"LCA","Saint Lucia"},
            {"SPM","Saint Pierre and Miquelon"},
            {"VCT","Saint Vincent and The Grenadines"},
            {"WSM","Samoa"},
            {"SMR","San Marino"},
            {"STP","Sao Tome and Principe"},
            {"SAU","Saudi Arabia"},
            {"SEN","Senegal"},
            {"SYC","Seychelles"},
            {"SLE","Sierra Leone"},
            {"SGP","Singapore"},
            {"SVK","Slovakia"},
            {"SVN","Slovenia"},
            {"SLB","Solomon Islands"},
            {"SOM","Somalia"},
            {"ZAF","South Africa"},
            {"SGS","South Georgia and The South Sandwich Islands"},
            {"ESP","Spain"},
            {"LKA","Sri Lanka"},
            {"SDN","Sudan"},
            {"SUR","Suriname"},
            {"SJM","Svalbard and Jan Mayen"},
            {"SWZ","Swaziland"},
            {"SWE","Sweden"},
            {"CHE","Switzerland"},
            {"SYR","Syrian Arab Republic"},
            {"TWN","Taiwan Province of China"},
            {"TJK","Tajikistan"},
            {"TZA","Tanzania United Republic of"},
            {"THA","Thailand"},
            {"TGO","Togo"},
            {"TKL","Tokelau"},
            {"TON","Tonga"},
            {"TTO","Trinidad and Tobago"},
            {"TUN","Tunisia"},
            {"TUR","Turkey"},
            {"TKM","Turkmenistan"},
            {"TCA","Turks and Caicos Islands"},
            {"TUV","Tuvalu"},
            {"VIR","U.S. Virgin Islands"},
            {"UGA","Uganda"},
            {"UKR","Ukraine"},
            {"ARE","United Arab Emirates"},
            {"GBR","United Kingdom"},
            {"USA","United States"},
            {"UMI","United States Minor Outlying Islands"},
            {"URY","Uruguay"},
            {"UZB","Uzbekistan"},
            {"VUT","Vanuatu"},
            {"VEN","Venezuela"},
            {"VNM","Vietnam"},
            {"WLF","Wallis and Futuna"},
            {"ESH","Western Sahara"},
            {"YEM","Yemen"},
            {"YUG","Yugoslavia"},
            {"ZMB","Zambia"},
{"ZWE","Zimbabwe"}
};

        bool defaultSet = false;

        options.Append("<optgroup label=\"\"><option value=\"\">Select Country</option>");
        options.Append("<optgroup label=\"Most Used\">");
        foreach (KeyValuePair<string, string> entry in mostUsedCountryArray)
        {
            options.Append("<option value=\"" + entry.Key + "\"");
            if (country == entry.Key && !defaultSet)
            {
                options.Append(" selected=\"selected\"");
                defaultSet = true;
            }
            options.Append(">" + entry.Value + "</option>");
        }
        options.Append("</optgroup>");
        options.Append("<optgroup label=\"All Countries\">");
        foreach (KeyValuePair<string, string> entry in countryArray)
        {
            options.Append("<option value=\"");
            options.Append(entry.Key);
            options.Append("\"");
            if (country == entry.Key && !defaultSet)
            {
                options.Append(" selected=\"selected\"");
                defaultSet = true;
            }
            options.Append(">");
            options.Append(entry.Value);
            options.Append("</option>");
        }

        options.Append("</optgroup>");
        return options.ToString();
    }

    public static Dictionary<string, string> getCurrentUrl()
    {
        var url = new Dictionary<string, string>();
        url.Add("protocol", "http://");
        if (HttpContext.Current.Request.IsSecureConnection)
        {
            url["protocol"] = "https://";
        } else if (HttpContext.Current.Request.Url.Port == 443)
        {
            url["protocol"] = "https://";
        }

        return url;
    }

    public static Dictionary<string, string> getRoot(Dictionary<string, string> fullPath)
    {
        return null;
    }

    public static String[] getStringArray(string input)
    {
        var output = new String[1];
        output[0] = input;
        return output;
    }
}
