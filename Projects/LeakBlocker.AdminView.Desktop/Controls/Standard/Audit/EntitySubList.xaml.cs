using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LeakBlocker.AdminView.Desktop.Controls.Common.Buttons;
using LeakBlocker.AdminView.Desktop.Generated;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Resources;

namespace LeakBlocker.AdminView.Desktop.Controls.Standard.Audit
{
    internal sealed partial class EntitySubList
    {
        private string headerTextFormat;

        private ListEntityType type;

        public event Action AddClicked;
        public event Action ItemRemoved;

        public EntitySubList()
        {
            InitializeComponent();
        }

        public void SetItems(IEnumerable<BaseEntity> newValues)
        {
            items.Items.Clear();

            newValues.ForEach(value => items.Items.Add(value));

            UpdateHeader();
        }

        public void AppendItem(BaseEntity newItem)
        {
            if (items.Items.Contains(newItem))
                return;

            items.Items.Add(newItem);
            UpdateHeader();

            items.SelectedItem = newItem;
            items.Focus();
        }

        public ReadOnlySet<BaseEntity> Items
        {
            get
            {
                return items.Items.Cast<BaseEntity>().ToReadOnlySet();
            }
        }

        public ListEntityType EntityType
        {
            get
            {
                return type;
            }
            set
            {
                type = value;

                switch (type)
                {
                    case ListEntityType.Computer:
                        imageContainer.Content = new ObjectTypeComputer();
                        break;
                    case ListEntityType.User:
                        imageContainer.Content = new ObjectTypeUser();
                        break;
                    case ListEntityType.Device:
                        imageContainer.Content = new ObjectTypeDevice();
                        break;
                }
            }
        }

        public string HeaderTextFormat
        {
            get
            {
                return headerTextFormat;
            }
            set
            {
                headerTextFormat = value;
                UpdateHeader();
            }
        }

        private void UpdateHeader()
        {
            headerText.Text = headerTextFormat.Combine(Convert(items.Items.Count));

            expander.IsExpanded = items.Items.Count > 0;
        }

        private static string Convert(int count)
        {
            if (count == 0)
                return AuditStrings.All;

            return count.ToString(CultureInfo.CurrentCulture);
        }

        private void RemoveButtonClicked(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var target = (BaseEntity)button.Tag;

            items.Items.Remove(target);
            UpdateHeader();

            if (ItemRemoved != null)
                ItemRemoved();
        }

        internal enum ListEntityType
        {
            Computer,
            User,
            Device
        }

        private void OnStretchedHeaderTemplateLoaded(object sender, RoutedEventArgs e)
        {
            //Тут творится форменный костыль, взятый отсюда: http://joshsmithonwpf.wordpress.com/2007/02/24/stretching-content-in-an-expander-header/
            //Желательно потом всё это слегка переделать

            var rootElem = sender as Border;

            if (rootElem == null)
                return;

            var contentPres = rootElem.TemplatedParent as ContentPresenter;

            if (contentPres != null)
                contentPres.HorizontalAlignment = HorizontalAlignment.Stretch;
        }

        private void ItemsKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Delete)
                return;

            RemoveSelectedItems();
        }

        private void RemoveSelectedItems()
        {
            ReadOnlySet<BaseEntity> selected = items.SelectedItems.Cast<BaseEntity>().ToReadOnlySet();
            selected.ForEach(items.Items.Remove);
        }

        private void RemoveSelected(object sender, RoutedEventArgs e)
        {
            RemoveSelectedItems();

            UpdateHeader();
        }

        private void OnExpanded(object sender, RoutedEventArgs e)
        {
            if (items.Items.Count == 0)
                expander.IsExpanded = false;
        }

        private void AddClickedHandler(object sender, RoutedEventArgs e)
        {
            if (AddClicked != null)
                AddClicked();
        }
    }
}
