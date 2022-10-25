using UnityEngine;
using System.Collections;

public class SceneChangeManager : MonoBehaviour
{
    public static SceneChangeManager instance { get; private set; }


    [SerializeField] private GameSceneOrganizer gameSceneOrganizer;
    [SerializeField] private TitleSceneOrganizer titleSceneOrganizer;
    [SerializeField] private StageSelectOrganizer stageSelectOrganizer;

    [SerializeField] private Transform CurtainTF;
    [SerializeField] private GameObject Curtain;
    private SceneEnum nowScene;

    private bool isChange;

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
        SceneChange(SceneEnum.None,SceneEnum.Title,2);
    }

    public static void SceneChange(SceneEnum fromSceneEnum,SceneEnum toSceneEnum,int num)
    {
        if (instance.isChange) return;
        instance.StartCoroutine(instance.SceneChangeDeal(fromSceneEnum, toSceneEnum, num));
    }

    private IEnumerator SceneChangeDeal(SceneEnum fromSceneEnum, SceneEnum toSceneEnum, int num)
    {
        isChange = true;
        if (fromSceneEnum != SceneEnum.None)
        {
            CurtainMove(0, 0.5f, 0.2f, "easeInCubic");
            yield return new WaitForSeconds(1.0f);
            switch (fromSceneEnum)
            {
                case SceneEnum.Game:
                    gameSceneOrganizer.DestroyDeal();
                    break;
                case SceneEnum.Title:
                    break;
                case SceneEnum.StageSelect:
                    break;
                default:
                    break;
            }
            
        }
        titleSceneOrganizer.gameObject.SetActive(false);
        gameSceneOrganizer.gameObject.SetActive(false);
        stageSelectOrganizer.gameObject.SetActive(false);
        CurtainMove(10f, 0.5f, 0f, "easeOutCubic");
        switch (toSceneEnum)
        {
            case SceneEnum.Game:
                gameSceneOrganizer.gameObject.SetActive(true);
                gameSceneOrganizer.Initiazlie(num);
                break;
            case SceneEnum.Title:
                titleSceneOrganizer.gameObject.SetActive(true);
                titleSceneOrganizer.Initialize();
                break;
            case SceneEnum.StageSelect:
                stageSelectOrganizer.gameObject.SetActive(true);
                stageSelectOrganizer.Initialize();
                break;
            default:
                Debug.Log(toSceneEnum + " がない");
                break;
        }
        isChange = false;
    }

    private void CurtainMove(float toY,float time,float delay,string easeType)
    {
        Hashtable moveTable = new Hashtable();
        moveTable.Add("position", new Vector3(0, toY, 1));
        moveTable.Add("time", time);
        moveTable.Add("delay", delay);
        moveTable.Add("easyType", easeType);
        iTween.MoveTo(Curtain, moveTable);
    }
}
