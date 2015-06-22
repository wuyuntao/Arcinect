using Microsoft.Kinect.Fusion;
using NLog;
using System;

namespace Arcinect
{
    class VolumeBuilder
    {
        /// <summary>
        /// Logger of current class
        /// </summary>
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Data source
        /// </summary>
        private Scanner source;

        /// <summary>
        /// The configuration to build volume 
        /// </summary>
        private VolumeBuilderPreferences preferences;

        /// <summary>
        /// The Kinect Fusion volume, enabling color reconstruction
        /// </summary
        private ColorReconstruction volume;

        /// <summary>
        /// The transformation between the world and camera view coordinate system
        /// </summary>
        private Matrix4 worldToCameraTransform;

        /// <summary>
        /// The default transformation between the world and volume coordinate system
        /// </summary>
        private Matrix4 defaultWorldToVolumeTransform;

        /// <summary>
        /// Parameter to translate the reconstruction based on the minimum depth setting. When set to
        /// false, the reconstruction volume +Z axis starts at the camera lens and extends into the scene.
        /// Setting this true in the constructor will move the volume forward along +Z away from the
        /// camera by the minimum depth threshold to enable capture of very small reconstruction volume
        /// by setting a non-identity world-volume transformation in the ResetReconstruction call.
        /// Small volumes should be shifted, as the Kinect hardware has a minimum sensing limit of ~0.35m,
        /// inside which no valid depth is returned, hence it is difficult to initialize and track robustly  
        /// when the majority of a small volume is inside this distance.
        /// </summary>
        private bool translateResetPoseByMinDepthThreshold = true;

        /// <summary>
        /// The counter for image process failures
        /// </summary>
        private int trackingErrorCount = 0;

        public VolumeBuilder(Scanner source, VolumeBuilderPreferences parameters)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            this.source = source;
            this.preferences = parameters;

            // Set the world-view transform to identity, so the world origin is the initial camera location.
            this.worldToCameraTransform = Matrix4.Identity;

            var volumeParameters = new ReconstructionParameters(parameters.VoxelsPerMeter, parameters.VoxelsX, parameters.VoxelsY, parameters.VoxelsZ);
            this.volume = ColorReconstruction.FusionCreateReconstruction(volumeParameters, ReconstructionProcessor.Amp, -1, this.worldToCameraTransform);

            this.defaultWorldToVolumeTransform = this.volume.GetCurrentWorldToVolumeTransform();
        }

        public VolumeBuilder(Scanner source)
            : this(source, new VolumeBuilderPreferences())
        { }

        /// <summary>
        /// Reset reconstruction object to initial state
        /// </summary>
        private void ResetReconstruction()
        {
            logger.Trace("Start reset reconstruction");

            // Reset tracking error counter
            this.trackingErrorCount = 0;

            // Set the world-view transform to identity, so the world origin is the initial camera location.
            this.worldToCameraTransform = Matrix4.Identity;

            // Reset volume
            try
            {
                // Translate the reconstruction volume location away from the world origin by an amount equal
                // to the minimum depth threshold. This ensures that some depth signal falls inside the volume.
                // If set false, the default world origin is set to the center of the front face of the 
                // volume, which has the effect of locating the volume directly in front of the initial camera
                // position with the +Z axis into the volume along the initial camera direction of view.
                if (this.translateResetPoseByMinDepthThreshold)
                {
                    var worldToVolumeTransform = this.defaultWorldToVolumeTransform;

                    // Translate the volume in the Z axis by the minDepthClip distance
                    float minDistance = Math.Min(this.preferences.MinDepthClip, this.preferences.MaxDepthClip);
                    worldToVolumeTransform.M43 -= minDistance * this.preferences.VoxelsPerMeter;

                    this.volume.ResetReconstruction(this.worldToCameraTransform, worldToVolumeTransform);
                }
                else
                {
                    this.volume.ResetReconstruction(this.worldToCameraTransform);
                }

                //ResetTracking();
                //ResetColorImage();
            }
            catch (InvalidOperationException error)
            {
                logger.Error("Failed to reset reconstruction. Error: {0}", error);
            }

            logger.Trace("Finish reset reconstruction");
        }
    }
}
