﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyTable.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 OxyPlot contributors
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Represents a table of auto generated property values.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Reporting
{
    using System;
    using System.Collections;
    using System.Linq;

    /// <summary>
    /// Represents a table of auto generated property values.
    /// </summary>
    /// <remarks>The PropertyTable auto generates columns or rows based on reflecting the Items type.</remarks>
    public class PropertyTable : ItemsTable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyTable" /> class.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="itemsInRows">The items in rows.</param>
        public PropertyTable(IEnumerable items, bool itemsInRows)
            : base(itemsInRows)
        {
            this.Alignment = Alignment.Left;
            var input = items.Cast<object>().ToArray();
            this.UpdateFields(input);
            this.Items = input;
        }

        /// <summary>
        /// Gets the item type.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns>The type of the items.</returns>
        private Type GetItemType(IEnumerable items)
        {
            Type result = null;
            foreach (var item in items)
            {
                var t = item.GetType();
                if (result == null)
                {
                    result = t;
                }

                if (t != result)
                {
                    return null;
                }
            }

            return result;
        }

        /// <summary>
        /// Updates the fields.
        /// </summary>
        /// <param name="items">The items.</param>
        private void UpdateFields(IEnumerable items)
        {
            Type type = this.GetItemType(items);
            if (type == null)
            {
                return;
            }

            this.Columns.Clear();

            foreach (var pi in type.GetProperties())
            {
                // TODO: support Browsable and Displayname attributes
                var header = pi.Name;
                this.Fields.Add(new ItemsTableField(header, pi.Name, null, Alignment.Left));
            }
        }
    }
}