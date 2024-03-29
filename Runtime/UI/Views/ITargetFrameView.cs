using System.Collections.Generic;
using DarkBestiary.Behaviours;

namespace DarkBestiary.UI.Views
{
    public interface ITargetFrameView : IView
    {
        void SetPoisoned(bool isPoisoned);
        void ChangeNameText(string text, bool isEnemy);
        void ChangeChallengeRatingText(string text);
        void RefreshHealth(float currentHealth, float currentShield, float maximum);
        void AddBehaviour(Behaviour behaviour);
        void RemoveBehaviour(Behaviour behaviour);
        void ClearBehaviours();
        void CreateAffixes(List<Behaviour> behaviours);
        void ClearAffixes();
    }
}