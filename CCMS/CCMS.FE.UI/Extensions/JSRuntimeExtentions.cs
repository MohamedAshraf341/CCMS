using Microsoft.JSInterop;
using System.Threading.Tasks;
using System;

namespace CCMS.FE.UI.Extensions
{
    public static class JSRuntimeExtentions
    {
        public static ValueTask SaveAs(this IJSRuntime js, string filename, byte[] data)
            => js.InvokeVoidAsync(
                "dcidr.interop.saveAsFile",
                filename,
                Convert.ToBase64String(data));
    }
}
