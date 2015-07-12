using Microsoft.Kinect.Fusion;
using System;

namespace Arcinect
{
    /*
    class VolumeBuilderPreferences
    {
        /// <summary>
        /// The reconstruction volume voxel density in voxels per meter (vpm)
        /// 1000mm / 256vpm = ~3.9mm/voxel
        /// </summary>
        public float VoxelsPerMeter = 256.0f;

        /// <summary>
        /// The reconstruction volume voxel resolution in the X axis
        /// At a setting of 256vpm the volume is 384 / 256 = 1.5m wide
        /// </summary>
        public int VoxelsX = 384;

        /// <summary>
        /// The reconstruction volume voxel resolution in the Y axis
        /// At a setting of 256vpm the volume is 384 / 256 = 1.5m high
        /// </summary>
        public int VoxelsY = 384;

        /// <summary>
        /// The reconstruction volume voxel resolution in the Z axis
        /// At a setting of 256vpm the volume is 384 / 256 = 1.5m deep
        /// </summary>
        public int VoxelsZ = 384;

        /// <summary>
        /// Minimum depth distance threshold in meters. Depth pixels below this value will be
        /// returned as invalid (0). Min depth must be positive or 0.
        /// </summary>
        public float MinDepthClip = FusionDepthProcessor.DefaultMinimumDepth;

        /// <summary>
        /// Maximum depth distance threshold in meters. Depth pixels above this value will be
        /// returned as invalid (0). Max depth must be greater than 0.
        /// </summary>
        public float MaxDepthClip = FusionDepthProcessor.DefaultMaximumDepth;

        /// <summary>
        /// Frame interval we calculate the deltaFromReferenceFrame 
        /// </summary>
        public int DeltaFrameCalculationInterval = 2;

        /// <summary>
        /// The factor to downsample the depth image by for AlignPointClouds
        /// </summary>
        public int DownsampleFactor = 2;

        /// <summary>
        /// Maximum residual alignment energy where tracking is still considered successful
        /// </summary>
        public int SmoothingKernelWidth = 1; // 0=just copy, 1=3x3, 2=5x5, 3=7x7, here we create a 3x3 kernel

        /// <summary>
        /// Maximum residual alignment energy where tracking is still considered successful
        /// </summary>
        public float SmoothingDistanceThreshold = 0.04f; // 4cm, could use up to around 0.1f;

        /// <summary>
        /// Maximum translation threshold between successive poses when using AlignPointClouds
        /// </summary>
        public float MaxTranslationDeltaAlignPointClouds = 0.3f; // 0.15 - 0.3m per frame typical

        /// <summary>
        /// Maximum rotation threshold between successive poses when using AlignPointClouds
        /// </summary>
        public float MaxRotationDeltaAlignPointClouds = 20.0f; // 10-20 degrees per frame typica

        /// <summary>
        /// CameraPoseFinderDistanceThresholdReject is a threshold used following the minimum distance 
        /// calculation between the input frame and the camera pose finder database. This calculated value
        /// between 0 and 1.0f must be less than or equal to the threshold in order to run the pose finder,
        /// as the input must at least be similar to the pose finder database for a correct pose to be
        /// matched.
        /// </summary>
        public float CameraPoseFinderDistanceThresholdReject = 1.0f; // a value of 1.0 means no rejection
        
        /// <summary>
        /// Here we set a high limit on the maximum residual alignment energy where we consider the tracking
        /// with AlignPointClouds to have succeeded. Typically this value would be around 0.005f to 0.006f.
        /// (Lower residual alignment energy after relocalization is considered better.)
        /// </summary>
        public float MaxAlignPointCloudsEnergyForSuccess = 0.006f;

        /// <summary>
        /// The maximum number of matched poseCount we consider when finding the camera pose. 
        /// Although the matches are ranked, so we look at the highest probability match first, a higher 
        /// value has a greater chance of finding a good match overall, but has the trade-off of being 
        /// slower. Typically we test up to around the 5 best matches, after which is may be better just
        /// to try again with the next input depth frame if no good match is found.
        /// </summary>
        public int MaxCameraPoseFinderPoseTests = 5;

        /// <summary>
        /// Here we set a low limit on the residual alignment energy, below which we reject a tracking
        /// success report from AlignPointClouds and believe it to have failed. This can typically be around 0.
        /// </summary>
        public float MinAlignPointCloudsEnergyForSuccess = 0.0f;

        /// <summary>
        /// How many frames after starting tracking will will wait before starting to store
        /// image frames to the pose finder database. Here we set 200 successful frames (~7s).
        /// </summary>
        public int MinSuccessfulTrackingFramesForCameraPoseFinderAfterFailure = 200;

        /// <summary>
        /// Image integration weight
        /// </summary>
        public short IntegrationWeight = FusionDepthProcessor.DefaultIntegrationWeight;

        /// <summary>
        /// Frame interval we update the camera pose finder database.
        /// </summary>
        public int CameraPoseFinderProcessFrameCalculationInterval = 5;

        /// <summary>
        /// How many frames after starting tracking will will wait before starting to store
        /// image frames to the pose finder database. Here we set 45 successful frames (1.5s).
        /// </summary>
        public int MinSuccessfulTrackingFramesForCameraPoseFinder = 45;

        /// <summary>
        /// CameraPoseFinderDistanceThresholdAccept is a threshold passed to the ProcessFrame 
        /// function in the camera pose finder interface. The minimum distance between the input frame and
        /// the pose finder database must be greater than or equal to this value for a new pose to be 
        /// stored in the database, which regulates how close together poseCount are stored in the database.
        /// </summary>
        public float CameraPoseFinderDistanceThresholdAccept = 0.1f;

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
        public bool TranslateResetPoseByMinDepthThreshold = true;
    }
    */
}
