/*
 * Copyright (c) 2018 PayGate (Pty) Ltd
 *
 * Author: App Inlet (Pty) Ltd
 * 
 * Released under the GNU General Public License
 */

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Web;
using System.Text;
using System.Net.Http;
using System.Net;

/// <summary>
/// Class that provides initiate and query functionality for PayGate PayWeb3 applications
/// </summary>
public class PaygatePayweb3
{
    /**
     * The url of the PayGate PayWeb 3 initiate page
     */
    static public string initiate_url = "https://secure.paygate.co.za/payweb3/initiate.trans";

    /**
	 * The url of the PayGate PayWeb 3 process page
	 */
    static public string process_url = "https://secure.paygate.co.za/payweb3/process.trans";

    /**
	 * The url of the PayGate PayWeb 3 query page
	 */
    static public string query_url = "https://secure.paygate.co.za/payweb3/query.trans";

    /**
	 * Dictionary contains the data to be posted to PayGate PayWeb 3 initiate
	 */
    private Dictionary<string, string> initiateRequest = new Dictionary<string, string>();

    /**
	 * Dictionary contains the response data from the initiate
	 */
    public Dictionary<string, string> initiateResponse = new Dictionary<string, string>();

    /**
	 * Dictionary contains the data returned from the initiate, required for the redirect of the client
	 */
    public Dictionary<string, string> processRequest = new Dictionary<string, string>();

    /**
	 * Dictionary contains the data to be posted to PayGate PayWeb 3 query service
	 */
    private Dictionary<string, string> queryRequest = new Dictionary<string, string>();

    /**
	 * @var Dictionary contains the response data from the query
	 */
    public Dictionary<string, string> queryResponse;

    /**
	 * String
	 *
	 * Most common errors returned will be:
	 *
	 * DATA_CHK    -> Checksum posted does not match the one calculated by PayGate, either due to an incorrect encryption key used or a field that has been excluded from the checksum calculation
	 * DATA_PW     -> Mandatory fields have been excluded from the post to PayGate, refer to page 9 of the documentation as to what fields should be posted.
	 * DATA_CUR    -> The currency that has been posted to PayGate is not supported.
	 * PGID_NOT_EN -> The PayGate ID being used to post data to PayGate has not yet been enabled, or there are no payment methods setup on it.
	 *
	 */
    public string lastError;

    private Dictionary<int, string> transactionStatusArray = new Dictionary<int, string>()
    {
        {1, "Approved" },
        {2, "Declined" },
        {4, "Cancelled" }
    };

    /**
     * Private fields for debugging and ssl/tls status
     * 
     */
    private bool debug = false;
    private bool ssl = false;

    /**
	 * String (as set up on the PayWeb 3 config page in the PayGate Back Office )
	 */
    private string encryptionKey;

    /**
     * Series of getter / setter functions
     */
    public bool isDebug()
    {
        return this.debug;
    }

    public void setDebug(bool debug)
    {
        this.debug = debug;
    }

    public bool isSsl()
    {
        return this.ssl;
    }

    public void setSsl(bool ssl)
    {
        this.ssl = ssl;
    }

    public Dictionary<string, string> getInitiateRequest()
    {
        return this.initiateRequest;
    }

    public void setInitiateRequest(Dictionary<string, string> postData)
    {
        this.initiateRequest = postData;
    }

    public Dictionary<string, string> getQueryRequest()
    {
        return this.queryRequest;
    }

    public void setQueryRequest(Dictionary<string, string> queryRequest)
    {
        this.queryRequest = queryRequest;
    }

    public string getEncryptionKey()
    {
        return this.encryptionKey;
    }

    public void setEncryptionKey(string encryptionKey)
    {
        this.encryptionKey = encryptionKey;
    }

    /**
	 * Returns a description of the transaction status number passed back from PayGate
	 *
	 * @param int statusNumber
	 * @return string
	 */
    public string getTransactionStatusDescription(int statusNumber)
    {
        return this.transactionStatusArray[statusNumber];
    }

    /**
	 * Function to generate the checksum to be passed in the initiate call. Refer to examples on Page 19 of the PayWeb3 documentation (Version 1.03.2)
	 *
	 * @param Dictionary<string, string> postData
	 * @return string (md5 hash value)
	 */
    public string generateChecksum(Dictionary<string, string> postData)
    {
		string checksum = "";

        foreach (var item in postData){
            if (item.Value != ""){
				checksum += item.Value;
            }
        }

		checksum += this.getEncryptionKey();

        if (this.isDebug()){
            Console.Write("Checksum Source: {0}", checksum);
        }

        var hash = MD5.Create().ComputeHash(System.Text.Encoding.ASCII.GetBytes(checksum));
        StringBuilder sb = new StringBuilder();
        for(int i = 0; i < hash.Length; i++)
        {
            sb.Append(hash[i].ToString("x2"));
        }

        return sb.ToString();
    }

    /**
     * Function to compare checksums 
     * 
     * @param Dictionary<string, string> data
     * @return bool
    */
    public bool validateChecksum(Dictionary<string,string> data)
    {
        string returnedChecksum = data["CHECKSUM"];
        data.Remove("CHECKSUM");

        string checksum = this.generateChecksum(data);
        return String.Compare(returnedChecksum, checksum) == 0;
    }

    /**
	 * Function to handle response from initiate request and set error or processRequest as need be
	 *
	 * @return bool
	 */
    public bool handleInitiateResponse()
    {
        //if (array_key_exists('ERROR', $this->initiateResponse))
        if (this.initiateResponse.ContainsKey("ERROR"))
        {
			this.lastError = this.initiateResponse["ERROR"];
            this.initiateResponse.Clear();
            return false;
        }

        this.processRequest["PAY_REQUEST_ID"] = this.initiateResponse["PAY_REQUEST_ID"];
        this.processRequest["CHECKSUM"] = this.initiateResponse["CHECKSUM"];
        return true;
    }

    /**
	 * Function to handle response from Query request and set error as need be
	 *
	 * @return bool
	 */
    public bool handleQueryResponse()
    {
        if (this.queryResponse.ContainsKey("ERROR"))
        {
			this.lastError = this.queryResponse["ERROR"];
            this.queryResponse.Clear();
            return false;
        }
        return true;
    }

    /**
	 * Function to do post to PayGate to initiate a PayWeb 3 transaction
	 *
	 * @return bool
	 */
    public bool doInitiate()
    {

		this.initiateRequest["CHECKSUM"] = this.generateChecksum(this.initiateRequest);

		var result = this.doCurlPost(this.initiateRequest, initiate_url);
        var type = result.GetType();

        if (type.Name == "String"){
            var response = HttpUtility.ParseQueryString(result);
            foreach(var key in response.AllKeys)
            {
                var test = response.Get(key);
                this.initiateResponse[key] = response.Get(key);
            }
			result = this.handleInitiateResponse();
        }

        return result;
    }

    /**
	 * Function to do  post to PayGate to query a PayWeb 3 transaction
	 *
	 * @return bool
	 */
    public bool doQuery()
    {
		this.queryRequest["CHECKSUM"] = this.generateChecksum(this.queryRequest);

		var result = this.doCurlPost(this.queryRequest, query_url);
        var type = result.GetType();


        if (type.Name == "String" ){
            this.queryResponse = new Dictionary<string, string>();
            var response = HttpUtility.ParseQueryString(result);
            foreach(var key in response.AllKeys)
            {
                this.queryResponse[key] = response.Get(key);
            }
			result = this.handleQueryResponse();
        }

        return result;
    }

    /**
	 * function to do actual post to PayGate
	 *
	 * @param Dictionary postData - data to be posted
	 * @param string url - Url to be posted to
	 * @return bool | string
	 */
    public dynamic doCurlPost(Dictionary <string, string> postData, string url)
    {
        var client = new HttpClient();
        var content = new FormUrlEncodedContent(postData);

        client.BaseAddress = new Uri(url);

        //Check to see if is a secure connection
        if (!isSsl())
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        var response = client.PostAsync(url, content).Result;
        if (response.IsSuccessStatusCode)
        {
            var responseString = response.Content.ReadAsStringAsync().Result;
            return responseString;
        }
        return false;
    }
}