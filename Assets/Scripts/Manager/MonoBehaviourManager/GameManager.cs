using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yangyu
{
    /// <summary>
    /// 游戏总管理，负责管理所有的管理者
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        private static GameManager s_Instance;

        public static GameManager Instance
        {
            get { return s_Instance; }
        }


        public PlayerManager playerManager;
        public FactoryManager factoryManager;
        public AudioSourceManager audioSourceManager;
        public UIManager uiManager;


        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            s_Instance = this;


            playerManager = new PlayerManager();
            factoryManager = new FactoryManager();
            audioSourceManager = new AudioSourceManager();
            uiManager = new UIManager();
        }
    }
}