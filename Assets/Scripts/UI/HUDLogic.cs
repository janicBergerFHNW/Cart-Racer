using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Random = System.Random;

namespace UI
{
    public class HUDLogic : MonoBehaviour
    {
        [SerializeField] private bool player2 = false;
        [SerializeField] private Cart cart;

        private const string EnergyBarName = "CurrentEnergy";
        private const string SpeedArrowName = "SpeedArrow";
        private const string SpeedLabelName = "SpeedLabel";

        private UIDocument _hudUIDocument;

        private VisualElement _energyBar;
        private VisualElement _arrow;
        private Label _speedLabel;

        private Random _rand;
        
        private void OnEnable()
        {
            _rand = new Random();
            
            _hudUIDocument = GetComponent<UIDocument>();
            _energyBar = _hudUIDocument.rootVisualElement.Q<VisualElement>(EnergyBarName);
            _arrow = _hudUIDocument.rootVisualElement.Q<VisualElement>(SpeedArrowName);
            _speedLabel = _hudUIDocument.rootVisualElement.Q<Label>(SpeedLabelName);

            cart.InitialBoostEvent += OnInitialBoost;
        }

        private void OnInitialBoost(object sender, EventArgs e)
        {
            
        }

        private void FixedUpdate()
        {
            _energyBar.style.height = Length.Percent(cart.CurrentEnergyRatio() * 100);
            float speed = cart.Speed;
            _speedLabel.text = (speed*4).ToString("0.0");
            _speedLabel.style.fontSize = 62 + _rand.Next((int)(0.5 * speed));
            _arrow.transform.rotation = Quaternion.Euler(0, 0, (player2 ? 1 : -1) * Math.Min(speed * 2, _rand.Next(100, 102)));
        }
    }
}