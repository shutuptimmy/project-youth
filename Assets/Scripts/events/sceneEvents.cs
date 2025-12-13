using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sceneEvents
{
    public event Action<SceneField, Vector2> onChangeScene;
    public void changeScene(SceneField scene, Vector2 newPos)
    {
        onChangeScene?.Invoke(scene, newPos);
    }

    public event Action onPlayCrossFade;
    public void playCrossFade()
    {
        onPlayCrossFade?.Invoke();
    }
}
