using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectOrganizer : MonoBehaviour
{
    public static StageSelectOrganizer instance;
    [SerializeField] private GameObject SelectScene;//-4,4 -15,4
    [SerializeField] private GameObject Stage1;//-6,2.1 -13,2.1
    [SerializeField] private GameObject Stage2;//-6,0.8 -13,0.8
    [SerializeField] private GameObject Stage3;//-6,-0.5 -13,-0.5
    [SerializeField] private GameObject Stage4;//-6,-1.8 -13,-1.8
    [SerializeField] private GameObject GoTitle;
    [SerializeField] private GameObject Manual;
    [SerializeField] private GameObject Credit;

    [SerializeField] private Transform StageTF1;
    [SerializeField] private Transform StageTF2;
    [SerializeField] private Transform StageTF3;
    [SerializeField] private Transform StageTF4;
    [SerializeField] private Transform GoTitleTF;
    [SerializeField] private Transform ManualTF;
    [SerializeField] private Transform CreditTF;

    [SerializeField] private Text StageText1;
    [SerializeField] private Text StageText2;
    [SerializeField] private Text StageText3;
    [SerializeField] private Text StageText4;
    [SerializeField] private Text GoTitleText;

    private int nowNum;
    private bool canSelect;
    private void SetNowNum(int _num)
    {
        nowNum = _num;
        StageText1.color = new Color(1, 1, 1);
        StageText2.color = new Color(1, 1, 1);
        StageText3.color = new Color(1, 1, 1);
        StageText4.color = new Color(1, 1, 1);
        GoTitleText.color = new Color(1, 1, 1);
        switch (nowNum)
        {
            case 1:
                StageText1.color = new Color(1, 0, 0);
                break;
            case 2:
                StageText2.color = new Color(1, 0, 0);
                break;
            case 3:
                StageText3.color = new Color(1, 0, 0);
                break;
            case 4:
                StageText4.color = new Color(1, 0, 0);
                break;
            case 5:
                GoTitleText.color = new Color(1, 0, 0);
                break;
        }
    }

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
        SoundManager.PlayBGM(SoundManager.BGM_Type.TitleScene);
        SetTexts();
        StartCoroutine(StartDeal());
        canSelect = false;
    }

    private void SetTexts()
    {
        SelectScene.transform.position = new Vector3(-15, 4, 0);
        StageTF1.position = new Vector3(-13, 2.1f, 0);
        StageTF2.position = new Vector3(-13, 0.8f, 0);
        StageTF3.position = new Vector3(-13, -0.5f, 0);
        StageTF4.position = new Vector3(-13, -1.8f, 0);
        GoTitleTF.position = new Vector3(-13, -3.1f, 0);
        ManualTF.position = new Vector3(9.0f,3.3f, 0);
        CreditTF.position = new Vector3(9f, -1.9f, 0);

        StageText1.color = new Color(1, 1, 1);
        StageText2.color = new Color(1, 1, 1);
        StageText3.color = new Color(1, 1, 1);
        StageText4.color = new Color(1, 1, 1);
        GoTitleText.color = new Color(1, 1, 1);
    }

    private IEnumerator StartDeal()
    {
        yield return new WaitForSeconds(0.5f);
        CreateHashtable(4, 0.2f, SelectScene);
        yield return new WaitForSeconds(0.2f);
        CreateHashtable(2.1f, 0.2f, Stage1);
        yield return new WaitForSeconds(0.1f);
        CreateHashtable(0.8f, 0.2f, Stage2);
        yield return new WaitForSeconds(0.1f);
        CreateHashtable(-0.5f, 0.2f, Stage3);
        yield return new WaitForSeconds(0.1f);
        CreateHashtable(-1.8f, 0.2f, Stage4);
        yield return new WaitForSeconds(0.1f);
        CreateHashtable(-3.1f, 0.2f, GoTitle);
        yield return new WaitForSeconds(0.2f);
        CreateHashtable(3.3f, 0.2f, Manual);
        yield return new WaitForSeconds(0.1f);
        CreateHashtable(-1.9f, 0.2f, Credit);
        yield return new WaitForSeconds(0.4f);
        canSelect = true;
        SetNowNum(1);
    }

    private void CreateHashtable(float y,float time,GameObject _gameObject)
    {
        Hashtable move = new Hashtable();
        float x = -6;
        if (y == 4)
        {
            x = -4;
        }
        if (y ==3.3f)
        {
            x = 0f;
        }
        if (y == -1.9f)
        {
            x = 1.0f;
        }
        move.Add("position", new Vector3(x, y, 0));
        move.Add("time", time);
        move.Add("easeType", "easeOutCubic");
        iTween.MoveTo(_gameObject, move);
        SoundManager.PlaySE(SoundManager.SE_Type.Move2);
    }

    void Update()
    {
        if (canSelect)
        {
            if (Input.GetButtonDown("Up"))
            {
                SetNowNum(nowNum - 1);
                if (nowNum < 1)
                {
                    SetNowNum(nowNum + 5);
                }
                SoundManager.PlaySE(SoundManager.SE_Type.Move);
            }
            if (Input.GetButtonDown("Down"))
            {
                SetNowNum(nowNum + 1);
                if (nowNum > 5)
                {
                    SetNowNum(nowNum - 5);
                }
                SoundManager.PlaySE(SoundManager.SE_Type.Move);
            }
            if (Input.GetButtonDown("Fire"))
            {
                if (nowNum != 5)
                {
                    SceneChangeManager.SceneChange(SceneEnum.StageSelect, SceneEnum.Game, nowNum);
                }
                else
                {
                    SceneChangeManager.SceneChange(SceneEnum.StageSelect, SceneEnum.Title, 1);
                }
                SoundManager.PlaySE(SoundManager.SE_Type.Decide);
            }
        }
    }
}
