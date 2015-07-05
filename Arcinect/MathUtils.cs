using Microsoft.Kinect.Fusion;
using System;

namespace Arcinect
{
    /// <summary>
    /// A helper class for common math operations.
    /// </summary>
    static class MathUtils
    {
        /// <summary>
        /// Test whether the camera moved too far between sequential frames by looking at starting
        /// and end transformation matrix. We assume that if the camera moves or rotates beyond a
        /// reasonable threshold, that we have lost track.
        /// Note that on lower end machines, if the processing frame rate decreases below 30Hz,
        /// this limit will potentially have to be increased as frames will be dropped and hence
        /// there will be a greater motion between successive frames.
        /// </summary>
        /// <param name="initial">The transform matrix from the previous frame.</param>
        /// <param name="final">The transform matrix from the current frame.</param>
        /// <param name="maxTrans">
        /// The maximum translation in meters we expect per x,y,z component between frames under normal motion.
        /// </param>
        /// <param name="maxRotDegrees">
        /// The maximum rotation in degrees we expect about the x,y,z axes between frames under normal motion.
        /// </param>
        /// <returns>
        /// True if camera transformation is greater than the threshold, otherwise false.
        /// </returns>
        public static bool CheckTransformChange(Matrix4 initial, Matrix4 final, float maxTrans, float maxRotDegrees)
        {
            // Check if the transform is too far out to be reasonable 
            var maxRot = (maxRotDegrees * (float)Math.PI) / 180.0f;

            // Calculate the deltas
            var eulerInitial = RotationMatrixToEulerFloatArray(initial);
            var eulerFinal = RotationMatrixToEulerFloatArray(final);

            var transInitial = ExtractTranslationFloatArray(initial);
            var transFinal = ExtractTranslationFloatArray(final);

            var eulerDeltas = new float[3];
            var transDeltas = new float[3];

            for (int i = 0; i < 3; i++)
            {
                // Handle when one angle is near PI, and the other is near -PI.
                if (eulerInitial[i] >= Math.PI - maxRot && eulerFinal[i] < maxRot - Math.PI)
                {
                    eulerInitial[i] -= (float)(Math.PI * 2);
                }
                else if (eulerFinal[i] >= Math.PI - maxRot && eulerInitial[i] < maxRot - Math.PI)
                {
                    eulerFinal[i] -= (float)(Math.PI * 2);
                }

                eulerDeltas[i] = eulerInitial[i] - eulerFinal[i];
                transDeltas[i] = transInitial[i] - transFinal[i];

                if (Math.Abs(eulerDeltas[i]) > maxRot)
                {
                    return false;
                }

                if (Math.Abs(transDeltas[i]) > maxTrans)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Extract 3x3 rotation matrix from the Matrix4 4x4 transformation,
        /// then convert to Euler angles.
        /// </summary>
        /// <param name="transform">The transform matrix.</param>
        /// <returns>Array of floating point values for Euler angle rotations around x,y,z.</returns>
        public static float[] RotationMatrixToEulerFloatArray(Matrix4 transform)
        {
            float[] rotation = new float[3];

            float phi = (float)Math.Atan2(transform.M23, transform.M33);
            float theta = (float)Math.Asin(-transform.M13);
            float psi = (float)Math.Atan2(transform.M12, transform.M11);

            rotation[0] = phi;  // This is rotation about x,y,z, or pitch, yaw, roll respectively
            rotation[1] = theta;
            rotation[2] = psi;

            return rotation;
        }

        /// <summary>
        /// Extract translation from the Matrix4 4x4 transformation in M41,M42,M43
        /// </summary>
        /// <param name="transform">The transform matrix.</param>
        /// <returns>Array of floating point values for translation in x,y,z.</returns>
        public static float[] ExtractTranslationFloatArray(Matrix4 transform)
        {
            float[] translation = new float[3];

            translation[0] = transform.M41;
            translation[1] = transform.M42;
            translation[2] = transform.M43;

            return translation;
        }

    }
}
