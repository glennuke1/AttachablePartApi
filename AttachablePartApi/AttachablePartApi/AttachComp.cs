using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;
using MSCLoader;

namespace AttachablePartApi
{
    public class AttachComp : MonoBehaviour
    {
        public Transform pivot;
        public bool on;
        public bool screwable;

        GameObject raycast_parent;
        FsmGameObject raycast_object;

        void Start()
        {
            raycast_parent = GameObject.Find("PLAYER").transform.FindChild("Pivot/AnimPivot/Camera/FPSCamera/1Hand_Assemble/Hand").gameObject;
            raycast_object = PlayMakerFSM.FindFsmOnGameObject(raycast_parent, "PickUp").FsmVariables.FindFsmGameObject("RaycastHitObject");

            if (SaveLoad.ReadValue<bool>(ModLoader.GetMod("AttachablePartApi"), "attached" + gameObject.name))
            {
                Attach();
            }
        }

        void OnTriggerStay(Collider other)
        {
            if (other.gameObject == pivot.gameObject && gameObject.layer == LayerMask.NameToLayer("Wheel") && !on)
            {
                PlayMakerGlobals.Instance.Variables.GetFsmBool("GUIassemble").Value = true;
                if (Input.GetMouseButtonDown(0))
                {
                    Attach();
                    PlayMakerGlobals.Instance.Variables.GetFsmBool("GUIassemble").Value = false;
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject == pivot.gameObject)
            {
                PlayMakerGlobals.Instance.Variables.GetFsmBool("GUIassemble").Value = false;
            }
        }

        void Update()
        {
            if (raycast_object.Value == gameObject && on)
            {
                PlayMakerGlobals.Instance.Variables.GetFsmBool("GUIdisassemble").Value = true;
                if (Input.GetMouseButtonDown(1))
                {
                    Detach();
                    PlayMakerGlobals.Instance.Variables.GetFsmBool("GUIdisassemble").Value = false;
                }
            }
        }

        public void Attach()
        {
            ModConsole.Log("La");
            transform.parent = pivot;
            transform.localPosition = Vector3.zero;
            transform.localEulerAngles = Vector3.zero;
            transform.gameObject.tag = "Untagged";
            Destroy(GetComponent<Rigidbody>());
            MasterAudio.PlaySound3DAndForget("CarBuilding", transform, false, 1f, null, 0f, "assemble");
            StartCoroutine(stay());
            pivot.GetComponent<Collider>().enabled = false;
            on = true;
        }

        void Detach()
        {
            StopCoroutine(stay());
            transform.parent = null;
            transform.gameObject.tag = "PART";
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            MasterAudio.PlaySound3DAndForget("CarBuilding", transform, false, 1f, null, 0f, "disassemble");
            pivot.GetComponent<Collider>().enabled = true;
            on = false;
        }

        IEnumerator stay()
        {
            yield return new WaitForSeconds(0.1f);
            transform.parent = pivot;
            transform.localPosition = Vector3.zero;
            transform.localEulerAngles = Vector3.zero;
            transform.gameObject.tag = "Untagged";
        }
    }
}