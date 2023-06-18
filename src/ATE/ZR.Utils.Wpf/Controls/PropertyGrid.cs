using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace ZR.Utils.Wpf.Controls
{
    [TemplatePart(Name = "PART_ItemsControl", Type = typeof(ItemsControl))]
    public class PropertyGrid : Control
    {
        private const string ElementItemsControl = "PART_ItemsControl";

        private const string ElementSearchBar = "PART_SearchBar";

        private ItemsControl _itemsControl;

        private ICollectionView _dataView;

        private string _searchKey;

        public static readonly RoutedEvent SelectedObjectChangedEvent = EventManager.RegisterRoutedEvent("SelectedObjectChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<object>), typeof(PropertyGrid));

        public static readonly DependencyProperty SelectedObjectProperty = DependencyProperty.Register("SelectedObject", typeof(object), typeof(PropertyGrid), new PropertyMetadata(null, OnSelectedObjectChanged));

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(string), typeof(PropertyGrid), new PropertyMetadata((object)null));

        public static readonly DependencyProperty MaxTitleWidthProperty = DependencyProperty.Register("MaxTitleWidth", typeof(double), typeof(PropertyGrid), new PropertyMetadata(ValueBoxes.Double0Box));

        public static readonly DependencyProperty MinTitleWidthProperty = DependencyProperty.Register("MinTitleWidth", typeof(double), typeof(PropertyGrid), new PropertyMetadata(ValueBoxes.Double0Box));

        public static readonly DependencyProperty ShowSortButtonProperty = DependencyProperty.Register("ShowSortButton", typeof(bool), typeof(PropertyGrid), new PropertyMetadata(ValueBoxes.TrueBox));

        public virtual PropertyResolver PropertyResolver { get; } = new PropertyResolver();


        public object SelectedObject
        {
            get
            {
                return GetValue(SelectedObjectProperty);
            }
            set
            {
                SetValue(SelectedObjectProperty, value);
            }
        }

        public string Description
        {
            get
            {
                return (string)GetValue(DescriptionProperty);
            }
            set
            {
                SetValue(DescriptionProperty, value);
            }
        }

        public double MaxTitleWidth
        {
            get
            {
                return (double)GetValue(MaxTitleWidthProperty);
            }
            set
            {
                SetValue(MaxTitleWidthProperty, value);
            }
        }

        public double MinTitleWidth
        {
            get
            {
                return (double)GetValue(MinTitleWidthProperty);
            }
            set
            {
                SetValue(MinTitleWidthProperty, value);
            }
        }

        public bool ShowSortButton
        {
            get
            {
                return (bool)GetValue(ShowSortButtonProperty);
            }
            set
            {
                SetValue(ShowSortButtonProperty, ValueBoxes.BooleanBox(value));
            }
        }

        public event RoutedPropertyChangedEventHandler<object> SelectedObjectChanged
        {
            add
            {
                AddHandler(SelectedObjectChangedEvent, value);
            }
            remove
            {
                RemoveHandler(SelectedObjectChangedEvent, value);
            }
        }

        public PropertyGrid()
        {
            base.CommandBindings.Add(new CommandBinding(ControlCommands.SortByCategory, SortByCategory, delegate (object s, CanExecuteRoutedEventArgs e)
            {
                e.CanExecute = ShowSortButton;
            }));
            base.CommandBindings.Add(new CommandBinding(ControlCommands.SortByName, SortByName, delegate (object s, CanExecuteRoutedEventArgs e)
            {
                e.CanExecute = ShowSortButton;
            }));
        }

        private static void OnSelectedObjectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PropertyGrid)d).OnSelectedObjectChanged(e.OldValue, e.NewValue);
        }

        protected virtual void OnSelectedObjectChanged(object oldValue, object newValue)
        {
            UpdateItems(newValue);
            RaiseEvent(new RoutedPropertyChangedEventArgs<object>(oldValue, newValue, SelectedObjectChangedEvent));
        }

        public override void OnApplyTemplate()
        {
            if (_searchBar != null)
            {
                _searchBar.SearchStarted -= SearchBar_SearchStarted;
            }

            base.OnApplyTemplate();
            _itemsControl = GetTemplateChild("PART_ItemsControl") as ItemsControl;
            _searchBar = GetTemplateChild("PART_SearchBar") as SearchBar;
            if (_searchBar != null)
            {
                _searchBar.SearchStarted += SearchBar_SearchStarted;
            }

            UpdateItems(SelectedObject);
        }

        private void UpdateItems(object obj)
        {
            if (obj != null && _itemsControl != null)
            {
                _dataView = CollectionViewSource.GetDefaultView((from item in TypeDescriptor.GetProperties(obj.GetType()).OfType<PropertyDescriptor>()
                                                                 where PropertyResolver.ResolveIsBrowsable(item)
                                                                 select item).Select(new Func<PropertyDescriptor, PropertyItem>(CreatePropertyItem)).Do(delegate (PropertyItem item)
                                                                 {
                                                                     item.InitElement();
                                                                 }));
                SortByCategory(null, null);
                _itemsControl.ItemsSource = _dataView;
            }
        }

        private void SortByCategory(object sender, ExecutedRoutedEventArgs e)
        {
            if (_dataView != null)
            {
                using (_dataView.DeferRefresh())
                {
                    _dataView.GroupDescriptions.Clear();
                    _dataView.SortDescriptions.Clear();
                    _dataView.SortDescriptions.Add(new SortDescription(PropertyItem.CategoryProperty.Name, ListSortDirection.Ascending));
                    _dataView.SortDescriptions.Add(new SortDescription(PropertyItem.DisplayNameProperty.Name, ListSortDirection.Ascending));
                    _dataView.GroupDescriptions.Add(new PropertyGroupDescription(PropertyItem.CategoryProperty.Name));
                }
            }
        }

        private void SortByName(object sender, ExecutedRoutedEventArgs e)
        {
            if (_dataView != null)
            {
                using (_dataView.DeferRefresh())
                {
                    _dataView.GroupDescriptions.Clear();
                    _dataView.SortDescriptions.Clear();
                    _dataView.SortDescriptions.Add(new SortDescription(PropertyItem.PropertyNameProperty.Name, ListSortDirection.Ascending));
                }
            }
        }

        private void SearchBar_SearchStarted(object sender, FunctionEventArgs<string> e)
        {
            if (_dataView == null)
            {
                return;
            }

            _searchKey = e.Info;
            if (string.IsNullOrEmpty(_searchKey))
            {
                foreach (UIElement item in _dataView)
                {
                    item.Show();
                }

                return;
            }

            foreach (PropertyItem item2 in _dataView)
            {
                item2.Show(item2.PropertyName.ToLower().Contains(_searchKey) || item2.DisplayName.ToLower().Contains(_searchKey));
            }
        }

        protected virtual PropertyItem CreatePropertyItem(PropertyDescriptor propertyDescriptor)
        {
            return new PropertyItem
            {
                Category = PropertyResolver.ResolveCategory(propertyDescriptor),
                DisplayName = PropertyResolver.ResolveDisplayName(propertyDescriptor),
                Description = PropertyResolver.ResolveDescription(propertyDescriptor),
                IsReadOnly = PropertyResolver.ResolveIsReadOnly(propertyDescriptor),
                DefaultValue = PropertyResolver.ResolveDefaultValue(propertyDescriptor),
                Editor = PropertyResolver.ResolveEditor(propertyDescriptor),
                Value = SelectedObject,
                PropertyName = propertyDescriptor.Name,
                PropertyType = propertyDescriptor.PropertyType,
                PropertyTypeName = propertyDescriptor.PropertyType.Namespace + "." + propertyDescriptor.PropertyType.Name
            };
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            TitleElement.SetTitleWidth(this, new GridLength(Math.Max(MinTitleWidth, Math.Min(MaxTitleWidth, base.ActualWidth / 3.0))));
        }
    }
}
