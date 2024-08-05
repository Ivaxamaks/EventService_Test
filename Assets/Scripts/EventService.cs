using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using UnityEngine.Networking;

public class EventService : MonoBehaviour
{
    private const string SERVER_URL = "";
    private const float COOLDOWN_BEFORE_SEND = 2f;
    private const long RESPOND_VALUE_OK = 200;

    private List<Event> _eventQueue = new(); 
    private List<Event> _pendingQueue = new();
    
    private bool _isCooldownActive;
    private bool _isSending;

    private void Start()
    {
        LoadPendingEvents();
    }
    
    public void TrackEvent(string type, string data)
    {
        var newEvent = new Event { type = type, data = data };
        _eventQueue.Add(newEvent);
        SavePendingEvents();
        
        if (!_isCooldownActive) 
            StartCoroutine(CooldownCoroutine());
    }
    
    private IEnumerator CooldownCoroutine()
    {
        _isCooldownActive = true;
        yield return new WaitForSeconds(COOLDOWN_BEFORE_SEND);
        _isCooldownActive = false;
        
        StartCoroutine(SendEvents());
    }
    
    private IEnumerator SendEvents()
    {
        if (_isSending || _eventQueue.Count == 0) yield break;

        _isSending = true;
        _pendingQueue = new List<Event>(_eventQueue);
        _eventQueue.Clear();

        var json = JsonUtility.ToJson(new EventWrapper { Events = _pendingQueue });

        using (var request = new UnityWebRequest(SERVER_URL, "POST"))
        {
            var bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success && request.responseCode == RESPOND_VALUE_OK)
            {
                _pendingQueue.Clear();
                SavePendingEvents();
            }
            else
            {
                Debug.LogError("Error sending events: " + request.error);
                _eventQueue.AddRange(_pendingQueue);
            }
        }

        _isSending = false;
        SavePendingEvents();
    }
    
    private void SavePendingEvents()
    {
        var allEvents = new List<Event>(_pendingQueue);
        allEvents.AddRange(_eventQueue);
        var events = JsonUtility.ToJson(new EventWrapper { Events = allEvents });
        PlayerPrefs.SetString("PendingEvents", events);
        PlayerPrefs.Save();
    }
    
    private void LoadPendingEvents()
    {
        var events = PlayerPrefs.GetString("PendingEvents", string.Empty);
        if (string.IsNullOrEmpty(events)) return;
        var loadedEvents = JsonUtility.FromJson<EventWrapper>(events);
        _eventQueue = new List<Event>(loadedEvents.Events);
    }

    [System.Serializable]
    private class Event
    {
        public string type;
        public string data;
    }

    [System.Serializable]
    private class EventWrapper
    {
        public List<Event> Events;
    }
}