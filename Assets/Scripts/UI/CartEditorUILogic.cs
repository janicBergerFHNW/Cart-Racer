using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CartEditorUILogic : MonoBehaviour
{
    private const string PowerSliderName = "Power";
    private const string SteeringSliderName = "Steering";
    private const string WeightSliderName = "Weight";
    private const string BackButtonName = "BackButton";
    private const string ConfirmButtonName = "ConfirmButton";

    public event EventHandler LeaveEditorMenu;

    protected virtual void OnLeaveEditorMenu()
    {
        LeaveEditorMenu?.Invoke(this, EventArgs.Empty);
    }

    private UIDocument _cartEditorUIDocument;
    private float _power;
    private float _steering;
    private float _weight;

    private void OnEnable()
    {
        _cartEditorUIDocument = GetComponent<UIDocument>();

        SliderInt powerSlider = _cartEditorUIDocument.rootVisualElement.Q<SliderInt>(PowerSliderName);
        _power = powerSlider.value;
        powerSlider.RegisterValueChangedCallback(v => _power = v.newValue);
        SliderInt steeringSlider = _cartEditorUIDocument.rootVisualElement.Q<SliderInt>(SteeringSliderName);
        _steering = steeringSlider.value;
        steeringSlider.RegisterValueChangedCallback(v => _steering = v.newValue);
        SliderInt weightSlider = _cartEditorUIDocument.rootVisualElement.Q<SliderInt>(WeightSliderName);
        _weight = weightSlider.value;
        weightSlider.RegisterValueChangedCallback(v => _weight = v.newValue);
        
        _cartEditorUIDocument.rootVisualElement.Q<Button>(BackButtonName).clicked += () =>
        {
            Debug.Log("back clicked");
            OnLeaveEditorMenu();
        };
        _cartEditorUIDocument.rootVisualElement.Q<Button>(ConfirmButtonName).clicked += () =>
        {
            Debug.Log("confirm clicked");
            Debug.Log($"power: {_power}, steering: {_steering}, weight: {_weight}");
            OnLeaveEditorMenu();
        };
    }
}
