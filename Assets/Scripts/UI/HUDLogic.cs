using System;
using System.Collections;
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

        private const string EnergyBarContainerName = "EnergyMeter";
        private const string EnergyBarName = "CurrentEnergy";
        private const string SpeedArrowName = "SpeedArrow";
        private const string SpeedLabelName = "SpeedLabel";
        private const string EnergyWarningName = "EnergyWarning";
        private const string WholeHalfContainerName = "Container";

        private UIDocument _hudUIDocument;

        private VisualElement _energyBarContainer;
        private VisualElement _energyBar;
        private VisualElement _arrow;
        private VisualElement _energyWarning;
        private VisualElement _container;
        private Label _speedLabel;

        private Random _rand;
        
        private void OnEnable()
        {
            _rand = new Random();
            
            _hudUIDocument = GetComponent<UIDocument>();
            _energyBarContainer = _hudUIDocument.rootVisualElement.Q<VisualElement>(EnergyBarContainerName);
            _energyBar = _hudUIDocument.rootVisualElement.Q<VisualElement>(EnergyBarName);
            _arrow = _hudUIDocument.rootVisualElement.Q<VisualElement>(SpeedArrowName);
            _speedLabel = _hudUIDocument.rootVisualElement.Q<Label>(SpeedLabelName);
            _energyWarning = _hudUIDocument.rootVisualElement.Q<VisualElement>(EnergyWarningName);
            _container = _hudUIDocument.rootVisualElement.Q<VisualElement>(WholeHalfContainerName);

            cart.InitialBoostEvent += OnInitialBoost;
            cart.DeathEvent += OnDeath;
        }

        private void OnDeath(object sender, EventArgs e)
        {
            IEnumerator Tint()
            {
                var c = Color.red;
                c.a = 0.4f;
                _container.style.backgroundColor = new StyleColor(c);
                yield return new WaitForSeconds(1.2f);
                c.a = 0;
                _container.style.backgroundColor = new StyleColor(c);
            }

            StartCoroutine(Tint());
        }

        private void OnInitialBoost(object sender, Cart.InitialBoostEventArgs e)
        {
            if (!e.Success)
            {
                StartCoroutine(HudWarning(_energyWarning));
            }
            StartCoroutine(Shake(_energyBarContainer));
        }

        private IEnumerator HudWarning(VisualElement element)
        {
            Color red = Color.red;
            
            red.a = 0.5f;
            element.style.backgroundColor = new StyleColor(red);
            yield return new WaitForSeconds(0.2f);
            red.a = 0.2f;
            element.style.backgroundColor = new StyleColor(red);
            yield return new WaitForSeconds(0.2f);
            red.a = 0.5f;
            element.style.backgroundColor = new StyleColor(red);
            yield return new WaitForSeconds(0.2f);

            red.a = 0;
            element.style.backgroundColor = new StyleColor(red);
        }

        private IEnumerator Shake(VisualElement element)
        {
            var t = _energyBarContainer.transform;
            for (int i = 3; i < 27; i++)
            {
                if (i % 12 < 6)
                {
                    t.position += Vector3.right;
                }
                else
                {
                    t.position += Vector3.left;
                }

                yield return null;
            }
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