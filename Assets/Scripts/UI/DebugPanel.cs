using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugPanel : BaseUIForm
{
    public Toggle ConfigRowsToggle;
    public ConfigRows ConfigRows;

    void Awake()
    {
        UIType.InitUIType(
            isClearStack: false,
            isESCClose: false,
            isClickElsewhereClose: false,
            uiForms_Type: UIFormTypes.Fixed,
            uiForms_ShowMode: UIFormShowModes.Normal,
            uiForm_LucencyType: UIFormLucencyTypes.Penetrable);
        ConfigRowsToggle.onValueChanged.AddListener(SetConfigPanelShown);
    }

    public Text fpsText;
    private float deltaTime;

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = Mathf.Ceil(fps).ToString();
    }

    public void SetConfigPanelShown(bool shown)
    {
        ConfigRows.gameObject.SetActive(shown);
    }
}