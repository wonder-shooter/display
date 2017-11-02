using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterHandler : MonoBehaviour {

    SteamVR_TrackedController trackedController;
    GameDirector gameDirector;

    void Start()
    {

        trackedController = gameObject.GetComponent<SteamVR_TrackedController>();

        if (trackedController == null)
        {
            trackedController = gameObject.AddComponent<SteamVR_TrackedController>();
        }
        gameDirector = GameDirector.GetSheredInstance();
        gameDirector.AddListenerViveration(OnViveration);
       
        //trackedController.MenuButtonClicked += new ClickedEventHandler(DoMenuButtonClicked);
        //trackedController.MenuButtonUnclicked += new ClickedEventHandler(DoMenuButtonUnClicked);
        trackedController.TriggerClicked += new ClickedEventHandler(DoTriggerClicked);
        //trackedController.TriggerUnclicked += new ClickedEventHandler(DoTriggerUnclicked);
        //trackedController.SteamClicked += new ClickedEventHandler(DoSteamClicked);
        //trackedController.PadClicked += new ClickedEventHandler(DoPadClicked);
        //trackedController.PadUnclicked += new ClickedEventHandler(DoPadClicked);
        //trackedController.PadTouched += new ClickedEventHandler(DoPadTouched);
        //trackedController.PadUntouched += new ClickedEventHandler(DoPadTouched);
        //trackedController.Gripped += new ClickedEventHandler(DoGripped);
        //trackedController.Ungripped += new ClickedEventHandler(DoUngripped);
    }

    private void Update()
    {
        var transform = trackedController.transform;
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit))
        {
            try
            {
                if (hit.collider.tag == "MaskScreen")
                {
                    var deviceType = (Tracker.DeviceType)Enum.Parse(typeof(Tracker.DeviceType), this.name, true);
                    var position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                    Tracker tracker = new Tracker(deviceType, position);
                    gameDirector.HoverTracker(tracker);
                }

            }
            catch (Exception ex)
            {
                Debug.Log("非対応デバイス");
            }

        }
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.green, 5, false);

    }

    private void OnViveration(Tracker.DeviceType type)
    {
        if(type.ToString() == this.trackedController.name)
        {
            var device = SteamVR_Controller.Input((int)trackedController.controllerIndex);
            device.TriggerHapticPulse(1000);
        }
    }

    //public void DoMenuButtonClicked(object sender, ClickedEventArgs e)
    //{
    //    Debug.Log("DoMenuButtonClicked");
    //}

    //public void DoMenuButtonUnClicked(object sender, ClickedEventArgs e)
    //{
    //    Debug.Log("DoMenuButtonUnClicked");
    //}

    /***
     * トリガークリック 
     */
    public void DoTriggerClicked(object sender, ClickedEventArgs e)
    {
        var transform = trackedController.transform;
        
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit))
        {
            //Rayの可視化（デバッグ用）
            Debug.DrawLine(Camera.main.transform.position, hit.point, Color.red);
            Debug.DrawRay(hit.point, hit.normal, Color.green);

            try
            {
                var deviceType = (Tracker.DeviceType)Enum.Parse(typeof(Tracker.DeviceType), this.name, true);

                if (hit.collider.tag == "MaskScreen")
                {
                    var pos = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                    gameDirector.ShotTracker(new Tracker(deviceType, pos));
                }

            } catch (Exception ex) {
                Debug.Log("非対応デバイス");
            }

        }

    }

    //public void DoTriggerUnclicked(object sender, ClickedEventArgs e)
    //{
    //    Debug.Log("DoTriggerUnclicked");
    //}

    //public void DoSteamClicked(object sender, ClickedEventArgs e)
    //{
    //    Debug.Log("DoSteamClicked");
    //}

    //public void DoPadClicked(object sender, ClickedEventArgs e)
    //{
    //    Debug.Log("DoPadClicked");
    //}

    //public void DoPadUnclicked(object sender, ClickedEventArgs e)
    //{
    //    Debug.Log("DoPadUnclicked");
    //}

    //public void DoPadTouched(object sender, ClickedEventArgs e)
    //{
    //    Debug.Log("DoPadTouched");
    //}

    //public void DoPadUntouched(object sender, ClickedEventArgs e)
    //{
    //    Debug.Log("DoPadUntouched");
    //}

    //public void DoGripped(object sender, ClickedEventArgs e)
    //{
    //    Debug.Log("DoGripped");
    //}

    //public void DoUngripped(object sender, ClickedEventArgs e)
    //{
    //    Debug.Log("DoUngripped");
    //}

}
