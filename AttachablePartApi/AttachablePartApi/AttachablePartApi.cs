using MSCLoader;
using UnityEngine;

namespace AttachablePartApi
{
    public class AttachablePartApi : Mod
    {
        public override string ID => "AttachablePartApi"; //Your mod ID (unique)
        public override string Name => "AttachablePartApi"; //You mod name
        public override string Author => "glen"; //Your Username
        public override string Version => "1.0"; //Version
        public override string Description => ""; //Short description of your mod

        public override void ModSetup()
        {
            SetupFunction(Setup.PostLoad, Mod_PostLoad);
            SetupFunction(Setup.OnSave, Mod_OnSave);
        }

        public override void ModSettings()
        {
            // All settings should be created here. 
            // DO NOT put anything else here that settings or keybinds
        }

        private void Mod_PostLoad()
        {
                foreach (AttachComp attachable in GameObject.FindObjectsOfType<AttachComp>())
                {
                    if (SaveLoad.ReadValue<bool>(this, "attached"+attachable.gameObject.name))
                    {
                        attachable.Attach();
                    }
                }
        }

        private void Mod_OnSave()
        {
            foreach (AttachComp attachable in GameObject.FindObjectsOfType<AttachComp>())
            {
                SaveLoad.WriteValue(this, "attached"+attachable.gameObject.name, attachable.on);
            }
        }
    }
}
