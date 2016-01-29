using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class ChatBox_InputFieldScript : NetworkBehaviour
{
    const short chatMsg = 1000;
    NetworkClient _client;

    public SyncListString chatLog = new SyncListString();

    [SerializeField]
    private Text chatline;
    [SerializeField]
    ScrollRect scrollRect;
    [SerializeField]
    InputField inputField;

    [SerializeField]
    GameObject TextPrefab;
    [SerializeField]
    GameObject chatPanel;

    
    private float amountOfWaitTime = 10.0f;
    private float timer = 10.0f;

    int state = 0;

    bool autoScrollToBottom = true;

    public override void OnStartClient()
    {
        chatLog.Callback = OnChatUpdated;
        _client = NetworkManager.singleton.client;

        NetworkServer.RegisterHandler(chatMsg, OnServerPostChatMessage);
        inputField.onEndEdit.AddListener(delegate { PostChatMessage(inputField.text); });

    }

    private void OnChatUpdated(SyncListString.Operation op, int index)
    {
        chatline.text += chatLog[chatLog.Count - 1] + "\n";
        
        if(chatPanel.activeSelf == false) {
            chatPanel.SetActive(true);
            state = 1;
        }
        // reset the wait timer
        timer = amountOfWaitTime;
    }

    public void OnMouseDownForVerticalScrollbar()
    {
        //Debug.Log("mouse down");
        autoScrollToBottom = false;
    }

    public void OnMouseUpForVerticalScrollbar()
    {
        //Debug.Log("mouse up");
        if (scrollRect.verticalScrollbar.value <= 0.0f)
        {
            autoScrollToBottom = true;
        }
    }

    void Start()
    {
        //_client = NetworkManager.singleton.client;

        //NetworkServer.RegisterHandler(chatMsg, OnServerPostChatMessage);
        //inputField.onEndEdit.AddListener(delegate { PostChatMessage(inputField.text); });
    }

    [Server]
    void OnServerPostChatMessage(NetworkMessage netMsg)
    {
        string message = netMsg.ReadMessage<StringMessage>().value;
        chatLog.Add(message);
    }

    [Client]
    public void PostChatMessage(string message)
    {
        if (message.Length == 0) return;
        var msg = new StringMessage(" " + DATACORE.PlayerClassStats.clientChatName + ": " + message);
        _client.Send(chatMsg, msg);

        inputField.text = "";
        inputField.ActivateInputField();
        inputField.Select();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (!inputField.isFocused)
            {
                // if the panel is off...
                if (chatPanel.activeSelf == false)
                {
                    // turn it on and ready for typing...
                    chatPanel.SetActive(true);
                    state = 0;

                    inputField.Select();
                    inputField.ActivateInputField();
                }

                // if the panel is on...
                else
                {
                    // if we're in typing mode...
                    if(state == 0)
                    {
                        // turn the panel off.
                        chatPanel.SetActive(false);
                    }
                    // if in read only mode...
                    else if(state == 1)
                    {
                        // turn the panel on in type mode...
                        chatPanel.SetActive(true);
                        state = 0;

                        inputField.Select();
                        inputField.ActivateInputField();
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (inputField.isFocused)
            {
                inputField.text = "";
                inputField.DeactivateInputField();
                return;
            }
            else
            {
                chatPanel.SetActive(false);
            }
        }

        if(state == 1)
        {
            timer -= 1.0f * Time.deltaTime;
            if(timer <= 0.0f) {
                timer = amountOfWaitTime;

            }
        }
    }

    void OnGUI()
    {
        if (state == 0)
        {
            inputField.interactable = true;
            Image[] images = chatPanel.GetComponentsInChildren<Image>();
            foreach (Image img in images)
            {
                img.color = new Color(img.color.r, img.color.g, img.color.b, 0.7f);
            }
        }

        else if (state == 1)
        {
            inputField.interactable = false;

            Image[] images = chatPanel.GetComponentsInChildren<Image>();
            foreach (Image img in images)
            {
                img.color = new Color(img.color.r, img.color.g, img.color.b, 0.1f);
            } 
            
        }

        // force vertical scrollbar to always be at bottom
        if (autoScrollToBottom)
        {
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalScrollbar.value = 0f;
            Canvas.ForceUpdateCanvases();
        }
    }

}
