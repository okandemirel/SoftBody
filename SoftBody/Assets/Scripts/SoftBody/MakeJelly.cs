using UnityEngine;

namespace SoftBody
{
    public class MakeJelly : MonoBehaviour
    {
        [Header("Actors")]

        #region Public Variables

        public float BounceSpeed;

        public float FallForce;
        public float Stiffness;

        #endregion

        [Header("Self Variables")]

        #region Private Variables

        private MeshFilter _meshFilter;

        private MeshCollider _meshCollider;
        private Mesh _mesh, _meshClone;

        #endregion

        [Header("Other Variables")]

        #region Private Variables

        private SoftBodyVertex[] _jellyVertices;

        private Vector3[] _currentMeshVertices;

        #endregion

        private void Awake()
        {
            _meshCollider = GetComponent<MeshCollider>();
            _meshFilter = GetComponent<MeshFilter>();
            _mesh = _meshFilter.sharedMesh;
            _meshClone = Instantiate(_mesh);
            GetComponent<MeshFilter>().sharedMesh = _meshClone;
        }

        private void Start()
        {
            GetVertices();
        }

        private void GetVertices()
        {
            _jellyVertices = new SoftBodyVertex[_meshClone.vertices.Length];
            _currentMeshVertices = new Vector3[_meshClone.vertices.Length];

            for (int i = 0; i < _meshClone.vertices.Length; i++)
            {
                _jellyVertices[i] = new SoftBodyVertex(i, _meshClone.vertices[i], _meshClone.vertices[i], Vector3.zero);
                _currentMeshVertices[i] = _meshClone.vertices[i];
            }
        }

        private void Update()
        {
            UpdateVertices();
        }

        private void UpdateVertices()
        {
            for (int i = 0; i < _jellyVertices.Length; i++)
            {
                _jellyVertices[i].UpdateVelocity(_bounceSpeed: BounceSpeed);
                _jellyVertices[i].Settle(_stiffness: Stiffness);

                _jellyVertices[i].CurrentVertexPosition += _jellyVertices[i].CurrentVelocity * Time.deltaTime;
                _currentMeshVertices[i] = _jellyVertices[i].CurrentVertexPosition;
            }

            _meshClone.vertices = _currentMeshVertices;
            _meshClone.RecalculateBounds();
            _meshClone.RecalculateNormals();
            _meshClone.RecalculateTangents();
            _meshCollider.sharedMesh = _meshClone;
        }

        private void OnCollisionEnter(Collision other)
        {
            var collisionPoints = other.contacts;
            foreach (var t in collisionPoints)
            {
                var inputPoint = t.point + (t.point * .1f);
                ApplyPressureToPoint(inputPoint, FallForce);
            }
        }

        private void ApplyPressureToPoint(Vector3 _point, float _pressure)
        {
            foreach (var t in _jellyVertices)
            {
                t.ApplyPressureToVertex(transform, _point, _pressure);
            }
        }
    }
}