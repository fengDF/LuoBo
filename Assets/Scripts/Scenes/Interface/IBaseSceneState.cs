using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yangyu
{
    public interface IBaseSceneState
    {
        void EnterScene();
        void ExitScene();
    }
}