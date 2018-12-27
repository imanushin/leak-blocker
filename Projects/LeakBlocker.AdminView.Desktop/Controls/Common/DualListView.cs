using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LeakBlocker.AdminView.Desktop.Controls.Common.Animations;
using LeakBlocker.AdminView.Desktop.Generated;
using LeakBlocker.AdminView.Desktop.Resources;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Resources;
using EnumerableExtensions = LeakBlocker.Libraries.Common.Collections.EnumerableExtensions;

namespace LeakBlocker.AdminView.Desktop.Controls.Common
{
    internal sealed class DualListView : ContentControl
    {
        private readonly Grid topGrid = new Grid();
        private readonly ListView mainItems = new ListView();
        private readonly ListView sourceItems = new ListView();
        private readonly GridSplitter gridSplitter = new GridSplitter();
        private readonly ContentPresenter contentPresenter = new ContentPresenter();
        private readonly BusyIndicator busyIndicator = new BusyIndicator();

        #region Initialization

        public DualListView()
        {
            InitializeControls();
        }

        private void InitializeControls()
        {
            InitializeGrid();

            Grid.SetRowSpan(mainItems, 2);

            Grid.SetRowSpan(gridSplitter, 2);
            Grid.SetColumn(gridSplitter, 1);
            gridSplitter.Width = 3;
            gridSplitter.VerticalAlignment = VerticalAlignment.Stretch;
            gridSplitter.HorizontalAlignment = HorizontalAlignment.Center;

            Grid.SetRow(sourceItems, 1);
            Grid.SetColumn(sourceItems, 2);

            sourceItems.VerticalContentAlignment = VerticalAlignment.Stretch;
            HorizontalContentAlignment = HorizontalAlignment.Stretch;

            Grid.SetColumn(contentPresenter, 2);

            Grid.SetColumn(busyIndicator, 2);
            Grid.SetRowSpan(busyIndicator, 2);

            topGrid.Children.Add(mainItems);
            topGrid.Children.Add(sourceItems);
            topGrid.Children.Add(gridSplitter);
            topGrid.Children.Add(contentPresenter);
            topGrid.Children.Add(busyIndicator);
        }

        private void InitializeGrid()
        {
            topGrid.ColumnDefinitions.Add(new ColumnDefinition());
            topGrid.ColumnDefinitions.Add(new ColumnDefinition()
                                              {
                                                  Width = GridLength.Auto
                                              });
            topGrid.ColumnDefinitions.Add(new ColumnDefinition());

            topGrid.RowDefinitions.Add(new RowDefinition()
                                           {
                                               Height = GridLength.Auto
                                           });
            topGrid.RowDefinitions.Add(new RowDefinition());
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            object oldContent = Content;

            Content = topGrid;

            contentPresenter.Content = oldContent;
        }

        #endregion Initialization

        public bool IsRightPanelIsBusy
        {
            get
            {
                return busyIndicator.IsBusy;
            }
            set
            {
                busyIndicator.IsBusy = value;
            }
        }

        public IEnumerable CurrentItems
        {
            get
            {
                return mainItems.Items.Cast<ListViewItem>().Select(item => item.Tag);
            }
            set
            {
                SetMainItems(value);
            }
        }

        private void SetMainItems(IEnumerable items)
        {
            mainItems.Items.Clear();

            foreach (var item in items)
            {
                AddItemToMain(item);
            }

            HignlightSourseItems();
        }

        public void SetSourceItems(IEnumerable items)
        {
            sourceItems.Items.Clear();

            foreach (var item in items)
            {
                AddItemToSource(item);
            }

            HignlightSourseItems();
        }

        private bool MainContainsItem(object item)
        {
            return mainItems.Items.Cast<ListViewItem>().Any(listItem => Equals(item, listItem.Tag));
        }

        private void AddItemToSource(object item)
        {
            var newItem = new ListViewItem();

            newItem.Content = item;

            newItem.ContextMenu = CreateSourceContextMenuItem();
            newItem.KeyUp += SourceListItemKeyUp;
            newItem.MouseDoubleClick += SourceItemClick;
            newItem.Tag = item;

            sourceItems.Items.Add(newItem);
        }

        private void AddItemToMain(object baseItem)
        {
            if (MainContainsItem(baseItem))
                return;

            var newItem = new ListViewItem();

            newItem.Content = baseItem;
            newItem.KeyUp += MainItemKeyUp;
            newItem.ContextMenu = CreateMainContextMenuItem();
            newItem.Tag = baseItem;

            mainItems.Items.Add(newItem);
        }

        private void AddSelectedSourceItemsToMain()
        {
            IList selectedItems = sourceItems.SelectedItems;

            foreach (ListViewItem item in selectedItems)
            {
                AddItemToMain(item.Tag);
            }

            HignlightSourseItems();
        }

        private void MainItemKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
                RemoveItemsFromMain();
        }

        private void RemoveItemsFromMain()
        {
            var itemsToRemove = mainItems.SelectedItems.Cast<ListViewItem>().ToList();

            itemsToRemove.ForEach(item => mainItems.Items.Remove(item));

            HignlightSourseItems();
        }

        private void SourceListItemKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                AddSelectedSourceItemsToMain();
        }

        private ContextMenu CreateSourceContextMenuItem()
        {
            var menuItem = new MenuItem();

            menuItem.Header = AdminViewResources.ExcludeFromBlocking;
            menuItem.Icon = new ContextMenuLeftArrow();

            menuItem.Click += SourceItemClick;

            var result = new ContextMenu();

            result.Items.Add(menuItem);

            return result;
        }

        private ContextMenu CreateMainContextMenuItem()
        {
            var menuItem = new MenuItem();

            menuItem.Header = AdminViewResources.ContinueBlocking;
            menuItem.Icon = new ButtonRemove();

            menuItem.Click += MainItemMenuClick;

            var result = new ContextMenu();

            result.Items.Add(menuItem);

            return result;
        }

        private void MainItemMenuClick(object sender, RoutedEventArgs e)
        {
            RemoveItemsFromMain();
        }

        private void SourceItemClick(object sender, RoutedEventArgs e)
        {
            AddSelectedSourceItemsToMain();
        }

        private void HignlightSourseItems()
        {
            IEnumerable<ListViewItem> listItems = sourceItems.Items.Cast<ListViewItem>();

            foreach (ListViewItem listItem in listItems)
            {
                object sourceItem = listItem.Tag;
                
                bool existsInMain = MainContainsItem(sourceItem);

                listItem.BorderThickness = new Thickness(1);

                if (existsInMain)
                {
                    listItem.FontStyle = FontStyles.Italic;
                }
                else
                {
                    listItem.FontStyle = FontStyles.Normal;
                }

                var menuItem = (MenuItem)listItem.ContextMenu.Items[0];

                menuItem.IsEnabled = !existsInMain;

            }
        }
    }
}
