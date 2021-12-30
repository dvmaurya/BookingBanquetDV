using CommonServices.Extensions;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace CommonServices.Utilities
{
    public static class CursorUtility
    {
        public static void ArrowCursor()
        {
            Window.Current.CoreWindow.PointerCursor =
                new CoreCursor(CoreCursorType.Arrow, (uint)CoreCursorType.Arrow.ToInt32());
        }

        public static void HandCursor()
        {
            Window.Current.CoreWindow.PointerCursor =
                new CoreCursor(CoreCursorType.Hand, (uint)CoreCursorType.Hand.ToInt32());
        }

        public static void CustomCursor(int id)
        {
            Window.Current.CoreWindow.PointerCursor =
                new CoreCursor(CoreCursorType.Custom, (uint)id.ToInt32());
        }
    }
}
