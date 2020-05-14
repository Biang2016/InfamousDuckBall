﻿using UnityEngine;
using System.Collections;
using TMPro;

public class DuckUI : MonoBehaviour
{
    internal Duck Duck;
    internal Player Player => Duck.Player;
    internal DuckConfig DuckConfig => Duck.DuckConfig;

    public void Attached()
    {
        Duck = GetComponentInParent<Duck>();
    }

    [SerializeField] private SpriteRenderer Arrow;
    [SerializeField] private TextMeshPro PlayerName;
    [SerializeField] private SpriteRenderer Bubble_Annoying;
    [SerializeField] private SpriteRenderer Bubble_Max;

    [SerializeField] private Sprite[] TeamArrowSprites;

    public void Initialize()
    {
        Arrow.sprite = TeamArrowSprites[(int) Player.TeamNumber];
        Bubble_Annoying.enabled = false;
        Bubble_Max.enabled = false;
    }

    void Update()
    {
        if (Player && Player.entity && Player.entity.IsAttached && Player.state != null)
        {
            PlayerName.text = Player.state.PlayerInfo.PlayerName;
        }
    }

    private Coroutine ShowAnnoyingUICoroutine;

    public void ShowAnnoyingUI()
    {
        if (ShowAnnoyingUICoroutine != null)
        {
            StopCoroutine(ShowAnnoyingUICoroutine);
        }

        ShowAnnoyingUICoroutine = StartCoroutine(Co_ShowAnnoyingUI(1.2f));
    }

    IEnumerator Co_ShowAnnoyingUI(float duration)
    {
        Bubble_Annoying.enabled = true;
        yield return new WaitForSeconds(duration);
        Bubble_Annoying.enabled = false;
    }

    private Coroutine ShowMaxUICoroutine;

    public void ShowMaxUI()
    {
        if (ShowMaxUICoroutine != null)
        {
            StopCoroutine(ShowMaxUICoroutine);
        }

        ShowMaxUICoroutine = StartCoroutine(Co_ShowMaxUI(0.5f));
    }

    IEnumerator Co_ShowMaxUI(float duration)
    {
        Bubble_Max.enabled = true;
        yield return new WaitForSeconds(duration);
        Bubble_Max.enabled = false;
    }

    void LateUpdate()
    {
        transform.position = Duck.Feet.transform.position;
    }
}