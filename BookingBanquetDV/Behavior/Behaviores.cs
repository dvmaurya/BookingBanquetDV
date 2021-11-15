using System;
using System.Reflection;
using CommonServices.Extensions;
using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;

namespace BookingBanquetDV.Behavior
{
    public abstract class Behaviores<T> : DependencyObject, IBehavior where T : DependencyObject
    {
        public T AssociatedObject { get; private set; }
        private const string Assignable = "associated Object is not assignable to type:{0}";

        DependencyObject IBehavior.AssociatedObject
        {
            get { return this.AssociatedObject; }
        }

        public void Attach(DependencyObject associatedObject)
        {
            if (associatedObject != null && !typeof(T).GetTypeInfo().IsAssignableFrom(associatedObject.GetType().GetTypeInfo()))
                throw new ArgumentException(Assignable.FormatString(typeof(T)));

            this.AssociatedObject = associatedObject as T;
            this.OnAttached();
        }

        public void Detach()
        {
            this.OnDetached();
            this.AssociatedObject = null;
        }

        protected abstract void OnAttached();

        protected abstract void OnDetached();
    }
}
