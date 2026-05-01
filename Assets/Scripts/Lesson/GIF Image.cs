using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GIFImage : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private float _intervalMS = 3000f;
    private int _currentIndex;
    private CancellationTokenSource _cts;
    
    private void Start()
    {
        if (_sprites == null || _sprites.Length == 0)
        {
            Debug.LogError("Sprites array is null or empty");
            return;
        }
        
        _image.sprite = _sprites[0];

        _cts = new CancellationTokenSource();
        FlipThroughSprites(_cts.Token).Forget();
    }

    private async UniTask FlipThroughSprites(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            await UniTask.Delay(TimeSpan.FromMilliseconds(_intervalMS), cancellationToken: token);
            _currentIndex++;
            if (_currentIndex >= _sprites.Length) _currentIndex = 0;
            _image.sprite = _sprites[_currentIndex];
        }
    }

    private void OnDestroy()
    {
        _cts?.Cancel();
        _cts?.Dispose();
    }
}
