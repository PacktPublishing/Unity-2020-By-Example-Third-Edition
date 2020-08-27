using System;
using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// Manages anchors.
    /// </summary>
    /// <remarks>
    /// <para>Use this component to programmatically add, remove, or query for
    /// anchors. Anchors are <c>Pose</c>s in the world
    /// which will be periodically updated by an AR device as its understanding
    /// of the world changes.</para>
    /// <para>Subscribe to changes (added, updated, and removed) via the
    /// <see cref="ARAnchorManager.anchorsChanged"/> event.</para>
    /// </remarks>
    /// <seealso cref="ARTrackableManager{TSubsystem, TSubsystemDescriptor, TSessionRelativeData, TTrackable}"/>
    [DefaultExecutionOrder(ARUpdateOrder.k_AnchorManager)]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ARSessionOrigin))]
    [HelpURL("https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@3.0/api/UnityEngine.XR.ARFoundation.ARAnchorManager.html")]
    public sealed class ARAnchorManager : ARTrackableManager<
        XRAnchorSubsystem,
        XRAnchorSubsystemDescriptor,
        XRAnchor,
        ARAnchor>
    {
        [SerializeField]
        [Tooltip("If not null, instantiates this prefab for each instantiated anchor.")]
        [FormerlySerializedAs("m_ReferencePointPrefab")]
        GameObject m_AnchorPrefab;

        /// <summary>
        /// Getter/setter for the Anchor Prefab.
        /// </summary>
        public GameObject anchorPrefab
        {
            get { return m_AnchorPrefab; }
            set { m_AnchorPrefab = value; }
        }

        /// <summary>
        /// Invoked once per frame to communicate changes to anchors, including
        /// new anchors, the update of existing anchors, and the removal
        /// of previously existing anchors.
        /// </summary>
        public event Action<ARAnchorsChangedEventArgs> anchorsChanged;

        /// <summary>
        /// Attempts to add an <see cref="ARAnchor"/> with the given <c>Pose</c>.
        /// </summary>
        /// <remarks>
        /// If <see cref="ARTrackableManager{TSubsystem, TSubsystemDescriptor, TSessionRelativeData, TTrackable}.prefab"/>
        /// is not null, a new instance of that prefab will be instantiated. Otherwise, a
        /// new <c>GameObject</c> will be created. In either case, the resulting
        /// <c>GameObject</c> will have an <see cref="ARAnchor"/> component on it.
        /// </remarks>
        /// <param name="pose">The pose, in Unity world space, of the <see cref="ARAnchor"/>.</param>
        /// <returns>A new <see cref="ARAnchor"/> if successful, otherwise <c>null</c>.</returns>
        public ARAnchor AddAnchor(Pose pose)
        {
            if (!enabled)
                throw new InvalidOperationException("Cannot create a anchor from a disabled anchor manager.");

            if (subsystem == null)
                throw new InvalidOperationException("Anchor manager has no subsystem. Enable the manager first.");

            var sessionRelativePose = sessionOrigin.trackablesParent.InverseTransformPose(pose);

            // Add the anchor to the XRAnchorSubsystem
            XRAnchor sessionRelativeData;
            if (subsystem.TryAddAnchor(sessionRelativePose, out sessionRelativeData))
                return CreateTrackableImmediate(sessionRelativeData);

            return null;
        }

        /// <summary>
        /// Attempts to create a new anchor that is attached to an existing <see cref="ARPlane"/>.
        /// </summary>
        /// <param name="plane">The <see cref="ARPlane"/> to which to attach.</param>
        /// <param name="pose">The initial <c>Pose</c>, in Unity world space, of the anchor.</param>
        /// <returns>A new <see cref="ARAnchor"/> if successful, otherwise <c>null</c>.</returns>
        public ARAnchor AttachAnchor(ARPlane plane, Pose pose)
        {
            if (!enabled)
                throw new InvalidOperationException("Cannot create a anchor from a disabled anchor manager.");

            if (subsystem == null)
                throw new InvalidOperationException("Anchor manager has no subsystem. Enable the manager first.");

            if (plane == null)
                throw new ArgumentNullException("plane");

            var sessionRelativePose = sessionOrigin.trackablesParent.InverseTransformPose(pose);
            XRAnchor sessionRelativeData;
            if (subsystem.TryAttachAnchor(plane.trackableId, sessionRelativePose, out sessionRelativeData))
                return CreateTrackableImmediate(sessionRelativeData);

            return null;
        }

        /// <summary>
        /// Attempts to remove an <see cref="ARAnchor"/>.
        /// </summary>
        /// <param name="anchor">The anchor you wish to remove.</param>
        /// <returns>
        /// <c>True</c> if the anchor was successfully removed.
        /// <c>False</c> usually means the anchor is not longer tracked by the system.
        /// </returns>
        public bool RemoveAnchor(ARAnchor anchor)
        {
            if (!enabled)
                throw new InvalidOperationException("Cannot create a anchor from a disabled anchor manager.");

            if (subsystem == null)
                throw new InvalidOperationException("Anchor manager has no subsystem. Enable the manager first.");

            if (anchor == null)
                throw new ArgumentNullException("anchor");

            if (subsystem.TryRemoveAnchor(anchor.trackableId))
            {
                DestroyPendingTrackable(anchor.trackableId);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the <see cref="ARAnchor"/> with given <paramref name="trackableId"/>,
        /// or <c>null</c> if it does not exist.
        /// </summary>
        /// <param name="trackableId">The <see cref="TrackableId"/> of the <see cref="ARAnchor"/> to retrieve.</param>
        /// <returns>The <see cref="ARAnchor"/> with <paramref name="trackableId"/> or <c>null</c> if it does not exist.</returns>
        public ARAnchor GetAnchor(TrackableId trackableId)
        {
            ARAnchor anchor;
            if (m_Trackables.TryGetValue(trackableId, out anchor))
                return anchor;

            return null;
        }

        protected override GameObject GetPrefab()
        {
            return m_AnchorPrefab;
        }

        protected override string gameObjectName
        {
            get { return "Anchor"; }
        }

        protected override void OnTrackablesChanged(
            List<ARAnchor> addedPoints,
            List<ARAnchor> updatedPoints,
            List<ARAnchor> removedPoints)
        {
            if (anchorsChanged != null)
            {
                anchorsChanged(
                    new ARAnchorsChangedEventArgs(
                        addedPoints,
                        updatedPoints,
                        removedPoints));
            }
        }
    }
}
