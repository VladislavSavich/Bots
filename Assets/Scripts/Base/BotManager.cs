using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotManager : MonoBehaviour
{
    [SerializeField] private BotSpawner _spawner;

    private List<Bot> _bots = new List<Bot>();
    private WaitForSeconds _delay = new WaitForSeconds(0.1f);
    private Coroutine _waitingCoroutine;
    private Bot _freeBot;

    private void Start()
    {
        StartCoroutine(SearchingFreeBot());
    }

    public void StartTask(Barrel barrel)
    {
        if (_freeBot != null && !_freeBot.IsActive)
        {
            _freeBot.SetTarget(barrel);
        }
        else
        {
            if (_waitingCoroutine != null)
                StopCoroutine(_waitingCoroutine);

            _waitingCoroutine = StartCoroutine(WaitingForBot(barrel));
        }
    }

    private IEnumerator WaitingForBot(Barrel barrel)
    {
        while (_freeBot == null)
        {
            yield return _delay;
        }

        if (!_freeBot.IsActive)
        {
            _freeBot.SetTarget(barrel);
            _freeBot = null;
        }
    }

    private IEnumerator SearchingFreeBot()
    {
        while (enabled)
        {
            yield return _delay;

            _bots = _spawner.GetAllBots();

            for (int i = 0; i < _bots.Count; i++)
            {
                if (!_bots[i].IsActive)
                {
                    _freeBot = _bots[i];
                }
            }
        }
    }
}
