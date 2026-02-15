using AxGrid;
using AxGrid.Base;
using AxGrid.Model;
using LootBox.Models;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LootBox.UI.View
{
    public class LooBoxView : MonoBehaviourExtBind
    {
        [Header("Reel UI")]
        [SerializeField] private RectTransform _content;
        [SerializeField] private float _slotsYSpacing;

        [SerializeField] private Image _topImage;
        [SerializeField] private Image _midImage;
        [SerializeField] private Image _bottomImage;

        [SerializeField] private List<Sprite> _symbols = new();

        [Header("Motion")]
        [SerializeField] private float _slotHeight = 210f;
        [SerializeField] private float _maxSpeed = 950f;
        [SerializeField] private float _acceleration = 1800f;
        [SerializeField] private float _deceleration = 1500f;
        [SerializeField] private float _snapDuration = 0.12f;

        private readonly List<Image> _images = new(3);

        private ReelMode _mode = ReelMode.Idle;
        private float _speed;
        private float _offset;

        private float _snapTimer;
        private float _snapOffsetFrom;
        private int _targetCenterIndex;

        private int _symbolCursor;

        [OnAwake]
        private void Init()
        {
            _topImage.rectTransform.anchoredPosition = new Vector2(0, _slotHeight + _slotsYSpacing);
            _midImage.rectTransform.anchoredPosition = Vector2.zero;
            _bottomImage.rectTransform.anchoredPosition = new Vector2(0, -_slotHeight - _slotsYSpacing);

            SetAt(_images, 0, _topImage);
            SetAt(_images, 1, _midImage);
            SetAt(_images, 2, _bottomImage);

            FillInitialSymbols();
            ApplyLayout();
        }

        [OnUpdate]
        private void Tick()
        {
            if (_mode == ReelMode.Idle)
                return;

            UpdateSpeed(Time.deltaTime);
            Move(Time.deltaTime);

            if (_mode == ReelMode.Snapping)
                UpdateSnap(Time.deltaTime);
        }

        [Bind(LootBoxSignals.ViewSpinStart)]
        private void OnSpinStart() => _mode = ReelMode.Accelerating;

        [Bind(LootBoxSignals.ViewAllowStop)]
        private void OnStopAllowed()
        {
            if (_mode == ReelMode.Accelerating)
                _mode = ReelMode.Spinning;
        }

        [Bind(LootBoxSignals.ViewSpinStop)]
        private void OnSpinStop()
        {
            _targetCenterIndex = _symbols.Count == 0 ? 0 : Random.Range(0, _symbols.Count);
            _mode = ReelMode.Decelerating;
        }

        private void SetAt<T>(List<T> list, int index, T value)
        {
            while (list.Count <= index)
                list.Add(default);
            list[index] = value;
        }

        private void UpdateSpeed(float deltaTime)
        {
            switch (_mode)
            {
                case ReelMode.Accelerating:
                    _speed = Mathf.MoveTowards(_speed, _maxSpeed, _acceleration * deltaTime);
                    if (Mathf.Approximately(_speed, _maxSpeed))
                        _mode = ReelMode.Spinning;
                    break;
                case ReelMode.Spinning:
                    _speed = _maxSpeed;
                    break;
                case ReelMode.Decelerating:
                    _speed = Mathf.MoveTowards(_speed, 0f, _deceleration * deltaTime);
                    if (_speed <= 0.001f)
                        BeginSnap();
                    break;
                case ReelMode.Snapping:
                    _speed = 0f;
                    break;
            }
        }

        private void Move(float deltaTime)
        {
            if(_speed <= 0f)
            {
                ApplyLayout();
                return;
            }

            _offset += _speed * deltaTime;

            while(_offset >= _slotHeight)
            {
                _offset -= _slotHeight;
                ShiftDownOneStep();
            }

            ApplyLayout();
        }

        private void BeginSnap()
        {
            _mode = ReelMode.Snapping;
            _snapTimer = 0f;
            _snapOffsetFrom = _offset;

            if (_symbols.Count > 0)
                SetCenterSymbol(_targetCenterIndex);
        }

        private void UpdateSnap(float deltaTime)
        {
            _snapTimer += deltaTime;
            var t = Mathf.Clamp01(_snapTimer / _snapDuration);
            var eased = 1f - Mathf.Pow(1f - t, 3f);

            _offset = Mathf.Lerp(_snapOffsetFrom, 0f, eased);
            ApplyLayout();

            if (t < 1f)
                return;

            _offset = 0f;
            _speed = 0f;
            _mode = ReelMode.Idle;
            ApplyLayout();

            Settings.Invoke(LootBoxSignals.ViewSpinStopped, _targetCenterIndex);
        }

        private void FillInitialSymbols()
        {
            if (_symbols.Count == 0)
                return;

            _images[0].sprite = NextSymbol();
            _images[1].sprite = NextSymbol();
            _images[2].sprite = NextSymbol();
        }

        private void ShiftDownOneStep()
        {
            _images[2].sprite = _images[1].sprite;
            _images[1].sprite = _images[0].sprite;
            _images[0].sprite = NextSymbol();
        }

        private Sprite NextSymbol()
        {
            if (_symbols.Count == 0)
                return null;

            var sprite = _symbols[_symbolCursor % _symbols.Count];
            _symbolCursor++;
            return sprite;
        }

        private void SetCenterSymbol(int centerIndex)
        {
            centerIndex = Mathf.Clamp(centerIndex, 0, _symbols.Count - 1);

            _images[1].sprite = _symbols[centerIndex];
            _images[0].sprite = _symbols[(centerIndex + 1) % _symbols.Count];
            _images[2].sprite = _symbols[(centerIndex - 1 + _symbols.Count) % _symbols.Count];
        }

        private void ApplyLayout()
        {
            _content.anchoredPosition = new Vector2( _content.anchoredPosition.x, -_offset);
        }
    }
}