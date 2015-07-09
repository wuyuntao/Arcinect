using Microsoft.Kinect.Fusion;
using NLog;
using System;
using System.Globalization;
using System.IO;

namespace Arcinect
{

    /// <summary>
    /// Save mesh in ASCII WaveFront .OBJ file
    /// </summary>
    class ObjMeshWriter : Disposable
    {
        /// <summary>
        /// Logger of current class
        /// </summary>
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private string fileName;

        private TextWriter textWriter;

        private int vertexCount;

        public ObjMeshWriter(string fileName)
        {
            this.fileName = fileName;
            this.textWriter = new StreamWriter(fileName);
        }

        protected override void DisposeManaged()
        {
            SafeDispose(ref textWriter);

            base.DisposeManaged();
        }

        public void Write(ColorMesh mesh)
        {
            if (mesh == null)
            {
                throw new ArgumentNullException("mesh");
            }

            var vertices = mesh.GetVertices();
            var normals = mesh.GetNormals();
            var indices = mesh.GetTriangleIndexes();

            // Check mesh arguments
            if (vertices.Count == 0 || vertices.Count % 3 != 0 || vertices.Count != indices.Count)
            {
                throw new ArgumentException("Invalid mesh");
            }

            logger.Trace("Start writing mesh to {0}", this.fileName);

            // Write the header lines
            this.textWriter.WriteLine("#");
            this.textWriter.WriteLine("# OBJ file created by Arcinect © Wu Yuntao 2015");
            this.textWriter.WriteLine("#");

            // Sequentially write the 3 vertices of the triangle, for each triangle
            foreach (var vertex in vertices)
            {
                var vertexString = string.Format("v {0} {1} {2}",
                        vertex.X.ToString(CultureInfo.InvariantCulture),
                        vertex.Y.ToString(CultureInfo.InvariantCulture),
                        vertex.Z.ToString(CultureInfo.InvariantCulture));

                this.textWriter.WriteLine(vertexString);
            }

            // Sequentially write the 3 normals of the triangle, for each triangle
            foreach (var normal in normals)
            {
                var normalString = string.Format("vn {0} {1} {2}",
                        normal.X.ToString(CultureInfo.InvariantCulture),
                        normal.Y.ToString(CultureInfo.InvariantCulture),
                        normal.Z.ToString(CultureInfo.InvariantCulture));

                this.textWriter.WriteLine(normalString);
            }

            // Sequentially write the 3 vertex indices of the triangle face, for each triangle
            // Note this is typically 1-indexed in an OBJ file when using absolute referencing!
            for (int i = 0; i < vertices.Count / 3; i++)
            {
                var index = vertexCount + i * 3;
                var faceString = string.Format("f {0}//{0} {1}//{1} {2}//{2}",
                    (index + 1).ToString(CultureInfo.InvariantCulture),
                    (index + 2).ToString(CultureInfo.InvariantCulture),
                    (index + 3).ToString(CultureInfo.InvariantCulture));

                this.textWriter.WriteLine(faceString);
            }

            this.vertexCount += vertices.Count;

            logger.Trace("Finish writing mesh to {0}", this.fileName);
        }
    }
}