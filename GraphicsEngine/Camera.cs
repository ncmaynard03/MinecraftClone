using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsEngine
{
    internal class Camera
    {
        public Vector3 Position;
        public Vector3 Direction;
        Vector3 Target;

        public Vector3 Front = -Vector3.UnitZ;
        public Vector3 Right = Vector3.UnitX;
        public Vector3 Up = Vector3.UnitY;
        Vector3 WorldUp = Vector3.UnitY;
        public float Yaw;
        public float Pitch;
        public Matrix4 View;

        public Camera(Vector3 position)
        {
            Position = new Vector3(position.X, position.Y, position.Z);
            //Console.WriteLine("Position: " + Position);
            Target = Vector3.Zero;
            Direction = Vector3.Normalize(Position - Target);
            Pitch = 0f;
            Yaw = -90f;
            UpdateCameraVectors();
        }

        public void UpdateCameraVectors()
        {
            // Calculate the new Front vector
            Front.X = (float)Math.Cos(MathHelper.DegreesToRadians(Yaw)) * (float)Math.Cos(MathHelper.DegreesToRadians(Pitch));
            Front.Y = (float)Math.Sin(MathHelper.DegreesToRadians(Pitch));
            Front.Z = (float)Math.Sin(MathHelper.DegreesToRadians(Yaw)) * (float)Math.Cos(MathHelper.DegreesToRadians(Pitch));
            Front = Vector3.Normalize(Front);
            Console.WriteLine(Front);

            // Also re-calculate the Right and Up vector
            Right = Vector3.Normalize(Vector3.Cross(Front, WorldUp));  // Normalize the vectors, because their length gets closer to 0 the more you look up or down which results in slower movement.
            Up = Vector3.Normalize(Vector3.Cross(Right, Front));

            // Update the view matrix
            View = Matrix4.LookAt(Position, Position + Front, Up);
            Console.WriteLine(Pitch);
            Console.WriteLine(Yaw + "\n\n");
            //Console.WriteLine(View + "\n");
        }


        public void MoveRelative(Vector3 relativePosition)
        {
            Position += relativePosition;
            UpdateCameraVectors();
        }

        public void RotateRelative(float pitch, float yaw)
        {
            Yaw -= yaw;
            Pitch += pitch;

            Pitch = MathHelper.Clamp(Pitch, -89f, 89f);

            UpdateCameraVectors();
        }
    }
}
