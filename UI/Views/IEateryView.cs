using System.Collections.Generic;
using DarkBestiary.Messaging;

namespace DarkBestiary.UI.Views
{
    public interface IEateryView : IView, IHideOnEscape
    {
        event Payload<Food> FoodSelected;
        event Payload<FoodType> FoodRemoved;
        event Payload BuyButtonClicked;

        void Construct(IReadOnlyDictionary<FoodType, Food> foodSlots, List<Food> assortment);

        void UpdateSlots(IReadOnlyDictionary<FoodType, Food> foodSlots);

        void UpdatePrice(int price, bool isTooExpensive);
    }
}