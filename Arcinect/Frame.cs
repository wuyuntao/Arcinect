using Microsoft.Kinect;
using NLog;

namespace Arcinect
{
    /// <summary>
    /// Frame Data
    /// </summary>
    sealed class Frame : Disposable
    {
        /// <summary>
        /// Logger of current class
        /// </summary>
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Image width of color frame
        /// </summary>
        private int colorWidth;

        /// <summary>
        /// Image height of color frame
        /// </summary>
        private int colorHeight;

        /// <summary>
        /// Intermediate storage for the color data received from the camera in 32bit color
        /// </summary>
        private byte[] colorData;

        /// <summary>
        /// Image Width of depth frame
        /// </summary>
        private int depthWidth;

        /// <summary>
        /// Image height of depth frame
        /// </summary>
        private int depthHeight;

        /// <summary>
        /// Intermediate storage for the extended depth data received from the camera in the current frame
        /// </summary>
        private ushort[] depthData;

        public Frame(int colorWidth, int colorHeight, int depthWidth, int depthHeight)
        {
            this.colorWidth = colorWidth;
            this.colorHeight = colorHeight;
            this.colorData = new byte[colorWidth * colorHeight * sizeof(int)];

            this.depthWidth = depthWidth;
            this.depthHeight = depthHeight;
            this.depthData = new ushort[depthWidth * depthHeight];
        }

        /// <summary>
        /// Update frame data of color / depth frames
        /// </summary>
        /// <param name="frameReference"></param>
        public void Update(MultiSourceFrameReference frameReference)
        {
            var multiSourceFrame = frameReference.AcquireFrame();
            if (multiSourceFrame == null)
            {
                logger.Trace("Abort update since MultiSourceFrame is null");
                return;
            }

            using (var colorFrame = multiSourceFrame.ColorFrameReference.AcquireFrame())
            {
                if (colorFrame == null)
                {
                    logger.Trace("Abort update since ColorFrame is null");
                    return;
                }

                using (var depthFrame = multiSourceFrame.DepthFrameReference.AcquireFrame())
                {
                    if (depthFrame == null)
                    {
                        logger.Trace("Abort update since DepthFrame is null");
                        return;
                    }

                    var colorFrameDescription = colorFrame.FrameDescription;
                    var colorFrameSize = colorFrameDescription.Width * colorFrameDescription.Height * sizeof(int);

                    if (colorFrameSize != this.colorData.Length)
                    {
                        logger.Error("Size of ColorFrame does not match. Expected: {0}, Actual: {1}", this.colorData.Length, colorFrameSize);
                    }
                    else
                    {
                        colorFrame.CopyConvertedFrameDataToArray(this.colorData, ColorImageFormat.Rgba);

                        logger.Trace("ColorFrame updated");
                    }

                    var depthFrameDescription = depthFrame.FrameDescription;
                    var depthFrameSize = depthFrameDescription.Width * depthFrameDescription.Height;

                    if (depthFrameSize != this.depthData.Length)
                    {
                        logger.Error("Size of DepthFrame does not match. Expected: {0}, Actual: {1}", this.depthData.Length, depthFrameSize);
                    }
                    else
                    {
                        depthFrame.CopyFrameDataToArray(this.depthData);

                        logger.Trace("DepthFrame updated");
                    }
                }
            }
        }
    }
}