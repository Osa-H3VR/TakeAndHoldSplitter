using System;
using BepInEx.Logging;
using Deli.Setup;
using On.FistVR;
using UnityEngine.SceneManagement;

namespace Osa.TakeAndHoldSplitter
{
    public class Hooks
    {
        private LiveSplitClient _client = null;
        private readonly ManualLogSource _manualLogSource;

        private bool _started = false;

        public Hooks(LiveSplitClient client, ManualLogSource manualLogSource)
        {
            _client = client;
            _manualLogSource = manualLogSource;
            Hook();
        }

        public void Dispose()
        {
            Unhook();
        }
        
        private void Unhook()
        {
            try
            {
                _client.Reset();
                _client.Dispose();
            }
            catch (Exception e)
            {
                _manualLogSource.LogError(e);
                throw;
            }
        }

        private void Hook()
        {
            On.FistVR.TNH_Manager.SetPhase_Take +=TNH_ManagerOnSetPhase_Take;
            On.FistVR.TNH_Manager.SetPhase_Dead +=TNH_ManagerOnSetPhase_Dead;
            On.FistVR.TNH_Manager.SetPhase_Completed +=TNH_ManagerOnSetPhase_Completed;
            SceneManager.activeSceneChanged +=SceneManagerOnactiveSceneChanged;
        }

        private void SceneManagerOnactiveSceneChanged(Scene arg0, Scene arg1)
        {
            _started = false;
            _client.Reset();
        }

        private void TNH_ManagerOnSetPhase_Completed(TNH_Manager.orig_SetPhase_Completed orig, FistVR.TNH_Manager self)
        {
            orig(self);
            try
            {
                _client.Split();
            }
            catch (Exception e)
            {
                _manualLogSource.LogError(e);
                throw;
            }
        }

        private void TNH_ManagerOnSetPhase_Dead(TNH_Manager.orig_SetPhase_Dead orig, FistVR.TNH_Manager self)
        {
            orig(self);
            try
            {
                _client.Pause();
            }
            catch (Exception e)
            {
                _manualLogSource.LogError(e);
                throw;
            }
        }
        

        private void TNH_ManagerOnSetPhase_Take(On.FistVR.TNH_Manager.orig_SetPhase_Take orig, FistVR.TNH_Manager self)
        {
            orig(self);
            try
            {
                if (!_started)
                {
                    _client.Reset();
                    _client.StartTimer();
                    _started = true;
                }
                else
                {
                    _client.Split(); 
                }
            }
            catch (Exception e)
            {
                _manualLogSource.LogError(e);
                throw;
            }

        }
    }
}