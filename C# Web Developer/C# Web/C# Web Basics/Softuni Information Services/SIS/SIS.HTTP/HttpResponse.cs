﻿namespace SIS.HTTP
{
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Represents an HTTP Response with properties for the <c>Response Status Line</c>, <c>Response Headers</c> and <c>Response Body</c>.
    /// </summary>
    public class HttpResponse
    {
        //------------- CONSTRUCTORS -------------
        /// <summary>
        /// Initializes a new <see cref="HttpResponse"/> class.
        /// </summary>
        /// <param name="statusCode">Response Status Code</param>
        /// <param name="body">Response Body in byte[]</param>
        public HttpResponse(HttpResponseStatusCode statusCode, byte[] body)
            : this()
        {
            this.StatusCode = statusCode;
            this.Body = body;

            if (body?.Length > 0)
            {
                this.Headers.Add(new Header("Content-Length", body.Length.ToString()));
            }
        }

        internal HttpResponse()
        {
            this.Headers = new List<Header>();
            this.Cookies = new List<ResponseCookie>();
            this.Version = HttpVersionType.Http10;
        }

        //-------------- PROPERTIES --------------
        /// <summary>
        /// HTTP Response status line Version.
        /// </summary>
        public HttpVersionType Version { get; set; }

        /// <summary>
        /// HTTP Response status line Status Code.
        /// </summary>
        public HttpResponseStatusCode StatusCode { get; set; }

        /// <summary>
        /// Collection of HTTP Response Headers.
        /// </summary>
        public IList<Header> Headers { get; set; }

        /// <summary>
        /// Collection of HTTP Response Cookies.
        /// </summary>
        public IList<ResponseCookie> Cookies { get; set; }

        /// <summary>
        /// HTTP Response Body in the form of byte[].
        /// </summary>
        public byte[] Body { get; set; }

        //------------ PUBLIC METHODS ------------
        /// <summary>
        /// Returns formatted HTTP Response for the browser.
        /// </summary>
        /// <returns>Formatted HTTP Response</returns>
        public override string ToString()
        {
            StringBuilder responseAsString = new StringBuilder();
            string httpVersionAsString = this.Version switch
            {
                HttpVersionType.Http10 => "HTTP/1.0",
                HttpVersionType.Http11 => "HTTP/1.1",
                HttpVersionType.Http20 => "HTTP/2.0",
                _                      => "HTTP/1.1"
            };

            responseAsString.Append($"{httpVersionAsString} {(int)this.StatusCode} {this.StatusCode}" + HttpConstants.NewLine);
            foreach (Header header in this.Headers)
            {
                responseAsString.Append(header.ToString() + HttpConstants.NewLine);
            }

            foreach (Cookie cookie in this.Cookies)
            {
                responseAsString.Append("Set-Cookie: " + cookie.ToString() + HttpConstants.NewLine);
            }


            responseAsString.Append(HttpConstants.NewLine);

            return responseAsString.ToString();
        }
    }
}