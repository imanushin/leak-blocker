using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;

namespace LeakBlocker.AdminView.Desktop.Controls.Common.AutoComplete
{
    internal sealed partial class HighlightedAutoCompleteBox
    {
        private const int maxCountToShow = 16;

        private Action unloadAction;

        public event Action TextChanged;

        private readonly Dictionary<object, HintControlContainer> controlDictionary = new Dictionary<object, HintControlContainer>();

        private readonly Dictionary<string, ReadOnlySet<HintControlContainer>> itemsToShow = new Dictionary<string, ReadOnlySet<HintControlContainer>>();

        private Func<object, FrameworkElement> imageGetter = obj => null;
        private Func<object, string> hintGetter = obj => null;

        private Action unsubscribeFromItems;

        public HighlightedAutoCompleteBox()
        {
            InitializeComponent();
            selector.ItemFilter = Filter;
        }

        private void InnerCollectionUpdated(object sender, EventArgs args)
        {
            SetItems((IEnumerable)sender);
        }

        public void SetItems(IEnumerable newItems)
        {
            SubscribeToChanges(newItems);

            var oldDictionary = new Dictionary<object, HintControlContainer>(controlDictionary);

            controlDictionary.Clear();

            foreach (object newItem in newItems)
            {
                HintControlContainer control = oldDictionary.TryGetValue(newItem);

                if (control == null)
                {
                    string hint = hintGetter(newItem);

                    object capturedItem = newItem;

                    control = new HintControlContainer(() => imageGetter(capturedItem), newItem.ToString());

                    object item = newItem;

                    control.MouseUp += (sender, args) =>
                        {
                            selector.Text = item.ToString();
                        };

                    if (!string.IsNullOrWhiteSpace(hint))
                        control.ToolTip = hint;
                }

                controlDictionary.Add(newItem, control);
            }

            selector.ItemsSource = newItems.Cast<object>().Select(item => controlDictionary[item]);
            itemsToShow.Clear();
        }

        private void SubscribeToChanges(IEnumerable newItems)
        {
            if (unsubscribeFromItems != null)
                unsubscribeFromItems();

            unsubscribeFromItems = null;

            var notifyCollection = newItems as INotifyCollectionChanged;

            if (notifyCollection != null)
            {
                notifyCollection.CollectionChanged += InnerCollectionUpdated;

                unsubscribeFromItems = () => notifyCollection.CollectionChanged -= InnerCollectionUpdated;
            }
        }

        public string Text
        {
            get
            {
                return selector.Text;
            }
            set
            {
                selector.Text = value;
            }
        }

        public void SetObjectToImageTemplateConverter(Func<object, FrameworkElement> converter)
        {
            imageGetter = converter;
            itemsToShow.Clear();
        }

        public void SetObjectToHintConverter(Func<object, string> converter)
        {
            hintGetter = converter;
            itemsToShow.Clear();
        }

        private bool Filter(string search, object value)
        {
            UpdateItemsToShow(search);

            return itemsToShow[search].Contains(value);
        }

        public new void Focus()
        {
            selector.Focus();
        }

        private void UpdateItemsToShow(string pattern)
        {
            if (!itemsToShow.ContainsKey(pattern))
                itemsToShow[pattern] = GetSearchItems(pattern);
        }

        private ReadOnlySet<HintControlContainer> GetSearchItems(string search)
        {
            List<Tuple<HintControlContainer, string>> items =
                controlDictionary.
                Values.
                Select(targetObject => new Tuple<HintControlContainer, string>(targetObject, targetObject.ToString().ToUpperInvariant())).ToList();

            items.Sort((left, right) => string.CompareOrdinal(left.Item2, right.Item2));

            var upperInvariant = search.ToUpperInvariant();

            return items.Where(inputObject =>
                inputObject.Item2.Contains(upperInvariant))
                .Take(maxCountToShow)
                .Select(item => item.Item1)
                .ToReadOnlySet();
        }

        private void ControlLoaded(object sender, RoutedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            Window window = Window.GetWindow(this);

            Check.ObjectIsNotNull(window);

            window.LocationChanged += WindowLocationChanged;

            unloadAction = () => window.LocationChanged -= WindowLocationChanged;//Избавляемся от лишних ссылок на себя
        }

        private void WindowLocationChanged(object sender, EventArgs e)
        {
            selector.IsDropDownOpen = false;//Так как иначе выплывающий список остается на старой позиции
        }

        private void ControlUnloaded(object sender, RoutedEventArgs e)
        {
            if (unloadAction != null)
                unloadAction();

            if (unsubscribeFromItems != null)
                unsubscribeFromItems();

            unsubscribeFromItems = null;
        }

        private void TextChangedHandler(object sender, RoutedEventArgs e)
        {
            if (TextChanged != null)
                TextChanged();

            var pattern = selector.Text.ToUpperInvariant();

            UpdateItemsToShow(pattern);

            if (selector.SelectedItem == null)//Хак, чтобы при переключении между элементами не происходила их подсветка
                itemsToShow[pattern].ForEach(control => control.HighlightText(pattern));
        }

        private void GotFocusHandler(object sender, RoutedEventArgs e)
        {//Еще один костыль для AutoCompleteBox (http://wpf.codeplex.com/workitem/13476) - уже третий по счету
            var autoBox = sender as AutoCompleteBox;
            if (autoBox == null)
                return;

            var textBox = autoBox.Template.FindName("Text", autoBox) as TextBox;
            if (textBox != null)
                textBox.Focus();
        }
    }
}
