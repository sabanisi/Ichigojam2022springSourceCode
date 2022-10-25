using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private int score;

    [SerializeField] private Player player;
    private Boss boss;
    public void SetBoss(Boss _boss)
    {
        boss = _boss;
    }

    [SerializeField] private GameObject BlackSquare;//ゲームオーバー・ポーズ用の半透明オブジェクト
    [SerializeField] private GameObject BlackSquareForBomb;//ボム用の半透明オブジェクト

    //ポーズ画面用フィールド
    [SerializeField] private GameObject Pause;
    [SerializeField] private GameObject GoOn;
    [SerializeField] private GameObject Restart;
    [SerializeField] private GameObject GoBack;
    [SerializeField] private Text GoOnText;
    [SerializeField] private Text RestartText;
    [SerializeField] private Text GoBackText;
    private bool isPause;//ポーズを管理するフラグ
    private bool isPausePrepare;//ポーズ準備・片付けも含めて管理するフラグ
    private int pauseStateNum;
    public bool IsPause()
    {
        return isPause;
    }

    //ゲームスタート時の演出用フィールド
    [SerializeField] private GameObject GameStartEffect;
    private bool isGameStart;
    public bool IsGameStart()
    {
        return isGameStart;
    }

    //ゲームオーバー用フィールド
    [SerializeField] private GameObject GameOverOrClear;
    [SerializeField] private GameObject Score;
    [SerializeField] private GameObject Restart2;
    [SerializeField] private GameObject GoBack2;
    [SerializeField] private Text GameOverOrClearText;
    [SerializeField] private Text RestartText2;
    [SerializeField] private Text GoBackText2;
    private bool isGameOver;
    public bool IsGameOver()
    {
        return isGameOver;
    }
    private bool isGameOverEffectFinish;
    private int gameOverStateNum;

    private bool isGameChange;//シーンチェンジ時に使用するフラグ

    [SerializeField] private GameObject SubUnits;

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

    public void Initialize(string gameStartText)
    {
        isPause = false;
        isPausePrepare = false;
        isGameStart = false;
        isGameOver = false;
        isGameOverEffectFinish = false;
        pauseStateNum = 1;
        gameOverStateNum = 1;
        isGameChange = false;

        BlackSquare.SetActive(false);
        BlackSquareForBomb.SetActive(false);

        Pause.transform.position = new Vector3(-13, 2.4f, 0);
        GoOn.transform.position = new Vector3(11.5f, -0.15f, 0);
        Restart.transform.position = new Vector3(11.5f, -1.45f, 0);
        GoBack.transform.position = new Vector3(11.5f, -2.75f, 0);
        GameStartEffect.GetComponent<Text>().text = gameStartText;
        GameStartDeal();

        GameOverOrClear.transform.position = new Vector3(0, 7.5f, 0);
        Score.SetActive(false);
        Restart2.SetActive(false);
        GoBack2.SetActive(false);
        RestartText2.color = new Color(1, 0, 0);
        GoBackText2.color = new Color(1, 1, 1);
        GoOnText.color = new Color(1, 1, 1);
        GoBackText.color = new Color(1, 1, 1);
        RestartText.color = new Color(1, 1, 1);
    }

    public void DestroyDeal()
    {
        DestroySubUnits();
        Destroy(boss.gameObject);
        BulletManager.instance.DestroyDeal();
    }

    void Update()
    {
        if (!isGameStart) return;
        if (isGameOver)
        {
            if (isGameOverEffectFinish)
            {
                GameOverDeal();
            }
            return;
        }

        if (!isPause)
        {
            if (Input.GetButtonDown("Pause"))
            {
                isPause = true;
                isPausePrepare = true;
                BlackSquare.SetActive(true);
                pauseStateNum = 1;

                Hashtable PausemoveHash = new Hashtable();
                PausemoveHash.Add("position", new Vector3(-5.3f,3.7f,0));
                PausemoveHash.Add("time", 0.15f);
                PausemoveHash.Add("delay", 0f);
                PausemoveHash.Add("easeType", "easeOutQuart");
                PausemoveHash.Add("oncomplete", "PauseMoveFinish1");
                PausemoveHash.Add("oncompletetarget",gameObject);
                iTween.MoveTo(Pause,PausemoveHash);
                SoundManager.PlaySE(SoundManager.SE_Type.Move2);
            }
        }
        else
        {
            if (!isPausePrepare)
            {
                PauseDeal();
            }

            if (Input.GetButtonDown("Pause"))
            {
                GoOnDeal();
                SoundManager.PlaySE(SoundManager.SE_Type.Decide);
            }
        }
    }

    private void GameOverDeal()
    {
        RestartText2.color = new Color(1, 1, 1);
        GoBackText2.color = new Color(1, 1, 1);
        switch (gameOverStateNum)
        {
            case 1:
                RestartText2.color = new Color(1, 0, 0);
                break;
            case 2:
                GoBackText2.color = new Color(1, 0, 0);
                break;
            default:
                Debug.Log(gameOverStateNum + "が用意されていない");
                break;
        }
        if (Input.GetButtonDown("Up"))
        {
            gameOverStateNum--;
            if (gameOverStateNum < 1)
            {
                gameOverStateNum = 2;
            }
            SoundManager.PlaySE(SoundManager.SE_Type.Move);
        }
        if (Input.GetButtonDown("Down"))
        {
            gameOverStateNum++;
            if (gameOverStateNum > 2)
            {
                gameOverStateNum = 1;
            }
            SoundManager.PlaySE(SoundManager.SE_Type.Move);
        }
        if (Input.GetButtonDown("Fire")&&!isGameChange)
        {
            SoundManager.PlaySE(SoundManager.SE_Type.Decide);
            switch (gameOverStateNum)
            {
                case 1:
                    //最初から
                    isGameChange = true;
                    SceneChangeManager.SceneChange(SceneEnum.Game,SceneEnum.Game, GameSceneOrganizer.instance.GetStageNum());
                    break;
                case 2:
                    //タイトルへ
                    isGameChange = true;
                    SceneChangeManager.SceneChange(SceneEnum.Game, SceneEnum.StageSelect, 1);
                    break;
                default:
                    Debug.Log(gameOverStateNum + "がない");
                    break;
            }
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        BlackSquare.SetActive(true);
        GameOverOrClearText.text = "ゲーム　オーバー";
        Invoke("GameOverDeal1", 0.5f);
        BulletManager.instance.AllBulletDelete();
        SoundManager.PlaySE(SoundManager.SE_Type.Miss);
        SoundManager.StopBGM();
    }

    public void GameClear()
    {
        isGameOver = true;
        boss.GameClearDeal();
        BulletManager.instance.AllBulletDelete();
    }

    //ボス破壊演出が終わったあとに呼ばれる
    public void GameClearDeal()
    {
        BlackSquare.SetActive(true);
        GameOverOrClearText.text = "ゲーム　クリア！";
        Invoke("GameOverDeal1", 0.5f);
        SoundManager.PlaySE(SoundManager.SE_Type.Clear2);
    }

    private void GameOverDeal1()
    {
        Hashtable table = new Hashtable();
        table.Add("position", new Vector3(0, 3.3f, 0));
        table.Add("time", 0.5f);
        table.Add("easyType", "easeOutQuart");
        table.Add("oncomplete", "GameOverDeal2");
        table.Add("oncompletetarget", gameObject);
        iTween.MoveTo(GameOverOrClear, table);
    }

    private void GameOverDeal2()
    {
        Invoke("GameOverDeal3", 1.5f);
        Invoke("GameOverDeal4", 2.0f);
    }

    private void GameOverDeal3()
    {
        Score.SetActive(true);
        Score.GetComponent<Text>().text = "死んだ数:"+GetPlayer().GetDiedCount() + "　使用ボム数:" + GetPlayer().GetUsedBombCount();
        SoundManager.PlaySE(SoundManager.SE_Type.Show);
    }

    private void GameOverDeal4()
    {
        Restart2.SetActive(true);
        GoBack2.SetActive(true);
        isGameOverEffectFinish = true;
        SoundManager.PlaySE(SoundManager.SE_Type.Show);
    }

    private void GameStartDeal()
    {
        Invoke("EnterBossDeal", 1.0f);
    }

    private void EnterBossDeal()
    {
        boss.GameStartDeal();
    }

    public void GameStartDeal1()
    {
        GameStartEffect.transform.position = new Vector3(0, -8f, 0);
        Hashtable table = new Hashtable();
        table.Add("position", new Vector3(0, 0, 0));
        table.Add("time", 0.3f);
        table.Add("easyType", "easeOutQuart");
        table.Add("oncomplete", "GameStartDeal2");
        table.Add("oncompletetarget", gameObject);
        iTween.MoveTo(GameStartEffect, table);
        SoundManager.PlaySE(SoundManager.SE_Type.Move3);
    }

    private void GameStartDeal2()
    {
        Invoke("GameStartDeal3", 1.0f);
    }

    private void GameStartDeal3()
    {
        Hashtable table = new Hashtable();
        table.Add("position", new Vector3(0, 7.2f, 0));
        table.Add("time", 0.4f);
        table.Add("easyType", "easeOutQuart");
        table.Add("oncomplete", "GameStartDeal4");
        table.Add("oncompletetarget", gameObject);
        iTween.MoveTo(GameStartEffect, table);
    }

    private void GameStartDeal4()
    {
        isGameStart = true;
    }

    private void GoOnDeal()
    {
        isPausePrepare = true;
        CreateBackHashTable(1, Pause);
        CreateBackHashTable(2, GoOn);
        CreateBackHashTable(3, Restart);
        CreateBackHashTable(4, GoBack);
        GoOnText.color = new Color(1, 1, 1);
        RestartText.color = new Color(1, 1, 1);
        GoBackText.color = new Color(1, 1, 1);
    }

    private void CreateBackHashTable(int i,GameObject Target)
    {
        Hashtable PausemoveHash = new Hashtable();
        float x = 11.5f;
        if (i == 1) { x = -13f; }
        float y = -0.15f;
        if (i == 1) { y = 2.4f; }
        PausemoveHash.Add("position", new Vector3(x,y-(i-2)*1.3f, 0));
        PausemoveHash.Add("time", 0.15f);
        PausemoveHash.Add("delay", 0f);
        PausemoveHash.Add("easeType", "easeOutQuart");
        if (i == 1)
        {
            PausemoveHash.Add("oncomplete", "PauseBackFinish");
            PausemoveHash.Add("oncompletetarget", gameObject);
        }
        iTween.MoveTo(Target, PausemoveHash);
    }

    private void PauseBackFinish()
    {
        isPause = false;
        isPausePrepare = false;
        BlackSquare.SetActive(false);
    }

    private void PauseMoveFinish1()
    {
        CreateTextHashTable(0, GoOn);
        CreateTextHashTable(1, Restart);
        CreateTextHashTable(2, GoBack);
        SoundManager.PlaySE(SoundManager.SE_Type.Move2);
    }

    private void CreateTextHashTable(int i,GameObject target)
    {
        Hashtable PausemoveHash = new Hashtable();
        PausemoveHash.Add("position", new Vector3(6.25f,-0.15f-i*1.3f, 0));
        PausemoveHash.Add("time", 0.15f);
        PausemoveHash.Add("delay", 0f);
        PausemoveHash.Add("easeType", "easeOutQuart");
        if (i == 2)
        {
            PausemoveHash.Add("oncomplete", "PauseMoveFinish2");
            PausemoveHash.Add("oncompletetarget", gameObject);
        }
        iTween.MoveTo(target, PausemoveHash);
    }

    private void PauseMoveFinish2()
    {
        isPausePrepare = false;
    }

    public static Player GetPlayer()
    {
        return instance.player;
    }

    private void PauseDeal()
    {
        GoOnText.color = new Color(1, 1, 1);
        RestartText.color = new Color(1, 1, 1);
        GoBackText.color = new Color(1, 1, 1);
        switch (pauseStateNum)
        {
            case 1:
                GoOnText.color = new Color(1, 0, 0);
                break;
            case 2:
                RestartText.color = new Color(1, 0, 0);
                break;
            case 3:
                GoBackText.color = new Color(1, 0, 0);
                break;
            default:
                Debug.Log(pauseStateNum + "が用意されていない");
                break;
        }
        if (Input.GetButtonDown("Up"))
        {
            pauseStateNum--;
            if (pauseStateNum < 1)
            {
                pauseStateNum = 3;
            }
            SoundManager.PlaySE(SoundManager.SE_Type.Move);
        }
        if (Input.GetButtonDown("Down"))
        {
            pauseStateNum++;
            if (pauseStateNum > 3)
            {
                pauseStateNum = 1;
            }
            SoundManager.PlaySE(SoundManager.SE_Type.Move);
        }
        if (Input.GetButtonDown("Fire")&&!isGameChange)
        {
            SoundManager.PlaySE(SoundManager.SE_Type.Decide);
            switch (pauseStateNum)
            {
                case 1:
                    //続ける
                    GoOnDeal();
                    break;
                case 2:
                    //最初から
                    isGameChange = true;
                    SceneChangeManager.SceneChange(SceneEnum.Game,SceneEnum.Game,GameSceneOrganizer.instance.GetStageNum());
                    break;
                case 3:
                    //セレクト画面へ
                    isGameChange = true;
                    SceneChangeManager.SceneChange(SceneEnum.Game, SceneEnum.StageSelect, 1);
                    break;
                default:
                    Debug.Log(pauseStateNum + "がない");
                    break;
            }
        }
    }

    public static void SetSubUnits(GameObject _gameObject)
    {
        _gameObject.transform.parent = instance.SubUnits.transform;
    }

    public static void DestroySubUnits()
    {
        foreach(Transform _transform in instance.SubUnits.transform)
        {
            if (_transform != null)
            {
                Destroy(_transform.gameObject);
            }
        }
    }

    public void ShowBlackSquare()
    {
        BlackSquareForBomb.SetActive(true);
    }
    public void DissableBlackSquare()
    {
        BlackSquareForBomb.SetActive(false);
    }
}
