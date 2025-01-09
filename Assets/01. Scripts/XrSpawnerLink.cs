using Autohand.Demo;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class XrSpawnerLink : MonoBehaviour
{
    public XrSpawner XrSpawner;
    public XRNode role;
    public CommonButton button;

    bool spawning = false;
    InputDevice device;
    List<InputDevice> devices;

    void Start()
    {
        devices = new List<InputDevice>();
    }

    void FixedUpdate()
    {
        InputDevices.GetDevicesAtXRNode(role, devices);
        if (devices.Count > 0)
            device = devices[0];

        if (device != null && device.isValid)
        {
            //Sets hand fingers wrap
            if (device.TryGetFeatureValue(XRHandControllerLink.GetCommonButton(button), out bool spawnButton))
            {
                if (spawning && !spawnButton)
                {
                    XrSpawner.Spawn();
                    spawning = false;
                }
                else if (!spawning && spawnButton)
                {
                    XrSpawner.StartSpawn();
                    spawning = true;
                }
            }
        }
    }
}
