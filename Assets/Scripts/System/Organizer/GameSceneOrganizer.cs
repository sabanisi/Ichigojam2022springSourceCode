using System;
using UnityEngine;

public class GameSceneOrganizer:MonoBehaviour
{
    private int stageNum;
    public int GetStageNum()
    {
        return stageNum;
    }
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private BackGround _backGround;
    [SerializeField] private Player _player;

    [SerializeField] private GameObject CatPrefab;
    [SerializeField] private GameObject SpiderPrefab;
    [SerializeField] private GameObject PuppetterPrefab;
    [SerializeField] private GameObject SnakePrefab;

    public static GameSceneOrganizer instance;

    public void Start()
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

    public void Initiazlie(int num)
    {
        stageNum = num;
        string text = "";
        switch (stageNum)
        {
            case 3:
                text = "ステージ3\nネコ";
                break;
            case 4:
                text = "ステージ4\nクモ";
                break;
            case 2:
                text = "ステージ2\nパペッター";
                break;
            case 1:
                text = "ステージ1\nヘビ";
                break;
            default:
                Debug.Log(stageNum + " がない");
                break;
        }
        _player.Initialize();
        _backGround.Initialize(num);
        _gameManager.Initialize(text);

        switch (stageNum)
        {
            case 3:
                _gameManager.SetBoss(Instantiate(CatPrefab, transform).GetComponent<Cats>());
                break;
            case 4:
                _gameManager.SetBoss(Instantiate(SpiderPrefab, transform).GetComponent<Spider>());
                break;
            case 2:
                _gameManager.SetBoss(Instantiate(PuppetterPrefab, transform).GetComponent<Puppetter>());
                break;
            case 1:
                _gameManager.SetBoss(Instantiate(SnakePrefab, transform).GetComponent<Snake>());
                break;
        }
        SoundManager.PlayBGM(SoundManager.BGM_Type.GameScene);
    }

    public void DestroyDeal()
    {
        _gameManager.DestroyDeal();
    }

}
