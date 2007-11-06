using System;
using System.Collections.Generic;
using System.Text;
using RC.Engine.GraphicsManagement;

namespace RC.Engine.Picking
{
    struct PickRecordEntry : IComparable<PickRecordEntry>
    {
        public PickRecordEntry(float intsect, ISpatial pickedObj)
        {
            intersection = intsect;
            picked = pickedObj;
        }

        public float intersection;
        public ISpatial picked;

        #region IComparable<PickRecordEntry> Members

        public int CompareTo(PickRecordEntry other)
        {
            return Math.Sign(intersection - other.intersection);
        }

        #endregion
    }

    class RCPickRecord
    {
        public enum SortModeType
        {
            ClosestFisrt,
            FurthestFisrt
        }

        public enum FilterMode
        {
            KeepCompatible,
            RemoveCompatiable
        }

        List<PickRecordEntry> _picked;
        Dictionary<Type, FilterMode> _filters;
        List<ISpatial> _filteredItems;
        bool _sorted;
        bool _filtered;

        SortModeType _sortMode;


        public SortModeType SortMode
        {
            get { return _sortMode; }
            set
            {
                _sortMode = value;
                _sorted = false;
            }
        }

        public int HitCount
        {
            get {return _picked.Count; }
        }

        public int FilteredCount
        {
            get 
            {
                CheckFlags();
                return _filteredItems.Count; 
            }
        }

        public RCPickRecord()
        {
            _picked = new List<PickRecordEntry>();
            _filteredItems = new List<ISpatial>();

            _filters = new Dictionary<Type, FilterMode>();

            _sorted = false;
            _filtered = false;

            _sortMode = SortModeType.ClosestFisrt;

        }

        public void AddPicked(
            ISpatial spatialObj,
            float intersection
            )
        {
            _picked.Add(
                new PickRecordEntry(
                    intersection,
                    spatialObj
                    )
                );

            _sorted = false;
        }

        public bool AddFilter(Type type, FilterMode mode)
        {
            bool addedFilter = false;

            if (!_filters.ContainsKey(type))
            {
                _filters.Add(
                    type,
                    mode
                    );

                addedFilter = true;
                _filtered = false;
            }


            return addedFilter;
        }

        public bool RemoveFilter(Type type)
        {
            bool removed = _filters.Remove(type);
            if (removed)
            {
                _filtered = false;
            }
            return removed;
        }


        public List<ISpatial> GetPicked()
        {
            CheckFlags();

            return _filteredItems;
        }

        public ISpatial GetFirst()
        {
            CheckFlags();

            return _filteredItems[0] as ISpatial;
        }

        public ISpatial GetLast()
        {
            CheckFlags();

            return _filteredItems[_filteredItems.Count - 1] as ISpatial;
        }

        private void CheckFlags()
        {
            if (!_sorted)
            {
                Sort();
            }

            if (!_filtered)
            {
                Filter();
            }
        }



        private void Filter()
        {
            _filteredItems.Clear();

            // Get picked items.
            foreach (PickRecordEntry entry in _picked)
            {
                _filteredItems.Add(entry.picked);
            }

            // Apply filters to each object
            foreach (KeyValuePair<Type, FilterMode> kv in _filters)
            {
                
                for (int iObj = 0; iObj < _filteredItems.Count; iObj++)
                {
                    // apply filter.
                     switch (kv.Value)
                    {
                        case FilterMode.KeepCompatible:
                             // Remove if not compatible
                            if (!kv.Key.IsInstanceOfType(_filteredItems[iObj]))
                            {
                                // Remove item and keep index the same.
                                _filteredItems.RemoveAt(iObj--);
                            }
                            // Let the item remain in the list
                            break;
                        case FilterMode.RemoveCompatiable:
                             // remove if compatible
                            if (kv.Key.IsInstanceOfType(_filteredItems[iObj]))
                            {
                                // Remove item and keep index the same.
                                _filteredItems.RemoveAt(iObj--);
                            }                            
                            break;
                    }
                }
            }

            _filtered = true;
        }

        private void Sort()
        {
            _picked.Sort();
            if (_sortMode == SortModeType.FurthestFisrt)
            {
                _picked.Reverse();
            }
            _sorted = true;
        }

    }
}
