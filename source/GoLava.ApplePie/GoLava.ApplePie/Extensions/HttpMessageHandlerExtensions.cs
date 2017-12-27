using System;
using System.Net.Http;

namespace GoLava.ApplePie.Extensions
{
    /// <summary>
    /// Http message handler extensions.
    /// </summary>
    public static class HttpMessageHandlerExtensions
    {
        /// <summary>
        /// Assigns the inner handler to the <see cref="P:DelegatingHandler.InnerHandler"/> of the outer handler. 
        /// </summary>
        /// <returns>The the outer handler.</returns>
        /// <param name="innerHandler">The inner  handler to be assigned to the <see cref="P:DelegatingHandler.InnerHandler"/> of the outer handler.</param>
        /// <param name="outerHandler">The outer handler that will be become the parent of the inner handler.</param>
        public static T DecorateWith<T>(this HttpMessageHandler innerHandler, T outerHandler)
            where T: DelegatingHandler 
        {
            if (outerHandler == null)
                throw new ArgumentNullException(nameof(outerHandler));
            outerHandler.InnerHandler = innerHandler;
            return outerHandler;
        }
    }
}