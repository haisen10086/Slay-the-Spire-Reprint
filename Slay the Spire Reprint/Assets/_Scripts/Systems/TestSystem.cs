using System.Collections.Generic;
using UnityEngine;

public class TestSystem : MonoBehaviour
{
    [SerializeField] private List<CardDataSO> deckData;
    private void Start()
    {
        CardSystem.Instance.SetUp(deckData);
    }
}
