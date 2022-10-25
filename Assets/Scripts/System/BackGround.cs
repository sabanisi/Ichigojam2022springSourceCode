using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    [SerializeField] private List<Transform> TFs = new List<Transform>();
    [SerializeField] private GameObject Back1;
    [SerializeField] private GameObject Back2;
    [SerializeField] private GameObject Back3;
    [SerializeField] private GameObject Back4;
    [SerializeField] private List<float> edges = new List<float>();
    private int stageNum;

    void Start()
    {
    }

    public void Initialize(int _stageNum)
    {
        stageNum = _stageNum;
        SetBack();
        TFs[stageNum - 1].position = new Vector3(0, 0, 0);
    }

    private void SetBack()
    {
        Back1.SetActive(false);
        Back2.SetActive(false);
        Back3.SetActive(false);
        Back4.SetActive(false);
        switch (stageNum)
        {
            case 1:
                Back1.SetActive(true);
                break;
            case 2:
                Back2.SetActive(true);
                break;
            case 3:
                Back3.SetActive(true);
                break;
            case 4:
                Back4.SetActive(true);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.IsGameOver()) return;
        if (GameManager.instance.IsPause()) return;

        TFs[stageNum-1].position -= new Vector3(2, 0, 0) * Time.deltaTime;
        if (TFs[stageNum-1].position.x <= edges[stageNum-1])
        {
            TFs[stageNum-1].position -= new Vector3(edges[stageNum-1], 0, 0);
        }
    }
}
