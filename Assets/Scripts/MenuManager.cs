using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MakeOver.Constant;
using UnityEngine.SceneManagement;

namespace MakeOver.UI
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] Button _startButton;

        // Start is called before the first frame update
        void Start()
        {
            //_startButton.onClick.AddListener(OnClickStartButton);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnClickStartButton()
        {
            SceneManager.LoadScene(Constants.CandySceneName);
        }

        public void OnClickLevelCompleteButton()
        {
            CharacterModelController.Instance.LoadCharacterData();
            //Camera.main.gameObject.SetActive(false);
            //SceneManager.LoadScene(Constants.CharacterScene);
        }
    }
}
