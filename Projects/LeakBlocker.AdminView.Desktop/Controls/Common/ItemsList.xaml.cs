using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.AdminView.Desktop.Controls.Common
{
    internal sealed partial class ItemsList
    {
        public static readonly RoutedEvent AddButtonClickedEvent = EventManager.RegisterRoutedEvent(
            "AddButtonClicked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ItemsList));

        public static readonly RoutedEvent ItemsWereRemovedEvent = EventManager.RegisterRoutedEvent(
            "ItemsWereRemoved", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ItemsList));

        public ItemsList()
        {
            InitializeComponent();
        }

        public void SetControlsEnabled(bool isEnabled)
        {
            addButton.IsEnabled = isEnabled;
            removeButton.IsEnabled = isEnabled;
        }

        public string Title
        {
            get
            {
                return title.Text;
            }
            set
            {
                title.Text = value;

                title.Visibility = string.IsNullOrWhiteSpace(value) ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public object SelectedItem
        {
            get
            {
                return listView.SelectedItem;
            }
        }

        public ItemCollection Items
        {
            get
            {
                return listView.Items;
            }
        }

        public DataTemplate ItemTemplate
        {
            get
            {
                return listView.ItemTemplate;
            }
            set
            {
                listView.ItemTemplate = value;
            }
        }

        public event RoutedEventHandler AddButtonClicked
        {
            add
            {
                AddHandler(AddButtonClickedEvent, value);
            }
            remove
            {
                RemoveHandler(AddButtonClickedEvent, value);
            }
        }

        public event RoutedEventHandler ItemsWereRemoved
        {
            add
            {
                AddHandler(ItemsWereRemovedEvent, value);
            }
            remove
            {
                RemoveHandler(ItemsWereRemovedEvent, value);
            }
        }

        private void AddItemClicked(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(AddButtonClickedEvent));
        }

        private void RemoveClicked(object sender, RoutedEventArgs e)
        {
            RemoveSelectedItems();
        }

        private void RemoveSelectedItems()
        {
            IList selectedItems = listView.SelectedItems;

            if (listView.Items.Count == 0)
                return;

            if (selectedItems.Count > 0)
            {
                List<object> items = selectedItems.Cast<object>().ToList();

                items.ForEach(listView.Items.Remove);
            }
            else
            {
                listView.Items.Remove(listView.Items[0]);
            }

            RaiseEvent(new RoutedEventArgs(ItemsWereRemovedEvent));
        }

        private void ScopeListKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
                RemoveSelectedItems();
        }
    }
}
