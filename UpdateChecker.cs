using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace UpdateStuff
{
    struct GameData
    {
        public string description;
        public string version;
        public string link;
    }
}

public class Updater : MonoBehaviour
{
    [SeralizeField] [TextArea(1, 5)] string JsonUrl; //the url of your JSON file
    static bool HasAlradyCheckedForUpdates = false;
    UpdateStuff.GameData LastData;
    
    void start()
    {
        if(!HasAlradyCheckedForUpdates)
        {
            StopAllCorutines();
            StartCorutine(checkUpdates());
            Debug.Log("Checking for updates");
        }
    }
    
    IEnumerator checkUpdates()
    {
        UnityWebRequest request = UnityWebRequest.Get(jsonDataURL);
        request.chunkedTransfer = false;
        request.disposeDownloadHandlerOnDispose = true;
        request.timeout = 60;
        
        yield return request.Send();
        
        if(request.IsDone)
        {
            LastData = JsonUtility.FromJson<UpdateStuff.GameData>(request.downloadHandler.text);
            if(!Application.version.Equals(LastData))
            {
                Debug.Log(LastData.description + "\n" + LastData.version + "\n" + LastData.link);
            }
            else
            {
                Debug.Log("The game is up to date, yay!");
            }
        }
        else
        {
            // the request failed
            Debug.Log("ERROR: failed to make the request to the server, check your internet connection!");
        }
    }
}