using System;
using System.Collections.Generic;

namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// Event arguments for the <see cref="ARAnchorManager.anchorsChanged"/> event.
    /// </summary>
    public struct ARAnchorsChangedEventArgs : IEquatable<ARAnchorsChangedEventArgs>
    {
        /// <summary>
        /// The list of <see cref="ARAnchor"/>s added since the last event.
        /// </summary>
        public List<ARAnchor> added { get; private set; }

        /// <summary>
        /// The list of <see cref="ARAnchor"/>s udpated since the last event.
        /// </summary>
        public List<ARAnchor> updated { get; private set; }

        /// <summary>
        /// The list of <see cref="ARAnchor"/>s removed since the last event.
        /// At the time the event is invoked, the <see cref="ARAnchor"/>s in
        /// this list still exist. They are destroyed immediately afterward.
        /// </summary>
        public List<ARAnchor> removed { get; private set; }

        /// <summary>
        /// Constructs an <see cref="ARAnchorsChangedEventArgs"/>.
        /// </summary>
        /// <param name="added">The list of <see cref="ARAnchor"/>s added since the last event.</param>
        /// <param name="updated">The list of <see cref="ARAnchor"/>s updated since the last event.</param>
        /// <param name="removed">The list of <see cref="ARAnchor"/>s removed since the last event.</param>
        public ARAnchorsChangedEventArgs(
            List<ARAnchor> added,
            List<ARAnchor> updated,
            List<ARAnchor> removed)
        {
            this.added = added;
            this.updated = updated;
            this.removed = removed;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 0;
                hash = hash * 486187739 + (added == null ? 0 : added.GetHashCode());
                hash = hash * 486187739 + (updated == null ? 0 : updated.GetHashCode());
                hash = hash * 486187739 + (removed == null ? 0 : removed.GetHashCode());
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ARAnchorsChangedEventArgs))
                return false;

            return Equals((ARAnchorsChangedEventArgs)obj);
        }

        public override string ToString()
        {
            return string.Format("Added: {0}, Updated: {1}, Removed: {2}",
                added == null ? 0 : added.Count,
                updated == null ? 0 : updated.Count,
                removed == null ? 0 : removed.Count);

        }

        public bool Equals(ARAnchorsChangedEventArgs other)
        {
            return
                (added == other.added) &&
                (updated == other.updated) &&
                (removed == other.removed);
        }

        public static bool operator ==(ARAnchorsChangedEventArgs lhs, ARAnchorsChangedEventArgs rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(ARAnchorsChangedEventArgs lhs, ARAnchorsChangedEventArgs rhs)
        {
            return !lhs.Equals(rhs);
        }
    }
}
