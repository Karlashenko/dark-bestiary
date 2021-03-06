using DarkBestiary.Messaging;
using UnityEngine;

namespace DarkBestiary.UI.Elements
{
    public class ArrowSelect : MonoBehaviour
    {
        public event Payload<int> Changed;

        [SerializeField] private Interactable leftArrow;
        [SerializeField] private Interactable rightArrow;

        private int index;
        private int count;

        private void Start()
        {
            this.leftArrow.PointerUp += OnLeftArrowPointerUp;
            this.rightArrow.PointerUp += OnRightArrowPointerUp;
        }

        public void Initialize(int count)
        {
            this.count = count;
        }

        public void Random()
        {
            this.index = RNG.Range(0, this.count - 1);
            Changed?.Invoke(this.index);
        }

        private void OnLeftArrowPointerUp()
        {
            this.index--;

            if (this.index < 0)
            {
                this.index = this.count - 1;
            }

            Changed?.Invoke(this.index);
        }

        private void OnRightArrowPointerUp()
        {
            this.index++;

            if (this.index > this.count - 1)
            {
                this.index = 0;
            }

            Changed?.Invoke(this.index);
        }
    }
}