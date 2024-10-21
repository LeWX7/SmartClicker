using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace SmartClicker.Services
{
    public static class AnimationService
    {
        public static async Task ButtonClickAnimation(VisualElement element)
        {
            if (element != null)
            {
                await element.ScaleTo(0.95, 50, Easing.CubicIn);
                await element.ScaleTo(1.0, 50, Easing.CubicOut);
            }
        }
    }
}
