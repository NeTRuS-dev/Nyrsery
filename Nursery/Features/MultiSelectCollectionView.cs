﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace Nursery.Features
{
    public class MultiSelectCollectionView<T> : ListCollectionView, IMultiSelectCollectionView
    {
        public MultiSelectCollectionView(IList list)
            : base(list)
        {
            SelectedItems = new ObservableCollection<T>();
        }

        void IMultiSelectCollectionView.AddControl(Selector selector)
        {
            this.controls.Add(selector);
            SetSelection(selector);
            selector.SelectionChanged += control_SelectionChanged;
        }

        void IMultiSelectCollectionView.RemoveControl(Selector selector)
        {
            if (this.controls.Remove(selector))
            {
                selector.SelectionChanged -= control_SelectionChanged;
            }
        }

        public ObservableCollection<T> SelectedItems { get; private set; }

        void SetSelection(Selector selector)
        {
            MultiSelector multiSelector = selector as MultiSelector;
            ListBox listBox = selector as ListBox;

            if (multiSelector != null)
            {
                multiSelector.SelectedItems.Clear();

                foreach (T item in SelectedItems)
                {
                    multiSelector.SelectedItems.Add(item);
                }
            }
            else if (listBox != null)
            {
                listBox.SelectedItems.Clear();

                foreach (T item in SelectedItems)
                {
                    listBox.SelectedItems.Add(item);
                }
            }
        }

        void control_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!this.ignoreSelectionChanged)
            {
                bool changed = false;

                this.ignoreSelectionChanged = true;

                try
                {
                    foreach (T item in e.AddedItems)
                    {
                        if (!SelectedItems.Contains(item))
                        {
                            SelectedItems.Add(item);
                            changed = true;
                        }
                    }

                    foreach (T item in e.RemovedItems)
                    {
                        if (SelectedItems.Remove(item))
                        {
                            changed = true;
                        }
                    }

                    if (changed)
                    {
                        foreach (Selector control in this.controls)
                        {
                            if (control != sender)
                            {
                                SetSelection(control);
                            }
                        }
                    }
                }
                finally
                {
                    this.ignoreSelectionChanged = false;
                }
            }
        }

        bool ignoreSelectionChanged;
        List<Selector> controls = new List<Selector>();
    }
}
