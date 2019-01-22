using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NarNarLibrary
{
    public class NetworkHelper
    {
        public async Task<Root<Out>> Post<In, Out>(string url, Root<In> data)
        {            
            try
            {
                var result = await AsyncAjax.Ajax(new AsyncAjaxOptions()
                {
                    ContentType = "application/json",
                    Data = JsonConvert.SerializeObject(data),
                    Method = "POST",
                    Url = url
                });
                return JsonConvert.DeserializeObject<Root<Out>>(result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public class Root<T>
        {
            public T Result;
        }
    }
}
