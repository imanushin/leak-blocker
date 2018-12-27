using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;

namespace LeakBlocker.AdminView.Desktop.Controls.Standard.Audit
{
    internal sealed partial class EnumSelector
    {
        public event Action ValueChanged;

        public EnumSelector()
        {
            InitializeComponent();
        }

        public void SetItems(ICollection<object> allItems, ReadOnlySet<object> selected)
        {
            Check.CollectionHasOnlyMeaningfulData(allItems, "allItems");

            ReadOnlySet<object> outOfDictionaryItems = selected.Without(allItems).ToReadOnlySet();

            if (outOfDictionaryItems.Any())
            {
                string error = "These items are out of the dictionary: {0}".Combine(outOfDictionaryItems);

                throw new ArgumentException(error);
            }

            innerView.Items.Clear();

            foreach (object item in allItems)
            {
                var view = new EnumView(item);

                view.IsSelected = selected.Contains(item);

                innerView.Items.Add(view);
            }
        }

        public ReadOnlySet<object> SelectedItems
        {
            get
            {
                return innerView.Items.Cast<EnumView>().Where(view => view.IsSelected).Select(view => view.Enum).ToReadOnlySet();
            }
        }

        private void ValueChangedHandler(object sender, RoutedEventArgs e)
        {
            if (ValueChanged != null)
                ValueChanged();
        }
    }

    internal sealed class EnumView
    {
        private readonly string text;

        public EnumView(object value)
        {
            Enum = value;
            text = ((Enum)value).GetValueDescription();
        }

        public object Enum
        {
            get;
            private set;
        }

        public bool IsSelected
        {
            get;
            set;
        }

        public override string ToString()
        {
            return text;
        }
    }
}
