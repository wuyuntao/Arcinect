using Microsoft.Kinect;
using NLog;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
        private const float DefaultDPI = 96;

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

        private WriteableBitmap colorBitmap;

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
            this.colorBitmap = new WriteableBitmap(colorWidth, colorHeight, DefaultDPI, DefaultDPI, PixelFormats.Bgr32, null);

            this.depthWidth = depthWidth;
            this.depthHeight = depthHeight;
            this.depthData = new ushort[depthWidth * depthHeight];
        }

        /// <summary>
        /// Update frame data of color / depth frames
        /// </summary>
        /// <param name="frameReference"></param>

        #region Update from Kinect sensor
       
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

                    UpdateColorData(colorFrame);
                    UpdateDepthData(depthFrame);
                }
            }
        }

        private void UpdateColorData(ColorFrame colorFrame)
        {
            var colorFrameDescription = colorFrame.FrameDescription;
            if (this.colorWidth == colorFrameDescription.Width && this.colorHeight == colorFrameDescription.Height)
            {
                colorFrame.CopyConvertedFrameDataToArray(this.colorData, ColorImageFormat.Rgba);

                UpdateColorBitmap(colorFrame);

                logger.Trace("ColorFrame updated");
            }
            else
            {
                logger.Error("Size of ColorFrame does not match. Expected: {0}x{1}, Actual: {2}x{3}",
                    this.colorWidth, this.colorHeight, colorFrameDescription.Width, colorFrameDescription.Height);
            }
        }

        private void UpdateColorBitmap(ColorFrame colorFrame)
        {
            var colorFrameDescription = colorFrame.FrameDescription;
            using (var colorBuffer = colorFrame.LockRawImageBuffer())
            {
                this.colorBitmap.Lock();

                // verify data and write the new color frame data to the display bitmap
                if ((colorFrameDescription.Width == this.colorBitmap.PixelWidth) && (colorFrameDescription.Height == this.colorBitmap.PixelHeight))
                {
                    colorFrame.CopyConvertedFrameDataToIntPtr(
                        this.colorBitmap.BackBuffer,
                        (uint)(colorFrameDescription.Width * colorFrameDescription.Height * 4),
                        ColorImageFormat.Bgra);

                    this.colorBitmap.AddDirtyRect(new Int32Rect(0, 0, this.colorBitmap.PixelWidth, this.colorBitmap.PixelHeight));
                }
                else
                {
                    logger.Error("Size of ColorBitmap does not match. Expected: {0}x{1}, Actual: {2}x{3}",
                        this.colorBitmap.PixelWidth, this.colorBitmap.PixelHeight, colorFrameDescription.Width, colorFrameDescription.Height);
                }

                this.colorBitmap.Unlock();
            }
        }

        private void UpdateDepthData(DepthFrame depthFrame)
        {
            var depthFrameDescription = depthFrame.FrameDescription;
            if (this.depthWidth == depthFrameDescription.Width && this.depthHeight == depthFrameDescription.Height)
            {
                depthFrame.CopyFrameDataToArray(this.depthData);

                logger.Trace("DepthFrame updated");
            }
            else
            {
                logger.Error("Size of DepthFrame does not match. Expected: {0}x{1}, Actual: {2}x{3}",
                    this.depthWidth, this.depthHeight, depthFrameDescription.Width, depthFrameDescription.Height);
            }
        }
        
        #endregion

        #region Properties

        public BitmapSource ColorBitmap
        {
            get { return this.colorBitmap; }
        }
        
        #endregion
    }
}