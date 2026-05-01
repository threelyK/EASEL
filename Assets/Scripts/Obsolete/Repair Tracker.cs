using Bots.Components;
using UnityEngine;
using UnityEngine.Events;

public class RepairTracker : MonoBehaviour
{
    private const int amountNeeded = 3;
    private int currentAmount;

    public UnityEvent OnAmountReached;

    private void OnEnable()
    {
        WIPBot.OnBotRepaired += incAndCheck;
    }

    private void OnDisable()
    {
        WIPBot.OnBotRepaired -= incAndCheck;
    }

    private void incAndCheck(int id)
    {
        currentAmount++; 
        if (currentAmount >= amountNeeded) OnAmountReached?.Invoke();
    }
}
