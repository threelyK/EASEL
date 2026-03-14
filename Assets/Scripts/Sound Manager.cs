using Inventory;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public float volume = 1.0f;
    
    [SerializeField] 
    private AudioSource _pouchAudioSrc;
    [SerializeField]
    private AudioClip _pouchInsertClip;
    
    
    private void PlayPouchInsert(ItemSO _)
    {
        _pouchAudioSrc.PlayOneShot(_pouchInsertClip, volumeScale: volume);
    }

    #region Unity Methods

    private void OnEnable()
    {
        InventoryManager.OnItemPickup += PlayPouchInsert;
    }

    private void OnDisable()
    {
        InventoryManager.OnItemPickup -= PlayPouchInsert;
    }

    #endregion
}
