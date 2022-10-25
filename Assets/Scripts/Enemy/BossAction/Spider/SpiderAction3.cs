using UnityEngine;

public class SpiderAction3:AbstractBossAction
{
    private Transform _bossTF;
    private Transform playerTF;

    private ActionState state;
    private enum ActionState
    {
        Up,Down,Attack
    }
    private float timeCount;
    private bool isLaugh;
    private int attackCount;

    public SpiderAction3(Boss parent, Transform bossTF) : base(parent)
    {
        _bossTF = bossTF;
        state = ActionState.Up;
        playerTF = GameManager.GetPlayer().transform;
    }

    public override void Update()
    {
        switch (state)
        {
            case ActionState.Up:
                UpAct();
                break;
            case ActionState.Down:
                DownAct();
                break;
            case ActionState.Attack:
                AttackAct();
                break;
        }
    }

    private void UpAct()
    {
        _bossTF.position += new Vector3(0, 9, 0) * Time.deltaTime;
        if (_bossTF.position.y >= 7.5f)
        {
            state = ActionState.Down;
        }
    }

    private void DownAct()
    {
        if (!isLaugh)
        {
            isLaugh = true;
            SoundManager.PlaySE(SoundManager.SE_Type.Laugh);
            _bossTF.position = new Vector3(playerTF.position.x, 7.5f, 0);
        }
        timeCount += Time.deltaTime;
        if (timeCount >= 0.5f)
        {
            _bossTF.position -= new Vector3(0, 12, 0) * Time.deltaTime;
            if (_bossTF.position.y <= -2.8f)
            {
                timeCount = 0;
                isLaugh = false;
                state = ActionState.Attack;
            }
        }
    }

    private void AttackAct()
    {
        timeCount += Time.deltaTime;
        if (timeCount >= 0.06f)
        {
            timeCount = 0;
            attackCount++;
            float randomRotate= Random.Range(0, 180);
            for (int i = 0; i <= 2; i++)
            {
                Bullet bullet = BulletManager.CreateBullet(BulletBelongEnum.Enemy, BulletImageEnum.Orange2, TrajectoryEnum.Straight, new Vector3(_bossTF.position.x, -4.8f, 0));
                float rotate = (randomRotate + (i - 1) * 5.0f) * Mathf.Deg2Rad;
                BulletManager.SetUpStraightBullet(bullet, 5 * Mathf.Cos(rotate), 5 * Mathf.Sin(rotate));
            }
            
            if (attackCount >= 50)
            {
                attackCount = 0;
                state = ActionState.Up;
            }
            SoundManager.PlaySE(SoundManager.SE_Type.Shot4);
        }
    }
}
