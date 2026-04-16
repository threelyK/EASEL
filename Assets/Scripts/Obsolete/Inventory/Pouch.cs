using UnityEngine;

namespace Inventory
{
    public class Pouch : MonoBehaviour
    {
        void Update()
        {
            var rotation = transform.rotation;
            rotation.eulerAngles =  new Vector3(0, rotation.eulerAngles.y, 0);
        }
    }
}
