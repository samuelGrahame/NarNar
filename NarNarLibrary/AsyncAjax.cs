using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Retyped.dom;

namespace NarNarLibrary
{
    public class AsyncAjax : IPromise
    {    
        public void Then(Delegate fulfilledHandler, Delegate errorHandler = null, Delegate progressHandler = null)
        {
            var request = new XMLHttpRequest();
            request.onreadystatechange = (ev) =>
            {
                if (request.readyState != 4)
                {
                    return;
                }

                if ((request.status == 200) || (request.status == 304))
                {
                    fulfilledHandler.Call(null, request);
                }
                else
                {
                    errorHandler.Call(null, request.responseText);
                }
            };

            if(!string.IsNullOrWhiteSpace(Options.ContentType))
                request.setRequestHeader("Content-type", Options.ContentType);

            request.open(Options.Method, Options.Url);
            if(Options.Method.ToLower() == "post")
                request.send(Options.Data);
            else
                request.send();
        }

        public AsyncAjaxOptions Options;
        
        public AsyncAjax(AsyncAjaxOptions options)
        {
            Options = options;
        }

        public static async Task<string>Ajax(AsyncAjaxOptions options)
        {
            var task = Task.FromPromise<string>(
                new AsyncAjax(options),
                (Func<XMLHttpRequest, string>)((request) => request.responseText)
            );

            return await task;
        }
    }

    public class AsyncAjaxOptions
    {
        public string Url;
        public string Data;
        public string ContentType;
        public string Method;
    }
}
