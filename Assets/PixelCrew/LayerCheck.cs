using System;
using UnityEngine;

public class LayerCheck : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayer;

    private Collider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    public bool IsTouchingLayer;

    private void OnTriggerStay2D(Collider2D other)
    {
        //Проверяем соприкасается ли коллайдер с указанными слоями
        IsTouchingLayer = _collider.IsTouchingLayers(_groundLayer); // проверяем если мы стоим на каком то слое, то мы можем проверить что наш коллайдер соприкасается с указанными нами слоями.
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //Проверяем соприкасается ли коллайдер с указанными слоями
        IsTouchingLayer = _collider.IsTouchingLayers(_groundLayer);
    }
}
