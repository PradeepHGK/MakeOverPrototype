using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class APIManager : MonoBehaviour
{

    [SerializeField] string apiURL;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(APIRequestHandler(apiURL));
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator APIRequestHandler(string url)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(url);

        yield return uwr.SendWebRequest();


        if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.DataProcessingError)
        {
            Debug.Log(uwr.result.ToString());
        }

        if (uwr.isDone)
        {
            var response = uwr.downloadHandler.text;
            Debug.Log($"response: {response}");
        }
    }
}

[System.Serializable]
public class Civilization
{
    public int id { get; set; }
    public string name { get; set; }
    public string expansion { get; set; }
    public string army_type { get; set; }
    public List<string> unique_unit { get; set; }
    public List<string> unique_tech { get; set; }
    public string team_bonus { get; set; }
    public List<string> civilization_bonus { get; set; }
}

[System.Serializable]
public class Root
{
    public List<Civilization> civilizations { get; set; }
}