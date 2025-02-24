﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandApi.Classes
{
    public class QueryParameters
    {
        const int _maxSize = 100;
        private int _size = 50;

        public int Page { get; set; }
        public int Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = Math.Min(_maxSize, value);
            }
        }
        public string SortBy { get; set; } = "Id";

        private string _sortOrder = "asc";
        public string SortOrder
        {
            get
            {
                return _sortOrder;
            }
            set
            {
                if (value == "asc" || value == "desc")
                {
                    _sortOrder = value;
                }
            }
        }
        public string HowTo { get; set; }
    }
}
