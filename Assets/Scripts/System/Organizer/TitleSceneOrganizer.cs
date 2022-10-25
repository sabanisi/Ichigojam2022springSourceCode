using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneOrganizer : MonoBehaviour
{
    public static TitleSceneOrganizer instance;
    private bool isAlive;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Initialize()
    {
        Invoke("SetAliveTrue", 0.5f);
        SoundManager.PlayBGM(SoundManager.BGM_Type.TitleScene);
    }

    private void SetAliveTrue()
    {
        isAlive = true;
    }

    void Update()
    {
        if (isAlive)
        {
            if (Input.anyKey)
            {
                SoundManager.PlaySE(SoundManager.SE_Type.Decide);
                SceneChangeManager.SceneChange(SceneEnum.Title, SceneEnum.StageSelect, 1);
                isAlive = false;
            }
        }
    }
}
