using UnityEngine;
using System.Collections;

public class newCamera : MonoBehaviour
{
        public GameObject target;
       
        public float targetHeight = 1.7f;
        public float distance = 5.0f;
        public float offsetFromWall = 0.1f;
       
        public float maxDistance = 20;
        public float minDistance = .6f;
       
        public float xSpeed = 200.0f;
        public float ySpeed = 200.0f;
       
        public int yMinLimit = -80;
        public int yMaxLimit = 80;
       
        public int zoomRate = 40;
       
        public float rotationDampening = 3.0f;
        public float zoomDampening = 5.0f;
       
        public LayerMask collisionLayers = -1;
       
        private float xDeg = 0.0f;
        private float yDeg = 0.0f;
        private float currentDistance;
        private float desiredDistance;
        private float correctedDistance;
       
        void Start()
        {
                Vector3 angles = transform.eulerAngles;
                xDeg = angles.x;
                yDeg = angles.y;
               
                currentDistance = distance;
                desiredDistance = distance;
                correctedDistance = distance;
               
                if (GetComponent<Rigidbody>())
                        GetComponent<Rigidbody>().freezeRotation = true;
        }
       
        void LateUpdate()
        {
                Vector3 vTargetOffset;
               
                if (!target.transform)
                        return;
               
                if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
                {
                        xDeg += Input.GetAxis ("Mouse X") * xSpeed * 0.02f;
                        yDeg -= Input.GetAxis ("Mouse Y") * ySpeed * 0.02f;
                }
                else if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
                {
                        float targetRotationAngle = target.transform.eulerAngles.y;
                        float currentRotationAngle = transform.eulerAngles.y;
                        xDeg = Mathf.LerpAngle (currentRotationAngle, targetRotationAngle, rotationDampening * Time.deltaTime);
                }
               
                yDeg = ClampAngle (yDeg, yMinLimit, yMaxLimit);
               
                Quaternion rotation = Quaternion.Euler (yDeg, xDeg, 0);
               
                desiredDistance -= Input.GetAxis ("Mouse ScrollWheel") * Time.deltaTime * zoomRate * Mathf.Abs (desiredDistance);
                desiredDistance = Mathf.Clamp (desiredDistance, minDistance, maxDistance);
                correctedDistance = desiredDistance;
               
                vTargetOffset = new Vector3 (0, -targetHeight, 0);
                Vector3 position = target.transform.position - (rotation * Vector3.forward * desiredDistance + vTargetOffset);
               
                RaycastHit collisionHit;
                Vector3 trueTargetPosition = new Vector3 (target.transform.position.x, target.transform.position.y + targetHeight, target.transform.position.z);
               
                bool isCorrected = false;
                if (Physics.Linecast (trueTargetPosition, position, out collisionHit, collisionLayers.value))
                {
                        correctedDistance = Vector3.Distance (trueTargetPosition, collisionHit.point) - offsetFromWall;
                        isCorrected = true;
                }
               
                currentDistance = !isCorrected || correctedDistance > currentDistance ? Mathf.Lerp (currentDistance, correctedDistance, Time.deltaTime * zoomDampening) : correctedDistance;
               
                currentDistance = Mathf.Clamp (currentDistance, minDistance, maxDistance);
               
                position = target.transform.position - (rotation * Vector3.forward * currentDistance + vTargetOffset);
               
                transform.rotation = rotation;
                transform.position = position;

                transform.LookAt(target.transform);
        }
       
        private static float ClampAngle(float angle, float min, float max)
        {
                if (angle < -360)
                        angle += 360;
                if (angle > 360)
                        angle -= 360;
                return Mathf.Clamp (angle, min, max);
        }
}