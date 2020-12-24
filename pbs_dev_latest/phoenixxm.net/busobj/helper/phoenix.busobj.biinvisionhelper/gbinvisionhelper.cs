#region (C) CopyRight
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2004 London Bridge Phoenix Software
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
//-------------------------------------------------------------------------------
// File Name: GbInvisionHelper.cs
// NameSpace: phoenix.busobj.biinvisionhelper
//-------------------------------------------------------------------------------
//Date			    Ver 	    Init    	    Change
//-------------------------------------------------------------------------------
//3/21/2017 	    1		    GlaxeenaJ	    Created.
//3/22/2017         2           GlaxeenaJ       Enh #209693, US #60591, Task #60827 Added helper class for BI_Invision to generate JWT token.
//-------------------------------------------------------------------------------
#endregion

using System;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

namespace phoenix.busobj.biinvisionhelper
{
    public class GbInvisionHelper
    {


        /// <summary>
        /// Different types of Hash Algoraith supported for JSON Token can be included below
        /// </summary>
        public enum JwtHashAlgorithm
        {
            HS256
        }

        /// <summary>
        /// Support for Json Token logic/methods
        /// </summary>
        private static Dictionary<JwtHashAlgorithm, Func<byte[], byte[], byte[]>> HashAlgorithms;

        static GbInvisionHelper()
        {
            HashAlgorithms = new Dictionary<JwtHashAlgorithm, Func<byte[], byte[], byte[]>>
        {
        // here we can add more algorithms upon need.
            { JwtHashAlgorithm.HS256, (key, value) => { using (var sha = new HMACSHA256(key)) { return sha.ComputeHash(value); } } },
            //{ JwtHashAlgorithm.HS512, (key, value) => { using (var sha = new HMACSHA512(key)) { return sha.ComputeHash(value); } } }
        };
        }

        /// <summary>
        /* Creates Request that to be sent to Invision server along with Authentication credentials*/
        /* GenerateJWTToken method - makes JWT token based on the credentials we pass*/
        /// </summary>
        /// <param name="url"></param>
        /// <param name="payload"></param>
        /// <param name="key"></param>
        /// <param name="issuer"></param>
        /// <returns></returns>
        public HttpClient GetClient(string url, string payload, string key)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            /*Creates and appends JWT token to header by calling helper method*/
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GenerateJWTToken(url, payload, key, "PHX", 4));

            return client;
        }

        /// <summary>
        /// Returns the JWT for XM calls
        /// </summary>
        /// <param name="url"></param>
        /// <param name="payload"></param>
        /// <param name="key"></param>
        /// <param name="issuer"></param>
        /// <param name="expiryInSeconds"></param>
        /// <returns></returns>
        /// this method gets invoke call from invison BO.
        public string GenerateJWTToken(string url, string payload, string key, string issuer, int expiryInSeconds)
        {
            if (string.IsNullOrEmpty(key))
                return null;

            JwtHashAlgorithm algorithm = JwtHashAlgorithm.HS256;
            var header = new { alg = algorithm.ToString(), typ = "JWT" };

            //payload: payloadString,
            //nbf: currentDateInSeconds,
            //exp: expiryDateInSeconds,
            //iat: currentDateInSeconds,
            //iss: issuerType,
            //aud: websiteurl
            var payloaddata = new { aud = url, iss = issuer, payload = payload, nbf = ConvertToUnixTimestamp(DateTime.UtcNow), exp = ConvertToUnixTimestamp(DateTime.UtcNow.AddSeconds(expiryInSeconds)), iat = ConvertToUnixTimestamp((DateTime.UtcNow)) };

            /*Encodes the header and payload into JWT format.*/
            return Encode(header, payloaddata, Encoding.UTF8.GetBytes(key), algorithm);


        }

        /// <summary>
        /// Encodes the header and payload into JWT format.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="payloaddata"></param>
        /// <param name="keyBytes"></param>
        /// <param name="algorithm"></param>
        /// <returns></returns>
        /// 
        private string Encode(object header, object payloaddata, byte[] keyBytes, JwtHashAlgorithm algorithm)
        {
            var segments = new List<string>();
            //serializes header and payloaddata object into bytes
            byte[] headerBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(header, Newtonsoft.Json.Formatting.None));
            byte[] payloadBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(payloaddata, Newtonsoft.Json.Formatting.None));

            //After converting binary to radix-64 , adds it to a list
            segments.Add(Base64UrlEncode(headerBytes));
            segments.Add(Base64UrlEncode(payloadBytes));

            
            var stringToSign = string.Join(".", segments.ToArray());
            var bytesToSign = Encoding.UTF8.GetBytes(stringToSign);

            //Hashalgorithms designed in the static constructor will be used here for creating signature
            //signature = HMACSHA256(token, secretkey);
            byte[] signature = HashAlgorithms[algorithm](keyBytes, bytesToSign);
            segments.Add(Base64UrlEncode(signature));

            //return signedToken;
            return string.Join(".", segments.ToArray());
        }


       //this method converts binary data to radix-64
        private static string Base64UrlEncode(byte[] input)
        {
            var output = Convert.ToBase64String(input);
            output = output.Split('=')[0]; // Remove any trailing '='s
            output = output.Replace('+', '-'); // 62nd char of encoding
            output = output.Replace('/', '_'); // 63rd char of encoding
            return output;
        }

        /// <summary>
        /// converts datetime to seconds
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }

    }
}
