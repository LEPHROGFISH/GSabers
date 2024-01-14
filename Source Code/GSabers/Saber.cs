using BepInEx;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using GorillaLocomotion;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

namespace GSabers
{
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]

    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Saber : BaseUnityPlugin
    {
        GameObject saber;

        Transform blade;

        AudioSource saberOn;
        AudioSource saberOff;
        AudioSource saberIdle;

        bool bladeStatus;
        private bool _lastFrame;
        private bool _currentFrame;


        void OnEnable()
        {
            if (saber != null)
            {
                saber.SetActive(true);
            }
        }

        void OnDisable()
        {
            if (saber != null)
            {
                saber.SetActive(false);
            }
        }


        void Start()
        {
            Utilla.Events.GameInitialized += OnGameInitialized;
        }
        void OnGameInitialized(object sender, EventArgs e)
        {
            setupSaber();
        }

        void setupSaber()
        {
            var bundle = LoadAssetBundle("GSabers.bundles.saber");
            var asset = bundle.LoadAsset<GameObject>("EclipseSaber");

            //Pretty much spawns in the saber to the players right hand and then sets the location rotation and scale of the light saber
            saber = Instantiate(asset, GameObject.Find("Player Objects/Local VRRig/Local Gorilla Player/rig/body/shoulder.R/upper_arm.R/forearm.R/hand.R").transform);
            saber.transform.localPosition = new Vector3(0.05052872f, 0.07158074f, 0.008802317f);
            saber.transform.localRotation = Quaternion.Euler(-16.652f, -9.292f, -80.824f);

            //I don't feel like doing anything else
            saberOn = saber.transform.FindChild("SaberOn").GetComponent<AudioSource>();
            saberOff = saber.transform.FindChild("SaberOff").GetComponent<AudioSource>();
            saberIdle = saber.transform.FindChild("SaberIdle").GetComponent<AudioSource>();


            blade = saber.transform.Find("Blade");
            blade.localScale = new Vector3(1, 1, 0);
        }
        public AssetBundle LoadAssetBundle(string path)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            AssetBundle bundle = AssetBundle.LoadFromStream(stream);
            stream.Close();
            return bundle;
        }


        void Update()
        {

            _lastFrame = _currentFrame;
            _currentFrame = ControllerInputPoller.instance.rightControllerPrimaryButton;

            if (GetButtonDown() || UnityInput.Current.GetKeyDown(KeyCode.E))
            {
                //Coolest code on the block😎
                if (!bladeStatus)
                {
                    saberOn.Play();
                    saberIdle.Play();
                    blade.localScale = new Vector3(1, 1, 1);
                    bladeStatus = true;

                }
                else
                {
                    saberOff.Play();
                    saberIdle.Stop();
                    blade.localScale = new Vector3(1, 1, 0);
                    bladeStatus = false;
                }

            }

        }
        //Remaking getKeyDown for the controller buttons probably another way but I got no clue what it would be so don't say anything about it cuh
        public bool GetButtonDown()
        {
            return !_lastFrame && _currentFrame;
        }

    }
}
    