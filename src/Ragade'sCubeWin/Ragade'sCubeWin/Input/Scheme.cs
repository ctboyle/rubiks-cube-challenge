using System;
using System.Collections.Generic;
using System.Text;
using RagadesCubeWin.Input.Events;
using RagadesCubeWin.Input.Types;

namespace RagadesCubeWin.Input
{
    /// <summary>
    /// Defines an input scheme for a class that can be used for 
    /// mapping input (via input watchers) with an input manager. 
    /// The inputs are initialized with the input manager while
    /// using the <see cref="Apply"/> method.  The inputs are stopped
    /// from being watched by using the <seealso cref="Unapply"/> method.
    /// </summary>
    /// <typeparam name="CntrlType">
    /// The class that provides the methods that are to be mapped to.
    /// </typeparam>
    public abstract class RCInputScheme<CntrlType> where CntrlType : class
    {
        private InputManager _inputMgr;
        private List<IWatcher> _watchList;
        private CntrlType _cntrlItem;

        /// <summary>
        /// Creates a new instance of the <see cref="RCInputScheme"/> class.
        /// </summary>
        public RCInputScheme()
        {
            _inputMgr = null;
            _cntrlItem = null;
            _watchList = new List<IWatcher>();
        }

        /// <summary>
        /// Applies the input mapping to the input manager for the scheme. 
        /// </summary>
        /// <param name="cntrlItem">
        /// The instance of <see cref="CntrlType"/> that provides the mapping functions.
        /// </param>
        /// <param name="im">The input manager.</param>
        /// <exception cref="NullReferenceException">If the <see cref="im"/> variable is null.</exception>
        /// <exception cref="Exception">If the scheme has already been applied.</exception>
        /// <exception cref="NullReferenceException">The <see cref="cntrlItem"/> variable cannot be null.</exception>
        public void Apply(InputManager im, CntrlType cntrlItem)
        {
            if (_cntrlItem != null || _inputMgr != null || _watchList.Count != 0)
                throw new Exception("Unable to apply because this instance is already in use.");

            if (im == null)
                throw new NullReferenceException("The input manager cannot be null.");

            if (cntrlItem == null)
                throw new NullReferenceException("The mapping provider cannot be null.");

            _inputMgr = im;
            _cntrlItem = cntrlItem;

            IWatcher[] mappedWatchers = MapWatcherEvents();

            if (mappedWatchers == null || mappedWatchers.Length == 0)
                return;

            _watchList.AddRange(mappedWatchers);

            foreach (IWatcher watcher in _watchList)
                _inputMgr.AddWatcher(watcher);
        }

        /// <summary>
        /// Unapplies the current mappings to the input manager.
        /// </summary>
        public void Unapply()
        {
            foreach (IWatcher watcher in _watchList)
                _inputMgr.RemoveWatcher(watcher);

            _inputMgr = null;
            _cntrlItem = null;

            _watchList.Clear();
        }

        /// <summary>
        /// The instance of <see cref="CntrlType"/> that provides the mapping functions.
        /// </summary>
        protected CntrlType ControlItem
        {
            get { return _cntrlItem; }
        }

        /// <summary>
        /// Maps the inputs to the watchers and provides the watchers
        /// for local storage (so they can be unapplied).
        /// </summary>
        /// <returns>The mapped input watchers.</returns>
        protected abstract IWatcher[] MapWatcherEvents();
    }
}