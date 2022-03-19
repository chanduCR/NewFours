using Unity.Netcode;
using Unity.Netcode.Transports.UNET;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MyProject;

public class MyServer : MonoBehaviour
{
    [SerializeField]
    InputField ipAddr;

    [SerializeField]
    Toggle hostBox;

    [SerializeField]
    MyLoader loader;

    MyPlayer localPlayer;

    private void Start()
    {
        ipAddr.text = "127.0.0.1:7777";
    }

    public void OnStartClick()
    {
        loader.LoadNextScene(getIP(), getPort(), hostBox.isOn);
    }

    private string getIP()
    {
        //Debug.Log(ipAddr.text.Split(':')[0]);
        return ipAddr.text.Split(':')[0];
    }

    private int getPort()
    {
        //Debug.Log(ipAddr.text.Split(':')[1]);
        return int.Parse(ipAddr.text.Split(':')[1]);
    }

    private void OnDestroy()
    {
        
    }

}
