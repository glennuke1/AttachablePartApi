# AttachablePartApi
Adds a way for modders to easily add assembling and disassembling to their mods
The api also saves the attached state (You'll need to delete the save value in your


Note: You must have the dll in your mods folder

Setup:

1. Create a pivot object with a trigger collider and set its parent to a GameObject (this is the object you can rotate, move, resize, etc)
2. To your attachable object, add the AttachComp component and set its pivot to your pivot object
3. Done

Example Script:

```
using MSCLoader;
using UnityEngine;
using AttachablePartApi;

namespace ExampleMod
{
    public class ExampleMod : Mod
    {
        public override string ID => "ExampleMod"; //Your mod ID (unique)
        public override string Name => "Example"; //You mod name
        public override string Author => "Username"; //Your Username
        public override string Version => "1.0"; //Version
        public override string Description => ""; //Short description of your mod

        public override void ModSetup()
        {
            SetupFunction(Setup.OnLoad, Mod_OnLoad);
            SetupFunction(Setup.OnNewGame, Mod_OnNewGame);
        }

        public override void ModSettings()
        {
            // All settings should be created here. 
            // DO NOT put anything else here that settings or keybinds
        }
        
        private void Mod_OnNewGame(
        {
            SaveLoad.DeleteValue(ModLoader.GetMod("AttachablePartApi"), "attachedCube(Clone)"); //the second string is the word "attached" followed by your objects name
        }

        private void Mod_OnLoad()
        {
            GameObject pivot = new GameObject("CubePivot");
            pivot.transform.position = new Vector3(0, 0, 2);
            pivot.AddComponent<BoxCollider>().isTrigger = true;
            GameObject one = GameObject.CreatePrimitive(PrimitiveType.Cube);
            one.AddComponent<Rigidbody>();
            one.MakePickable();
            one.AddComponent<AttachComp>().pivot = pivot.transform;
            one.name = "Cube(Clone)";
            
            GameObject pivot2 = new GameObject("SpherePivot");
            pivot2.transform.position = new Vector3(0, 1, 2);
            pivot2.AddComponent<BoxCollider>().isTrigger = true;
            GameObject two = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            two.AddComponent<Rigidbody>();
            two.MakePickable();
            two.AddComponent<AttachComp>().pivot = pivot2.transform;
            two.name = "Sphere(Clone)";
        }
    }
}
```
