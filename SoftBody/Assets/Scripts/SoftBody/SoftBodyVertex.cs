using UnityEngine;

namespace SoftBody
{
    public class SoftBodyVertex
    {
        [Header("Mesh Variables")]

        #region Public Variables

        public int VerticeIndex;

        public Vector3 InitialVertexPosition;
        public Vector3 CurrentVertexPosition;

        #endregion

        [Header("Other Variables")]

        #region Public Variables

        public Vector3 CurrentVelocity;

        #endregion

        public SoftBodyVertex(int _verticeIndex, Vector3 _initialVertexPosition, Vector3 _currentVertexPosition,
            Vector3 _currentVelocity)
        {
            VerticeIndex = _verticeIndex;
            InitialVertexPosition = _initialVertexPosition;
            CurrentVertexPosition = _currentVertexPosition;
            CurrentVelocity = _currentVelocity;
        }

        private Vector3 GetCurrentDisplacement()
        {
            return CurrentVertexPosition - InitialVertexPosition;
        }

        public void UpdateVelocity(float _bounceSpeed)
        {
            CurrentVelocity -= GetCurrentDisplacement() * (_bounceSpeed * Time.deltaTime);
        }

        public void Settle(float _stiffness)
        {
            CurrentVelocity *= 1f - _stiffness * Time.deltaTime;
        }

        public void ApplyPressureToVertex(Transform _transform, Vector3 _position, float _pressure)
        {
            var distanceVerticePoint = CurrentVertexPosition - _transform.InverseTransformPoint(_position);
            var adaptedPressure = _pressure / (1f + distanceVerticePoint.sqrMagnitude);
            var velocity = adaptedPressure * Time.deltaTime;
            CurrentVelocity += distanceVerticePoint.normalized * velocity;
        }
    }
}