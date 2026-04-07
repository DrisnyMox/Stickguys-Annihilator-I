using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;
using UnityEngine;
using System;

public class AdvertiseService : MonoBehaviour {

	public static string appKey = "";
	
	void Start () { 

		appKey = "1fac65437578908eb5be3e2e721517ab2857dc21fb407c89";

        DebugLog.Add("before initialize");
        
        Appodeal.initialize (appKey, Appodeal.REWARDED_VIDEO | Appodeal.BANNER | Appodeal.INTERSTITIAL, false);

        DebugLog.Add("after initialize");
    }
	

	public static void ShowAdmobBottom(){
		Appodeal.show (Appodeal.BANNER_BOTTOM);
	}

	public static void HideAdmobBottom(){
		Appodeal.hide (Appodeal.BANNER);
	}
	


}   
