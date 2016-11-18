#region Using

using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Web;
using System.Web.UI;

#endregion

namespace Cff.SaferTrader.Web
{
    /// <summary>
    /// Compresses the output using standard gzip/deflate.
    /// Ported from http://blog.madskristensen.dk/post/HTTP-compression-of-WebResourceaxd-and-pages-in-ASPNET.aspx
    /// </summary>
    public sealed class CompressionModule : IHttpModule
    {
        private const string DEFLATE = "deflate";
        private const string GZIP = "gzip";

        /// <summary>
        /// Compress the page when the request is not an AJAX request and the response is not an excel file
        /// </summary>
        private static void context_PreSendRequestContent(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            if (application.Context.CurrentHandler is Page
                && application.Request["HTTP_X_MICROSOFTAJAX"] == null
                && !application.Response.ContentType.Equals("application/xls")
                && !application.Response.ContentType.Equals("application/xlsx"))
            {
                if (IsEncodingAccepted(DEFLATE))
                {
                    application.Response.Filter = new DeflateStream(application.Response.Filter, CompressionMode.Compress);
                    SetEncoding(DEFLATE);
                }
                else if (IsEncodingAccepted(GZIP))
                {
                    application.Response.Filter = new GZipStream(application.Response.Filter, CompressionMode.Compress);
                    SetEncoding(GZIP);
                }
            }
        }

        private static bool IsEncodingAccepted(string encoding)
        {
            bool isEncodingAccepted = false;
            HttpContext context = HttpContext.Current;
            if (context != null)
            {
                isEncodingAccepted = context.Request.Headers["Accept-encoding"] != null &&
                   context.Request.Headers["Accept-encoding"].Contains(encoding);
            }
            return isEncodingAccepted;
        }

        private static void SetEncoding(string encoding)
        {
            try
            {
                HttpContext.Current.Response.AppendHeader("Content-encoding", encoding);
            } catch (Exception) {
            }
        }

        /// <summary>
        /// Compress WebResource.axd
        /// </summary>
        private static void context_BeginRequest(object sender, EventArgs e)
        {
            var app = (HttpApplication) sender;
            if (app.Request.Path.Contains("WebResource.axd"))
            {
                SetCachingHeaders(app);

                if (IsBrowserSupported() && app.Context.Request.QueryString["c"] == null &&
                    (IsEncodingAccepted(DEFLATE) || IsEncodingAccepted(GZIP)))
                    app.CompleteRequest();
            }
        }
        
        private static void context_EndRequest(object sender, EventArgs e)
        {
            if (!IsBrowserSupported() || (!IsEncodingAccepted(DEFLATE) && !IsEncodingAccepted(GZIP)))
                return;

            var app = (HttpApplication) sender;
            string key = app.Request.QueryString.ToString();
            if (app.Request.Path.Contains("WebResource.axd") && app.Context.Request.QueryString["c"] == null)
            {
                if (app.Application[key] == null)
                {
                    AddCompressedBytesToCache(app, key);
                }

                SetEncoding((string)app.Application[key + "enc"]);
                app.Context.Response.ContentType = "text/javascript";
                app.Context.Response.BinaryWrite((byte[]) app.Application[key]);
            }
        }

        /// <summary>
        /// Sets the caching headers and monitors the If-None-Match request header,
        /// to save bandwidth and CPU time.
        /// </summary>
        private static void SetCachingHeaders(HttpApplication app)
        {
            string etag = "\"" + app.Context.Request.QueryString.ToString().GetHashCode() + "\"";
            string incomingEtag = app.Request.Headers["If-None-Match"];

            app.Response.Cache.VaryByHeaders["Accept-Encoding"] = true;
            app.Response.Cache.SetExpires(DateTime.Now.AddDays(30));
            app.Response.Cache.SetCacheability(HttpCacheability.Public);
            app.Response.Cache.SetLastModified(DateTime.Now.AddDays(-30));
            app.Response.Cache.SetETag(etag);

            if (String.Compare(incomingEtag, etag, StringComparison.OrdinalIgnoreCase) == 0)
            {
                app.Response.StatusCode = (int) HttpStatusCode.NotModified;
                app.Response.End();
            }
        }

        /// <summary>
        /// Check if the browser is Internet Explorer 6 that have a known bug with compression
        /// </summary>
        /// <returns></returns>
        private static bool IsBrowserSupported()
        {
            // Because of bug in Internet Explorer 6
            HttpContext context = HttpContext.Current;
            return !(context.Request.UserAgent != null && context.Request.UserAgent.Contains("MSIE 6"));
        }

        /// <summary>
        /// Adds a compressed byte array into the application items.
        /// <remarks>
        /// This is done for performance reasons so it doesn't have to
        /// create an HTTP request every time it serves the WebResource.axd.
        /// </remarks>
        /// </summary>
        private static void AddCompressedBytesToCache(HttpApplication app, string key)
        {
            var request = (HttpWebRequest) WebRequest.Create(app.Context.Request.Url.OriginalString + "&c=1");
            using (var response = request.GetResponse() as HttpWebResponse)
            {
                if (response != null)
                {
                    
                    Stream responseStream = response.GetResponseStream();
                    using (MemoryStream ms = CompressResponse(responseStream, app, key))
                    {
                        byte[] buffer = ms.ToArray();
                        if (buffer.Length == 0)
                        {
                            buffer = new[] { byte.MinValue };
                        }
                        app.Application.Add(key, buffer);
                    }
                }
            }
        }

        /// <summary>
        /// Compresses the response stream if the browser allows it.
        /// </summary>
        private static MemoryStream CompressResponse(Stream responseStream, HttpApplication app, string key)
        {
            var dataStream = new MemoryStream();
            StreamCopy(responseStream, dataStream);
            responseStream.Dispose();

            byte[] buffer = dataStream.ToArray();
            dataStream.Dispose();

            var ms = new MemoryStream();
            Stream compress = null;

            if (IsEncodingAccepted(DEFLATE))
            {
                compress = new DeflateStream(ms, CompressionMode.Compress);
                app.Application.Add(key + "enc", DEFLATE);
            }
            else if (IsEncodingAccepted(GZIP))
            {
                compress = new GZipStream(ms, CompressionMode.Compress);
                app.Application.Add(key + "enc", GZIP);
            }

            if (compress != null)
            {
                compress.Write(buffer, 0, buffer.Length);
                compress.Dispose();
            }
            return ms;
        }

        /// <summary>
        /// Copies one stream into another.
        /// </summary>
        private static void StreamCopy(Stream input, Stream output)
        {
            var buffer = new byte[2048];
            int read;
            do
            {
                read = input.Read(buffer, 0, buffer.Length);
                output.Write(buffer, 0, read);
            } while (read > 0);
        }

        #region IHttpModule Members

        /// <summary>
        /// Disposes of the resources (other than memory) used by the module 
        /// that implements <see cref="T:System.Web.IHttpModule"></see>.
        /// </summary>
        void IHttpModule.Dispose()
        {
            // Nothing to dispose; 
        }

        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpApplication"></see> 
        /// that provides access to the methods, properties, and events common to 
        /// all application objects within an ASP.NET application.
        /// </param>
        void IHttpModule.Init(HttpApplication context)
        {
            // For WebResource.axd compression
            context.BeginRequest += context_BeginRequest;
            context.EndRequest += context_EndRequest;
            context.PreSendRequestContent += context_PreSendRequestContent;
        }

        #endregion
    }
}