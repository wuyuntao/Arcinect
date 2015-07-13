using Arcinect.Schema;
using FlatBuffers;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Arcinect
{
    class Recorder : Disposable
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

        /// <summary>
        /// Lock object of frame list
        /// </summary>
        private object framesLock = new object();

        /// <summary>
        /// Worker thread to process color and depth data
        /// </summary>
        private Thread workerThread;

        /// <summary>
        /// Event to stop worker thread
        /// </summary>
        private ManualResetEvent workerThreadStopEvent = new ManualResetEvent(false);

        /// <summary>
        /// Event to notify that frame data is added
        /// </summary>
        private ManualResetEvent frameDataSaveEvent = new ManualResetEvent(false);

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

            this.workerThread = new Thread(WorkerThreadProc);
            this.workerThread.Start();
        }

        protected override void DisposeManaged()
        {
            this.workerThreadStopEvent.Set();
            this.workerThread.Join();

            base.DisposeManaged();
        }

        private void WorkerThreadProc(object state)
        {
            using (var stream = new FileStream(this.fileName, FileMode.Create, FileAccess.Write))
            {
                var events = new WaitHandle[] { this.frameDataSaveEvent, this.workerThreadStopEvent };

                for (var i = WaitHandle.WaitAny(events); i != 1; i = WaitHandle.WaitAny(events))
                {
                    switch (i)
                    {
                        case 0:
                            this.frameDataSaveEvent.Reset();

                            List<Frame> frames;
                            lock (this.framesLock)
                            {
                                if (this.frames.Count > 0)
                                {
                                    frames = this.frames;
                                    this.frames = new List<Frame>();
                                }
                                else
                                {
                                    frames = null;
                                }
                            }

                            if (frames != null)
                            {
                                foreach (var frame in frames)
                                    WriteFrameToStream(stream, frame);
                            }

                            break;

                        default:
                            throw new ArgumentOutOfRangeException("Unexpected event index");
                    }
                }
            }
        }

        private static void WriteFrameToStream(FileStream stream, Frame frame)
        {
            var estimatedSize = sizeof(uint) +
                frame.ColorData.Length * sizeof(byte) +
                frame.DepthData.Length * sizeof(ushort) +
                1024;

            var fbb = new FlatBufferBuilder(estimatedSize);
            var colorDataVector = FrameData.CreateColorDataVector(fbb, frame.ColorData);
            var depthDataVector = FrameData.CreateDepthDataVector(fbb, frame.DepthData);
            var frameData = FrameData.CreateFrameData(fbb, frame.Time, colorDataVector, depthDataVector);
            fbb.Finish(frameData);

            var buffer = fbb.DataBuffer;
            var bufferSize = buffer.Length - buffer.Position;
            
            var bufferSizeBytes = BitConverter.GetBytes(bufferSize);
            stream.Write(bufferSizeBytes, 0, bufferSizeBytes.Length);
            stream.Write(buffer.Data, buffer.Position, bufferSize);
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

            lock (this.framesLock)
            {
                this.frames.Add(frame);
            }

            this.frameDataSaveEvent.Set();

            logger.Trace("Add frame. Time: {0}ms, ColorData: {1} bytes, DepthData: {2} bytes", frame.Time, frame.ColorData.Length, frame.DepthData.Length);
        }
    }
}