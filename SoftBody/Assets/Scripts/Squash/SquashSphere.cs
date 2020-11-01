using UnityEngine;

namespace Squash
{
    public class SquashSphere : MonoBehaviour
    {
        [Header("Mesh Variables")]

        #region Private Variables

        private Mesh _meshOriginal, _meshClone;

        private Renderer _renderer;

        private MeshCollider _meshCollider;

        #endregion

        [Header("Other Variables")]

        #region Public Variables

        public float Intensity = 1f;

        public float Mass = 1f;

        public float Stiffness = 1f;

        public float Damping = .75f;

        #endregion

        #region Private Variables

        private BodyVertex[] _bodyVertices;

        private Vector3[] _vertexArray;

        #endregion


        private void Awake()
        {
            _meshOriginal = GetComponent<MeshFilter>().sharedMesh;
            _meshClone = Instantiate(_meshOriginal);
            GetComponent<MeshFilter>().sharedMesh = _meshClone;
            _renderer = GetComponent<Renderer>();
            _meshCollider = GetComponent<MeshCollider>();
        }

        private void Start()
        {
            _bodyVertices = new BodyVertex[_meshClone.vertices.Length];
            for (var i = 0; i < _meshClone.vertices.Length; i++)
            {
                _bodyVertices[i] = new BodyVertex(i, transform.TransformPoint(_meshClone.vertices[i]));
            }
        }

        private void FixedUpdate()
        {
            ChangeMesh();
        }

        private void ChangeMesh()
        {
            _vertexArray = _meshOriginal.vertices;
            foreach (var t in _bodyVertices)
            {
                var target = transform.TransformPoint(_vertexArray[t.ID]);
                var bounds = _renderer.bounds;
                var intensity = (1 - (bounds.max.y - target.y) / bounds.size.y) * Intensity;
                t.Shake(target, Mass, Stiffness, Damping);
                target = transform.InverseTransformPoint(t.Position);
                _vertexArray[t.ID] = Vector3.Lerp(_vertexArray[t.ID], target, intensity);
            }

            _meshClone.vertices = _vertexArray;
            // _meshClone.RecalculateBounds();
            // _meshClone.RecalculateNormals(); 
            // _meshClone.RecalculateTangents();
            _meshCollider.sharedMesh = _meshClone;
        }
    }
}