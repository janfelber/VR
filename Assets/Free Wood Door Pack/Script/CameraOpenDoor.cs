using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace CameraDoorScript
{
    public class CameraOpenDoor : MonoBehaviour
    {
        public float DistanceOpen = 3f;
        public GameObject text;
        public XRController controller; // Reference to the XR controller
        private InputDevice targetDevice;

        void Start()
        {
            // Get the XR controller's device
            targetDevice = controller.inputDevice;
        }

        void Update()
        {
            RaycastHit hit;
            // Perform raycast using the controller position and direction
            if (Physics.Raycast(controller.transform.position, controller.transform.forward, out hit, DistanceOpen))
            {
                if (hit.transform.GetComponent<DoorScript.Door>())
                {
                    text.SetActive(true);
                    if (targetDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerValue) && triggerValue)
                    {
                        hit.transform.GetComponent<DoorScript.Door>().OpenDoor();
                    }
                }
                else
                {
                    text.SetActive(false);
                }
            }
            else
            {
                text.SetActive(false);
            }
        }
    }
}