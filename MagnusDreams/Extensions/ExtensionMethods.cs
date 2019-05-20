using System;
using System.Windows;
using System.Windows.Threading;

namespace MagnusDreams.Extensions
{
    /// <summary>
    /// Renders the UIElement in canvas
    /// </summary>
    public static class ExtensionMethods
    {
        private static Action EmptyDelegate = delegate () { };
        public static void Refresh(this UIElement uiElement)
        {
            uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
        }
    }
}
