using UnityEngine;

namespace Squash
{
    public class BodyVertex
    {
        #region Public Variables

        public int ID;
        public Vector3 Position;
        public Vector3 Velocity, Force;

        #endregion

        public BodyVertex(int id, Vector3 pos)
        {
            ID = id;
            Position = pos;
        }

        public void Shake(Vector3 target, float mass, float stiffness, float damping)
        {
            Force = (target - Position) * stiffness;
            Velocity = (Velocity + Force / mass) * damping;
            Position += Velocity;
            if ((Velocity + Force + (Force / mass)).magnitude < .001f)
            {
                Position = target;
            }
        }
    }
}