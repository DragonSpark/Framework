﻿// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System.Windows.Controls;

namespace DragonSpark.Application.Client.Controls.LongListSelector
{
    /// <summary>
    /// Represents an item within a <see cref="TemplatedListBox"/>
    /// </summary>
    /// <QualityBand>Preview</QualityBand>
    internal class TemplatedListBoxItem : ListBoxItem
    {
        public LongListSelectorItem Tuple { get; set; }
    }
}
