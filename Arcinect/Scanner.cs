using Microsoft.Kinect;
using Microsoft.Kinect.Fusion;
using NLog;
using System;

namespace Arcinect
{
    /// <summary>
    /// Scanner
    /// </summary>
    sealed class Scanner : Disposable
    {
        /// <summary>
        /// Logger of current class
        /// </summary>
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Singleton instance of Scanner
        /// </summary>
        private static Scanner instance;

        /// <summary>
        /// Active Kinect sensor
        /// </summary>
        private KinectSensor sensor;

        /// <summary>
        /// Reader for depth & color
        /// </summary>
        private MultiSourceFrameReader reader;

        /// <summary>
        /// Frame data
        /// </summary>
        private Frame frame;

        #region Init / Dispose

        Scanner(KinectSensor sensor)
        {
            this.sensor = sensor;
            this.sensor.Open();

            if (this.sensor.IsOpen)
            {
                this.reader = this.sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Depth | FrameSourceTypes.Color);

                var colorFrameDescription = this.sensor.ColorFrameSource.FrameDescription;
                var depthFrameDescription = this.sensor.DepthFrameSource.FrameDescription;

                this.frame = new Frame(colorFrameDescription.Width, colorFrameDescription.Height, depthFrameDescription.Width, depthFrameDescription.Height);

                this.reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;

                logger.Trace("Kinect sensor is open");
            }
            else
            {
                logger.Error("Kinect sensor is not open");
            }
        }

        /// <summary>
        /// Factory method of Scanner
        /// </summary>
        /// <returns>return a singleton instance of Scanner</returns>
        public static Scanner Open()
        {
            if (instance == null && IsHardwareCompatible())
            {
                var sensor = KinectSensor.GetDefault();
                if (sensor == null)
                {
                    logger.Error("Kinect sensor is neither connected nor available");
                }
                else
                {
                    logger.Trace("Found a Kinect sensor");

                    instance = new Scanner(sensor);
                }
            }

            return instance;
        }

        /// <summary>
        /// Close scanner
        /// </summary>
        public static void Close()
        {
            if (instance != null)
            {
                instance.Dispose();
                instance = null;
            }
        }

        /// <summary>
        /// Check to ensure suitable DirectX11 compatible hardware exists before initializing Kinect Fusion
        /// </summary>
        /// <returns></returns>
        private static bool IsHardwareCompatible()
        {
            try
            {
                string deviceDescription;
                string deviceInstancePath;
                int deviceMemoryKB;
                FusionDepthProcessor.GetDeviceInfo(ReconstructionProcessor.Amp, -1, out deviceDescription, out deviceInstancePath, out deviceMemoryKB);

                return true;
            }
            catch (IndexOutOfRangeException ex)
            {
                // Thrown when index is out of range for processor type or there is no DirectX11 capable device installed.
                // As we set -1 (auto-select default) for the DeviceToUse above, this indicates that there is no DirectX11 
                // capable device. The options for users in this case are to either install a DirectX11 capable device 
                // (see documentation for recommended GPUs) or to switch to non-real-time CPU based reconstruction by 
                // changing ProcessorType to ReconstructionProcessor.Cpu

                logger.Error("No DirectX11 device detected, or invalid device index", ex);
                return false;
            }
            catch (DllNotFoundException ex)
            {
                logger.Error("A prerequisite component for Kinect Fusion is missing", ex);
                return false;
            }
            catch (InvalidOperationException ex)
            {
                logger.Error("Unknown exception", ex);
                return false;
            }
        }

        /// <summary>
        /// Dispose resources
        /// </summary>
        protected override void DisposeManaged()
        {
            if (this.sensor != null)
            {
                this.sensor.Close();
                this.sensor = null;
            }

            instance = null;

            base.DisposeManaged();
        }

        #endregion

        private void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            logger.Trace("Frame arrived");

            this.frame.Update(e.FrameReference);
        }

        public Frame Frame
        {
            get { return this.frame; }
        }
    }
}