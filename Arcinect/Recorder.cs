using Arcinect.Schema;
using FlatBuffers;
using NLog;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Arcinect
{
    class Recorder
    {
        /// <summary>
        /// Logger of current class
        /// </summary>
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// File path to save frame timeline
        /// </summary>
        private string fileName;

        /// <summary>
        /// Stopwatch to calculate elapsed time
        /// </summary>
        private Stopwatch watch = Stopwatch.StartNew();

        /// <summary>
        /// List to hold frame data
        /// </summary>
        private List<Frame> frames = new List<Frame>();

        #region Frame

        /// <summary>
        /// Struct to hold frame data
        /// </summary>
        class Frame
        {
            public uint Time;
            public byte[] ColorData;
            public ushort[] DepthData;
        }

        #endregion

        public Recorder(string fileName)
        {
            this.fileName = fileName;
        }

        /// <summary>
        /// Append frame data
        /// </summary>
        /// <param name="colorData"></param>
        /// <param name="depthData"></param>
        public void AppendFrame(byte[] colorData, ushort[] depthData)
        {
            var frame = new Frame()
            {
                Time = (uint)watch.ElapsedMilliseconds,
                ColorData = colorData,
                DepthData = depthData,
            };

            this.frames.Add(frame);

            logger.Trace("Add frame. Time: {0}ms, ColorData: {1} bytes, DepthData: {2} bytes", frame.Time, frame.ColorData.Length, frame.DepthData.Length);
        }

        /// <summary>
        /// Save frame timeline to file
        /// </summary>
        public void Save()
        {
            var size = EstimateTotalSize();
            if (size == 0)
            {
                logger.Trace("Nothing to save");
                return;
            }
            else if (size < 0)
            {
                logger.Error("Too big size to save");
                return;
            }

            var fbb = new FlatBufferBuilder(size);

            var frames = new List<int>();
            foreach (var frame in this.frames)
            {
                var colorDataVector = FrameData.CreateColorDataVector(fbb, frame.ColorData);
                var depthDataVector = FrameData.CreateDepthDataVector(fbb, frame.DepthData);
                var frameData = FrameData.CreateFrameData(fbb, frame.Time, colorDataVector, depthDataVector);

                frames.Add(frameData);
            }

            var framesVector = FrameTimeline.CreateFramesVector(fbb, frames.ToArray());
            var timeline = FrameTimeline.CreateFrameTimeline(fbb, framesVector);
            fbb.Finish(timeline);

            using (var fileStream = new FileStream(this.fileName, FileMode.Create, FileAccess.Write))
            {
                var buffer = fbb.DataBuffer;
                var bufferSize = buffer.Length - buffer.Position;
                fileStream.Write(buffer.Data, buffer.Position, bufferSize);

                logger.Trace("Save frame timeline to file {0} [Size: {1}bytes]", this.fileName, bufferSize);
            }
        }

        /// <summary>
        /// Estimate total size of FlatBufferBuilder
        /// </summary>
        /// <returns>Estimated total size</returns>
        private int EstimateTotalSize()
        {
            var size = 0;
            foreach (var frame in this.frames)
            {
                size += sizeof(uint) +
                        frame.ColorData.Length * sizeof(byte) +
                        frame.DepthData.Length * sizeof(ushort);
            }

            return size + 1024;
        }
    }
}