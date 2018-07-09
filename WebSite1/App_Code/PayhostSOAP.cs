/*
 * Copyright (c) 2018 PayGate (Pty) Ltd
 *
 * Author: App Inlet (Pty) Ltd
 * 
 * Released under the GNU General Public License
 */

using System;
using System.Text;
using System.Globalization;
using System.Web;

using System.Runtime.Serialization.Formatters.Soap;
using System.Xml.Serialization;
using System.IO;

/// <summary>
/// Summary description for PayhostSOAP
/// </summary>
public static class PayhostSOAP
{
    //Private variables
    //Standard Inputs


    //Set some default fields
    public static string DEFAULT_PGID = "10011072130";
    public static int DEFAULT_AMOUNT = 3299;
    public static string DEFAULT_CURRENCY = "ZAR";
    public static string DEFAULT_LOCALE = "en-us";
    public static string DEFAULT_ENCRYPTION_KEY = "test";
    public static string DEFAULT_TITLE = "Mr";
    public static string DEFAULT_FIRST_NAME = "PayGate";
    public static string DEFAULT_LAST_NAME = "Test";
    public static string DEFAULT_EMAIL = "itsupport@paygate.co.za";
    public static string DEFAULT_COUNTRY = "ZAF";
    public static string DEFAULT_NOTIFY_URL = "http://www.gatewaymanagementservices.com/ws/gotNotify.php";

    public static PayHost.LookUpVaultRequestType makeLookupVaultRequest(HttpRequestBase r)
    {
        PayHost.LookUpVaultRequestType request = new PayHost.LookUpVaultRequestType();
        request.Account = new PayHost.PayGateAccountType();
        if (r["payGateId"] != null && r["payGateId"] != "") request.Account.PayGateId = r["payGateId"];
        if (r["encryptionKey"] != null && r["encryptionKey"] != "") request.Account.Password = r["encryptionKey"];

        if (r["vaultId"] != null && r["vaultId"] != "") request.VaultId = r["vaultId"];

        //User defined fields
        int j = 0;
        while (r["userKey" + (j + 1)] != null && r["userField" + (j + 1)] != null)
        {
            j++;
        }
        if (j > 0)
        {
            request.UserDefinedFields = new PayHost.KeyValueType[j];
            for (int k = 0; k < j; k++)
            {
                if (r["userKey" + (k + 1)] != null && r["userField" + (k + 1)] != null)
                {
                    request.UserDefinedFields[k] = new PayHost.KeyValueType();
                    request.UserDefinedFields[k].key = r["userKey" + (k + 1)];
                    request.UserDefinedFields[k].value = r["userField" + (k + 1)];
                }

            }
        }

        return request;
    }

    public static PayHost.DeleteVaultRequestType makeDeleteVaultRequest(HttpRequestBase r)
    {
        PayHost.DeleteVaultRequestType request = new PayHost.DeleteVaultRequestType();
        request.Account = new PayHost.PayGateAccountType();
        if (r["payGateId"] != null && r["payGateId"] != "") request.Account.PayGateId = r["payGateId"];
        if (r["encryptionKey"] != null && r["encryptionKey"] != "") request.Account.Password = r["encryptionKey"];

        if (r["vaultId"] != null && r["vaultId"] != "") request.VaultId = r["vaultId"];

        //User defined fields
        int j = 0;
        while (r["userKey" + (j + 1)] != null && r["userField" + (j + 1)] != null)
        {
            j++;
        }
        if (j > 0)
        {
            request.UserDefinedFields = new PayHost.KeyValueType[j];
            for (int k = 0; k < j; k++)
            {
                if (r["userKey" + (k + 1)] != null && r["userField" + (k + 1)] != null)
                {
                    request.UserDefinedFields[k] = new PayHost.KeyValueType();
                    request.UserDefinedFields[k].key = r["userKey" + (k + 1)];
                    request.UserDefinedFields[k].value = r["userField" + (k + 1)];
                }

            }
        }

        return request;
    }

    public static PayHost.CardVaultRequestType makeCardVaultRequest(HttpRequestBase r)
    {
        PayHost.CardVaultRequestType request = new PayHost.CardVaultRequestType();
        request.Account = new PayHost.PayGateAccountType();
        if (r["payGateId"] != null && r["payGateId"] != "") request.Account.PayGateId = r["payGateId"];
        if (r["encryptionKey"] != null && r["encryptionKey"] != "") request.Account.Password = r["encryptionKey"];
        if (r["cardNumber"] != null && r["cardNumber"] != "") request.CardNumber = r["cardNumber"];
        if (r["expiryDate"] != null && r["expiryDate"] != "") request.CardExpiryDate = r["expiryDate"];

        //User defined fields
        int j = 0;
        while (r["userKey" + (j + 1)] != null && r["userField" + (j + 1)] != null)
        {
            j++;
        }
        if (j > 0)
        {
            request.UserDefinedFields = new PayHost.KeyValueType[j];
            for (int k = 0; k < j; k++)
            {
                if (r["userKey" + (k + 1)] != null && r["userField" + (k + 1)] != null)
                {
                    request.UserDefinedFields[k] = new PayHost.KeyValueType();
                    request.UserDefinedFields[k].key = r["userKey" + (k + 1)];
                    request.UserDefinedFields[k].value = r["userField" + (k + 1)];
                }

            }
        }

        return request;
    }

    public static PayHost.WebPaymentRequestType makeWebPaymentRequest(HttpRequestBase r)
    {
        PayHost.WebPaymentRequestType request = new PayHost.WebPaymentRequestType();
        var amount = "";
        //Account Detail - required
        request.Account = new PayHost.PayGateAccountType();
        if (r["pgid"] != null && r["pgid"] != "") request.Account.PayGateId = r["pgid"];
        SessionModel.pgid = request.Account.PayGateId;
        if (r["encryptionKey"] != null && r["encryptionKey"] != "") request.Account.Password = r["encryptionKey"];
        SessionModel.key = request.Account.Password;
        //Customer Detail - required
        request.Customer = new PayHost.PersonType();
        if (r["customerTitle"] != null && r["customerTitle"] != "") request.Customer.Title = r["customerTitle"]; //Optional
        if (r["firstName"] != null && r["firstName"] != "") request.Customer.FirstName = r["firstName"]; //Required
        if (r["middleName"] != null && r["middleName"] != "") request.Customer.MiddleName = r["middleName"]; //Optional
        if (r["lastName"] != null && r["lastName"] != "") request.Customer.LastName = r["lastName"]; //Required
        if (r["telephone"] != null && r["telephone"] != "")
        { //Optional
            request.Customer.Telephone = new string[1]; //Multiple entries allowed. In this demo make provision for one only
            request.Customer.Telephone[0] = r["telephone"]; //Optional
        }
        if (r["mobile"] != null && r["mobile"] != "")
        { //Optional
            request.Customer.Mobile = new string[1]; //Multiple entries allowed. In this demo make provision for one only
            request.Customer.Mobile[0] = r["mobile"]; //Optional
        }
        if (r["fax"] != null && r["fax"] != "")
        { //Optional
            request.Customer.Fax = new string[1]; //Multiple entries allowed. In this demo make provision for one only
            request.Customer.Fax[0] = r["fax"]; //Optional
        }
        if (r["email"] != null && r["email"] != "")
        { //Required
            request.Customer.Email = new string[1]; //Multiple entries allowed. In this demo make provision for one only
            request.Customer.Email[0] = r["email"]; //One required
        }
        if (r["dateOfBirth"] != null && r["dateOfBirth"] != "")
        { //Optional
            request.Customer.DateOfBirth = DateTime.Parse(r["dateOfBirth"]); //Optional
        }
        if (r["nationality"] != null && r["nationality"] != "") request.Customer.Nationality = r["nationality"]; //Optional
        if (r["idNumber"] != null && r["idNumber"] != "") request.Customer.IdNumber = r["idNumber"]; //Optional
        if (r["idType"] != null && r["idType"] != "") request.Customer.IdType = r["idType"]; //Optional
        if (r["socialSecurity"] != null && r["socialSecurity"] != "") request.Customer.SocialSecurityNumber = r["socialSecurity"]; //Optional
        if (r["incCustomer"] == "incCustomer")
        {
            request.Customer.Address = new PayHost.AddressType();
            request.Customer.Address.AddressLine = new string[3];
            request.Customer.Address.AddressLine[0] = r["addressLine1"]; //Optional
            request.Customer.Address.AddressLine[1] = r["addressLine2"]; //Optional
            request.Customer.Address.AddressLine[2] = r["addressLine3"]; //Optional
            if (r["city"] != null && r["city"] != "") { request.Customer.Address.City = r["city"]; } //Optional
            if (r["country"] != null && r["country"] != "")
            {
                request.Customer.Address.Country = new PayHost.CountryType();
                request.Customer.Address.Country = (PayHost.CountryType)Enum.Parse(typeof(PayHost.CountryType), r["country"]); //Optional
                request.Customer.Address.CountrySpecified = true;
            }
            if (r["state"] != null && r["state"] != "") request.Customer.Address.State = r["state"]; //Optional
            if (r["zip"] != null && r["zip"] != "") request.Customer.Address.Zip = r["zip"]; //Optional
        }

        //Payment type - optional
        request.PaymentType = new PayHost.PaymentType[1];
        request.PaymentType[0] = new PayHost.PaymentType();
        if(r["payMethod"] != null && r["payMethod"] != "")
        {
            request.PaymentType[0].Method = new PayHost.PaymentMethodType();
            request.PaymentType[0].Method = (PayHost.PaymentMethodType)Enum.Parse(typeof(PayHost.PaymentMethodType), r["payMethod"]);
        }
        if(r["payMethodDetail"] != null && r["payMethodDetail"] != "")
        {
            request.PaymentType[0].Detail = r["payMethodDetail"];
        }

        //Redirect Detail - required
        request.Redirect = new PayHost.RedirectRequestType();
        if (r["retUrl"] != null && r["retUrl"] != "") request.Redirect.ReturnUrl = r["retUrl"];
        if (r["notifyUrl"] != null && r["notifyUrl"] != "") request.Redirect.NotifyUrl = r["notifyUrl"];

        //Order Detail - required
        request.Order = new PayHost.OrderType();
        if (r["reference"] != null && r["reference"] != "") request.Order.MerchantOrderId = r["reference"]; //required
        SessionModel.reference = request.Order.MerchantOrderId;
        if (r["currency"] != null && r["currency"] != "")
        {
            request.Order.Currency = (PayHost.CurrencyType)Enum.Parse(typeof(PayHost.CurrencyType), r["currency"]); //required
        }
        if (r["amount"] != null && r["amount"] != "") //required
        {
            //Replace decimal comma with decimal point if needed
            amount = r["amount"].Replace(",", ".");
            //And multiply to get cents as required
            request.Order.Amount = (int)(100 * Decimal.Parse(r["amount"], CultureInfo.InvariantCulture)); //required
        }
        if (r["transDate"] != null && r["transDate"] != "") request.Order.TransactionDate = DateTime.ParseExact(r["transDate"], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture); //optional

        //Order->BillingDetails->Address->Country is a required field
        //So we have to add a Billing Deatils entry
        if (r["incBilling"] == "incBilling")
        {
            request.Order.BillingDetails = new PayHost.BillingDetailsType();
            request.Order.BillingDetails.Customer = request.Customer;
            request.Order.BillingDetails.Address = new PayHost.AddressType();
            request.Order.BillingDetails.Address.AddressLine = new string[3];
            request.Order.BillingDetails.Address.AddressLine[0] = r["addressLine1"]; //Optional
            request.Order.BillingDetails.Address.AddressLine[1] = r["addressLine2"]; //Optional
            request.Order.BillingDetails.Address.AddressLine[2] = r["addressLine3"]; //Optional
            if (r["city"] != null && r["city"] != "") { request.Order.BillingDetails.Address.City = r["city"]; } //Optional

            //request.Order.BillingDetails.Address.Country = PayHost.CountryType.ZAF; //Optional
            if (r["country"] != null && r["country"] != "")
            {
                request.Order.BillingDetails.Address.Country = new PayHost.CountryType();
                request.Order.BillingDetails.Address.Country = (PayHost.CountryType)Enum.Parse(typeof(PayHost.CountryType), r["country"]); //Optional
                
                //Must set this field for serialization to work
                request.Order.BillingDetails.Address.CountrySpecified = true;
            }
            if (r["state"] != null && r["state"] != "") request.Order.BillingDetails.Address.State = r["state"]; //Optional
            if (r["zip"] != null && r["zip"] != "") request.Order.BillingDetails.Address.Zip = r["zip"]; //Optional
            //PayHost.CountryType
        }

        //Get shipping details - optional
        if(r["incShipping"] == "incShipping")
        {
            request.Order.ShippingDetails = new PayHost.ShippingDetailsType();
            request.Order.ShippingDetails.Customer = request.Order.BillingDetails.Customer; //Required
            request.Order.ShippingDetails.Address = request.Order.BillingDetails.Address; //Required
            if(r["deliveryDate"] != null && r["deliveryDate"] != "")
            {
                request.Order.ShippingDetails.DeliveryDate = DateTime.Parse(r["deliveryDate"]);
                request.Order.ShippingDetails.DeliveryDateSpecified = true;
            }
            if(r["deliveryMethod"] != null && r["deliveryMethod"] != "")
            {
                request.Order.ShippingDetails.DeliveryMethod = r["deliveryMethod"];
            }
            if(r["installRequired"] == "installRequired")
            {
                request.Order.ShippingDetails.InstallationRequested = true;
                request.Order.ShippingDetails.InstallationRequestedSpecified = true;
            }
        }

        //Get airline fields - optional
        if(r["PNR"] != null && r["PNR"] != "")
        {
            request.Order.AirlineBookingDetails = new PayHost.AirlineBookingDetailsType();
            request.Order.AirlineBookingDetails.PNR = r["PNR"];
            if (r["ticketNumber"] != null && r["ticketNumber"] != "") request.Order.AirlineBookingDetails.TicketNumber = r["ticketNumber"];
            request.Order.AirlineBookingDetails.Passengers = new PayHost.PassengerType[1];
            request.Order.AirlineBookingDetails.Passengers[0] = new PayHost.PassengerType();
            if(r["travellerType"] != null && r["travellerType"] != "")
            {
                request.Order.AirlineBookingDetails.Passengers[0].TravellerType = (PayHost.PassengerTypeTravellerType)Enum.Parse(typeof(PayHost.PassengerTypeTravellerType), r["travellerType"]);
            }
            request.Order.AirlineBookingDetails.Passengers[0].Passenger = request.Order.BillingDetails.Customer;

            //Flight legs
            request.Order.AirlineBookingDetails.FlightLegs = new PayHost.FlightLegType[1];
            request.Order.AirlineBookingDetails.FlightLegs[0] = new PayHost.FlightLegType();
            if (r["departureAirport"] != null && r["departureAirport"] != "") request.Order.AirlineBookingDetails.FlightLegs[0].DepartureAirport = r["departureAirport"];
            if (r["departureCountry"] != null && r["departureCountry"] != "") request.Order.AirlineBookingDetails.FlightLegs[0].DepartureCountry = (PayHost.CountryType)Enum.Parse(typeof(PayHost.CountryType), r["departureCountry"]);
            if (r["departureCity"] != null && r["departureCity"] != "") request.Order.AirlineBookingDetails.FlightLegs[0].DepartureCity = r["departureCity"];
            if (r["departureCountry"] != null && r["departureCountry"] != "") request.Order.AirlineBookingDetails.FlightLegs[0].DepartureCountry = (PayHost.CountryType)Enum.Parse(typeof(PayHost.CountryType), r["departureCountry"]);
            if (r["departureDateTime"] != null && r["departureDateTime"] != "") request.Order.AirlineBookingDetails.FlightLegs[0].DepartureDateTime = DateTime.Parse(r["departureDateTime"]);
            if (r["arrivalAirport"] != null && r["arrivalAirport"] != "") request.Order.AirlineBookingDetails.FlightLegs[0].ArrivalAirport = r["arrivalAirport"];
            if (r["arrivalCountry"] != null && r["arrivalCountry"] != "") request.Order.AirlineBookingDetails.FlightLegs[0].ArrivalCountry = (PayHost.CountryType)Enum.Parse(typeof(PayHost.CountryType), r["arrivalCountry"]);
            if (r["arrivalCity"] != null && r["arrivalCity"] != "") request.Order.AirlineBookingDetails.FlightLegs[0].ArrivalCity = r["arrivalCity"];
            if (r["arrivalCountry"] != null && r["arrivalCountry"] != "") request.Order.AirlineBookingDetails.FlightLegs[0].ArrivalCountry = (PayHost.CountryType)Enum.Parse(typeof(PayHost.CountryType), r["arrivalCountry"]);
            if (r["arrivalDateTime"] != null && r["arrivalDateTime"] != "") request.Order.AirlineBookingDetails.FlightLegs[0].ArrivalDateTime = DateTime.Parse(r["arrivalDateTime"]);

            //Carrier info
            if (r["marketingCarrierCode"] != null && r["marketingCarrierCode"] != "") request.Order.AirlineBookingDetails.FlightLegs[0].MarketingCarrierCode = r["marketingCarrierCode"];
            if (r["marketingCarrierName"] != null && r["marketingCarrierName"] != "") request.Order.AirlineBookingDetails.FlightLegs[0].MarketingCarrierName = r["marketingCarrierName"];
            if (r["issuingCarrierCode"] != null && r["issuingCarrierCode"] != "") request.Order.AirlineBookingDetails.FlightLegs[0].IssuingCarrierCode = r["issuingCarrierCode"];
            if (r["issuingCarrierName"] != null && r["issuingCarrierName"] != "") request.Order.AirlineBookingDetails.FlightLegs[0].IssuingCarrierName = r["issuingCarrierName"];

            //Flight Number
            if (r["flightNumber"] != null && r["flightNumber"] != "") request.Order.AirlineBookingDetails.FlightLegs[0].FlightNumber = r["flightNumber"];
        
        }

        //Get risk fields
        if(r["riskAccNum"] != null && r["riskAccNum"] != "")
        {
            request.Risk = new PayHost.RiskType();
            request.Risk.AccountNumber = r["riskAccNum"];
            if(r["riskIpAddr"] != null && r["riskIpAddr"] != "")
            {
                request.Risk.IpV4Address = r["riskIpAddr"];
            }
        }

        //User defined fields
        int j = 0;
        while(r["userKey" + (j + 1)] != null && r["userField" + (j + 1)] != null)
        {
            j++;
        }
        if(j > 0)
        {
            request.UserDefinedFields = new PayHost.KeyValueType[j];
            for(int k=0; k < j; k++)
            {
                if (r["userKey" + (k + 1)] != null && r["userField" + (k + 1)] != null)
                {
                    request.UserDefinedFields[k] = new PayHost.KeyValueType();
                    request.UserDefinedFields[k].key = r["userKey" + (k + 1)];
                    request.UserDefinedFields[k].value = r["userField" + (k + 1)];
                }

            }
        }

        return request;
    }

    public static string getSOAPText(Object requestObject)
    {
        SoapFormatter soapFormatter = new SoapFormatter();
        XmlSerializer xmlSerializer = new XmlSerializer(requestObject.GetType());
        var byteArray = new Byte[10000];
        //using (Stream s = new MemoryStream(byteArray))
        using (Stream s = new FileStream("d:soap.soap", FileMode.Create))
        {
            soapFormatter.Serialize(s, requestObject);
            //xmlSerializer.Serialize(s, requestObject);

            var str = new StringBuilder();
            for (var i = 0; i < byteArray.Length; i++)
            {
                str.Append((char)byteArray[i]);
            }
            return str.ToString();
        }
    }

    public static string getXMLText(Object requestObject)
    {
        XmlSerializer xmlSerializer = new XmlSerializer(requestObject.GetType());
        using (MemoryStream s = new MemoryStream())
        //using (Stream s = new FileStream("d:xml.xml", FileMode.Create))
        {
            xmlSerializer.Serialize(s, requestObject);
            var byteArray = s.GetBuffer();
            var str = new StringBuilder();
            for (var i = 0; i < byteArray.Length; i++)
            {
                str.Append((char)byteArray[i]);
            }
            return str.ToString();
        }
    }

    public static string htmlAfterAuth(string lastRequest, string responseText, string redirectText)
    {
        var html = @"<div class=""row"" style=""margin-bottom: 15px; "">
                   <div class=""col-sm-offset-4 col-sm-4"">
                    <button id = ""showRequestBtn"" class=""btn btn-primary btn-block"" type=""button"" data-toggle=""collapse"" data-target=""#requestDiv"" aria-expanded=""false"" aria-controls=""requestDiv"">
                        Request
                    </button>
                </div>
            </div>
            <div id = ""requestDiv"" class=""row collapse well well-sm"">
                <textarea rows = ""20"" cols=""100"" id=""request"" class=""form-control"">" + lastRequest;
        html += @"</textarea>
            </div>
            <div class=""row"" style=""margin-bottom: 15px; "">
                   <div class=""col-sm-offset-4 col-sm-4"">
                    <button id = ""showResponseBtn"" class=""btn btn-primary btn-block"" type=""button"" data-toggle=""collapse"" data-target=""#responseDiv"" aria-expanded=""false"" aria-controls=""responseDiv"">
                        Response
                    </button>
                </div>
            </div>
            <div id = ""responseDiv"" class=""row collapse well well-sm"">
                <textarea rows = ""20"" cols=""100"" id=""response"" class=""form-control"">" + responseText;
        html += @"</textarea>
            </div>" + redirectText;
            
            
        return html;
    }
}