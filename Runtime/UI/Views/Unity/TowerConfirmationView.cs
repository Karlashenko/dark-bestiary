using System;
using System.Collections.Generic;
using DarkBestiary.Items;
using DarkBestiary.Managers;
using DarkBestiary.UI.Elements;
using UnityEngine;

namespace DarkBestiary.UI.Views.Unity
{
    public class TowerConfirmationView : View, ITowerConfirmationView
    {
        public event Action ContinueButtonClicked;
        public event Action ReturnToTownButtonClicked;

        [SerializeField] private InventoryItem m_ItemPrefab;
        [SerializeField] private Transform m_ItemContainer;
        [SerializeField] private Interactable m_ContinueButton;
        [SerializeField] private Interactable m_ReturnToTownButton;

        public void Construct(List<Item> items)
        {
            foreach (var item in items)
            {
                var itemView = Instantiate(m_ItemPrefab, m_ItemContainer);
                itemView.Change(item);
                itemView.IsDraggable = false;
            }
        }

        protected override void OnInitialize()
        {
            m_ContinueButton.PointerClick += OnContinueButtonPointerClick;
            m_ReturnToTownButton.PointerClick += OnReturnToTownButtonPointerClick;
        }

        protected override void OnTerminate()
        {
            m_ContinueButton.PointerClick -= OnContinueButtonPointerClick;
            m_ReturnToTownButton.PointerClick -= OnReturnToTownButtonPointerClick;
        }

        private void OnContinueButtonPointerClick()
        {
            if (SettingsManager.Instance.Data.DoNotShowTowerConfirmation)
            {
                ContinueButtonClicked?.Invoke();
                return;
            }

            ConfirmationWindowWithCheckbox.Instance.Cancelled += OnConfirmationCancelled;
            ConfirmationWindowWithCheckbox.Instance.Confirmed += OnConfirmationConfirmed;
            ConfirmationWindowWithCheckbox.Instance.Show(
                I18N.Instance.Get("ui_confirm_tower"),
                I18N.Instance.Get("ui_confirm")
            );
        }

        private void OnReturnToTownButtonPointerClick()
        {
            ReturnToTownButtonClicked?.Invoke();
        }

        private void OnConfirmationConfirmed(bool doNotShowAgain)
        {
            ContinueButtonClicked?.Invoke();
            OnConfirmationCancelled(doNotShowAgain);
        }

        private void OnConfirmationCancelled(bool doNotShowAgain)
        {
            SettingsManager.Instance.Data.DoNotShowTowerConfirmation = doNotShowAgain;

            ConfirmationWindowWithCheckbox.Instance.Cancelled -= OnConfirmationCancelled;
            ConfirmationWindowWithCheckbox.Instance.Confirmed -= OnConfirmationConfirmed;
        }
    }
}