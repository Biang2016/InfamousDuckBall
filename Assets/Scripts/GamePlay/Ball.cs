using System.Collections;
using Bolt;
using DG.Tweening;
using UnityEngine;

public class Ball : EntityEventListener<IBallState>
{
    public Animator Anim;
    public Collider Collider;
    public Rigidbody RigidBody;
    internal Transform ResetTransform;

    public Transform SOS_BubbleContainer;
    public SpriteRenderer SOS_Bubble;

    public GameObject Model;

    private Vector3 defaultScale;

    private string ballName;

    public string BallName
    {
        get
        {
            if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
            {
                return state.BallName;
            }
            else
            {
                return ballName;
            }
        }
        set
        {
            if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
            {
                state.BallName = value;
            }
            else
            {
                ballName = value;
            }
        }
    }

    void Awake()
    {
        defaultScale = Model.gameObject.transform.localScale;
    }

    void OnTriggerEnter(Collider c)
    {
        if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Local || BoltNetwork.IsServer)
        {
            if (c.gameObject.GetComponent<GoalCollider>())
            {
                Player p = c.GetComponentInParent<Player>();
                if (PlayerObjectRegistry_Online.MyPlayer == p)
                {
                    //Todo Vibrate
                }

                GameManager.Instance.Cur_BallBattleManager.BallHit_Server(this, p, p.TeamNumber);
                HideSOSBubble_Server();
            }

            ScoreRingSingle srs = c.gameObject.GetComponentInParent<ScoreRingSingle>();
            if (srs)
            {
                srs.Explode(true);
                KickedFly();
            }
        }
    }

    void OnCollisionEnter(Collision c)
    {
        if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Local || BoltNetwork.IsServer)
        {
            GameManager.Instance.SendSFXEvent(AudioDuck.Instance.FishFlapping);
        }
    }

    public override void Attached()
    {
        state.SetTransforms(state.Transform, transform);
    }

    void Update()
    {
        if (GameManager.Instance.Cur_BattleManager != null)
        {
            if (GameManager.Instance.Cur_BattleManager is BattleManager_Smash smash)
            {
                if (smash.Ball == null)
                {
                    if (BallName == "SmashBall")
                    {
                        smash.Ball = this;
                    }
                }
            }

            if (GameManager.Instance.Cur_BattleManager is BattleManager_FlagRace flagRace)
            {
                if (flagRace.LeftBall == null)
                {
                    if (BallName == "FlagRaceBall_Left")
                    {
                        flagRace.LeftBall = this;
                    }
                }

                if (flagRace.RightBall == null)
                {
                    if (BallName == "FlagRaceBall_Right")
                    {
                        flagRace.RightBall = this;
                    }
                }
            }

            if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Local || BoltNetwork.IsServer)
            {
                noTouchTick += Time.deltaTime;
                if (noTouchTick > NoTouchDurationBeforeSOS)
                {
                    if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
                    {
                        BallSOSEvent evnt = BallSOSEvent.Create();
                        evnt.BallName = BallName;
                        evnt.Shown = true;
                        evnt.Send();
                    }
                    else
                    {
                        Battle_All_Callbacks.OnEvent_BallSOSEvent(true, BallName);
                    }

                    noTouchTick = 0f;
                }
            }
        }
    }

    private float NoTouchDurationBeforeSOS = 3f;
    private float noTouchTick = 0f;

    public void KickedFly()
    {
        GameManager.Instance.SendSFXEvent(AudioDuck.Instance.FishBreath);
        transform.DOMoveY(10f, 0.3f);
        RigidBody.AddForce(Vector3.up * 800f);
        HideSOSBubble_Server();
    }

    public void ResetBall()
    {
        if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Local || BoltNetwork.IsServer)
        {
            StartCoroutine(Co_ResetBall(1f));
        }
    }

    IEnumerator Co_ResetBall(float suspendingTime)
    {
        HideSOSBubble_Server();
        RigidBody.DOPause();
        transform.position = ResetTransform.position;
        RigidBody.velocity = Vector3.zero;
        RigidBody.angularVelocity = Vector3.zero;
        RigidBody.useGravity = false;
        yield return new WaitForSeconds(suspendingTime);
        RigidBody.useGravity = true;
    }

    public void Kick(TeamNumber teamNumber, Vector3 force)
    {
        if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Local || BoltNetwork.IsServer)
        {
            RigidBody.AddForce(force);
            HideSOSBubble_Server();
        }
    }

    private void HideSOSBubble_Server()
    {
        noTouchTick = 0;

        if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
        {
            BallSOSEvent evnt = BallSOSEvent.Create();
            evnt.BallName = BallName;
            evnt.Shown = false;
            evnt.Send();
        }
        else
        {
            Battle_All_Callbacks.OnEvent_BallSOSEvent(false, BallName);
        }
    }

    public void SetSOSBubbleShown_Client(bool shown)
    {
        if (ShowSOSBubbleCoroutine != null)
        {
            StopCoroutine(ShowSOSBubbleCoroutine);
            transform.localScale = Vector3.one;
        }

        if (shown)
        {
            ShowSOSBubbleCoroutine = StartCoroutine(Co_ShowSOSBubble(3f));
        }
        else
        {
            SOS_Bubble.enabled = false;
        }
    }

    private Coroutine ShowSOSBubbleCoroutine;

    IEnumerator Co_ShowSOSBubble(float duration)
    {
        SOS_Bubble.enabled = true;
        for (int i = 0; i < 5; i++)
        {
            Model.transform.DOScale(Vector3.one * (1 + 0.1f * i), duration / 10f);
            yield return new WaitForSeconds(duration / 10f);
            Model.transform.DOScale(Vector3.one * (1 + 0.1f * i - 0.05f), duration / 10f);
            yield return new WaitForSeconds(duration / 10f);
        }

        SOS_Bubble.enabled = false;
        noTouchTick = 5f;
    }

    void LateUpdate()
    {
        SOS_BubbleContainer.up = Vector3.up;
        if (GameManager.Instance.Cur_BattleManager)
        {
            SOS_BubbleContainer.LookAt(GameManager.Instance.Cur_BattleManager.BattleCamera.transform, Vector3.up);
        }

        Model.transform.localScale = defaultScale * Mathf.Clamp((transform.position.y - 0.7f) / GameManager.Instance.temp, 1f, 1.5f);
        if (transform.position.y > 5f)
        {
            Anim.SetTrigger("Fly");
        }
        else
        {
            Anim.SetTrigger("Calm");
        }
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Local || BoltNetwork.IsServer)
        {
            if (transform.position.y > 5f)
            {
                transform.up = Vector3.up;
                transform.right = -RigidBody.velocity.normalized;
            }
        }
    }
}