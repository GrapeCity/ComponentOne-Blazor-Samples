using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace BlazorExplorer;

/// <summary>
/// Realizes access to the browser's local storage.
/// </summary>
/// <param name="js"> <see cref="IJSRuntime"/>. </param>
public class LocalStorageService(IJSRuntime js)
{
    private readonly IJSRuntime _js = js;
    private readonly bool _isWasm = js is IJSInProcessRuntime;

    /// <summary>
    /// Sets a value in the local storage.
    /// </summary>
    /// <param name="key">Key.</param>
    /// <param name="value">Value.</param>
    /// <returns></returns>
    public async Task SetAsync(string key, string value)
    {
        if (_isWasm)
        {
            ((IJSInProcessRuntime)_js).InvokeVoid("localStorage.setItem", key, value);
        }
        else
        {
            await _js.InvokeVoidAsync("localStorage.setItem", key, value);
        }
    }

    /// <summary>
    /// Gets a value from the local storage.
    /// </summary>
    /// <param name="key"> Key.</param>
    /// <returns> Value.</returns>
    public async Task<string> GetAsync(string key)
    {
        if (_isWasm)
        {
            return ((IJSInProcessRuntime)_js).Invoke<string>("localStorage.getItem", key);
        }
        else
        {
            return await _js.InvokeAsync<string>("localStorage.getItem", key);
        }
    }
}
