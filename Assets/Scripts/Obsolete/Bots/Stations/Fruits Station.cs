using DG.Tweening;

namespace Bots.Stations
{
    public class FruitsStation: Station
    {
        public StationType stationType = StationType.FRUIT;
        private protected override void ExecuteStationProcess()
        {
            var heightChange = 1f;
            var duration = 1f;
            transform.DOMoveY(transform.localPosition.y + heightChange, duration).SetEase(Ease.InSine)
                .OnComplete(() => Destroy(gameObject));
        }
    }
}
