using System.Collections.Generic;

namespace RaycastPro.RaySensors
{
    using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif
    
    public sealed class BounceRay : PathRay, IRadius
    {
        [Tooltip("Number of segments to divide the arc path.")]
        public int segments = 8;
        public int bounceCount = 1;
        public float multiplyRate = .6f;
        
        [Tooltip("If enabled, the velocity will be applied in local space.")]
        public bool velocityLocal;
        
        public LayerMask bonusLayer;

        
        [Tooltip("Total duration over which the arc is simulated.")]
        public float elapsedTime;
        
        [SerializeField, Tooltip("Collision detection radius (will be clamped to non-negative).")]
        private float radius;
        public float Radius
        {
            get => radius;
            set => radius = Mathf.Max(0,value);
        }
        
        [Tooltip("Initial velocity vector for the arc trajectory.")]
        public Vector3 velocity;
        protected override void OnCast()
        {
            UpdatePath();
            if (pathCast)
            {
                DetectIndex = PathCast(radius);
            }
        }
        
        /// <summary>
        /// Calculates arc trajectory points based on initial velocity and elapsed time.
        /// This is used both for casting logic and Gizmos visualization.
        /// </summary>
        protected override void UpdatePath()
        {
            PathPoints.Clear();
            if (bounceCount > 0)
            {     
                g = velocityLocal ? transform.TransformDirection(velocity) : velocity;
                _dir = Direction;
                _pos = transform.position;
                BounceCast(0);
            }
        }

        private RaycastHit _hit;
        private Vector3 tDir, lPos;
        private void BounceCast(int currentBounce)
        {
            _tPos = _pos;
            PathPoints.Add(_pos);
            for (var i = 1; i <= segments; i++)
            {
                t = (float) i / segments * elapsedTime;
                lPos = _pos;
                _pos = _tPos + (_dir * t + g * (t * t) / 2);
                tDir = _pos - lPos;
                if (radius > 0)
                {
                    Physics.SphereCast(lPos, radius, tDir, out _hit, tDir.magnitude, bonusLayer.value, triggerInteraction);
                }
                else
                {
                    Physics.Raycast(lPos, tDir, out _hit, tDir.magnitude, bonusLayer.value, triggerInteraction);
                }
                if (_hit.transform)
                {
                    if (currentBounce < bounceCount)
                    {
                        _dir = Quaternion.FromToRotation(Vector3.up, _hit.normal) * (_dir * multiplyRate);
                        _pos = _hit.point;
                        BounceCast(++currentBounce);
                        break;
                    }
                }
                else
                {
                    PathPoints.Add(_pos);
                }
            }
        }

        private float t;
        private Vector3 g, _dir, _tPos, _pos;
        
#if UNITY_EDITOR
        internal override string Info =>  "Send a ray based on the incoming velocity and return hit Info." + HAccurate + HDirectional + HPathRay + HIRadius + HPreview;
        internal override void OnGizmos()
        {
            EditorUpdate();
            
            FullPathDraw(radius, true);
            
            if (hit.transform) DrawNormal(hit.point, hit.normal, hit.transform.name);
        }

        internal override void EditorPanel(SerializedObject _so, bool hasMain = true, bool hasGeneral = true,
            bool hasEvents = true,
            bool hasInfo = true)
        {
            if (hasMain)
            {
                DirectionField(_so);
                BeginHorizontal();
                EditorGUILayout.PropertyField(_so.FindProperty(nameof(velocity)));
                LocalField(_so.FindProperty(nameof(velocityLocal)));
                EndHorizontal();
                EditorGUILayout.PropertyField(_so.FindProperty(nameof(segments)));
                segments = Mathf.Max(1, segments);

                BeginVerticalBox();
                EditorGUILayout.PropertyField(_so.FindProperty(nameof(bonusLayer)));
                
                
                EditorGUILayout.PropertyField(_so.FindProperty(nameof(elapsedTime)));
                EditorGUILayout.PropertyField(_so.FindProperty(nameof(bounceCount)));
                EditorGUILayout.PropertyField(_so.FindProperty(nameof(multiplyRate)));
                EndVertical();

                RadiusField(_so);
            }

            if (hasGeneral) PathRayGeneralField(_so);

            if (hasEvents) EventField(_so);

            if (hasInfo) InformationField();
        }
#endif
    }
}