using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

namespace Aaron.Toolkit.Graphics.UserInterface
{
    /// <summary>
    /// Class containing logic for a basic textbox. Inherit from this class to customize it
    /// </summary>
    public class Textbox : InteractableContainer
    {
        protected const float KEY_REPEAT_DELAY = 0.5f;

        protected const float KEY_REPEAT_SPEED = 0.05f;
        
        [SerializeField, Header("Textbox")]
        protected TMP_Text m_TextDisplay;
        protected TMP_TextInfo textInfo;
        
        [SerializeField]
        protected TMP_Text m_PlaceholderTextDisplay;

        [SerializeField]
        protected RectTransform m_MaskRect;
        
        [SerializeField]
        // Container m_Caret;
        protected Container m_Caret;
        
        [SerializeField]
        protected float smoothDamping = 5f;

        [SerializeField]
        protected RectTransform m_SelectionRect;
        
        [SerializeField]
        protected string m_Text;
        
        protected (Key, Action)[] repeatKeys;

        protected bool mouseIsDragging = false;
        protected float pivotOffset;
        protected int m_CaretIndex;
        protected Vector2 caretAnchor = new();

        protected Vector2Int m_SelectionIndices;
        protected int SelectionLength => Mathf.Abs(SelectionIndices.x - SelectionIndices.y);
        protected int SelectionMin => Mathf.Min(SelectionIndices.x, SelectionIndices.y);
        protected int SelectionMax => Mathf.Max(SelectionIndices.x, SelectionIndices.y);

        public string Text
        {
            get => m_Text;
            set
            {
                m_Text = value;
                UpdateDisplayText();
            }
        }

        protected int CaretIndex
        {
            get => m_CaretIndex;
            set
            {
                m_CaretIndex = Mathf.Clamp(value, -1, Text.Length - 1);
                UpdateCaretAnchor();
            }
        }

        /// <summary>
        /// X -> Anchor Index
        /// Y -> Active Index
        /// </summary>
        protected Vector2Int SelectionIndices
        {
            get => m_SelectionIndices;
            set
            {
                int x = Mathf.Clamp(value.x, -1, Text.Length - 1);
                int y = Mathf.Clamp(value.y, -1, Text.Length - 1);

                m_SelectionIndices = new(x, y);

                UpdateSelectionDisplay();
            }
        }

        protected override void Start()
        {
            base.Start();

            if (!Application.isPlaying)
                return;

            textInfo = m_TextDisplay.textInfo;
            
            repeatKeys = new (Key, Action)[]
            {
                (Key.LeftArrow, () => MoveCaret(-1, Keyboard.current.shiftKey.isPressed)),
                (Key.RightArrow, () => MoveCaret(1, Keyboard.current.shiftKey.isPressed)),
                (Key.Delete, () => DeleteKey())
            };
        }

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();

            if (!Application.isPlaying)
                return;
            
            pivotOffset = m_MaskRect.rect.width * m_MaskRect.pivot.x;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (Application.isPlaying)
                Keyboard.current.onTextInput += HandleKeyboardInput;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Keyboard.current.onTextInput -= HandleKeyboardInput;
        }
        
        protected override void Update()
        {
            if (!Application.isPlaying)
                return;

            base.Update();

            if (Selected)
            {
                m_Caret.gameObject.SetActive(SelectionLength == 0);
                HandleExtraKeys();

                if (Mouse.current.leftButton.wasReleasedThisFrame || Mouse.current.rightButton.wasReleasedThisFrame)
                    mouseIsDragging = false;

                if (mouseIsDragging && (Mouse.current.leftButton.isPressed || Mouse.current.rightButton.isPressed))
                {
                    int index = MousePositionToCaretIndex();
                    SelectionIndices = new(SelectionIndices.x, index);
                }
                
                m_Caret.rectTransform.anchoredPosition = Vector3.Lerp(m_Caret.rectTransform.anchoredPosition, caretAnchor, smoothDamping * Time.deltaTime);
            }
        }

        protected virtual void OnSubmit()
        {
            DeselectAll();
            m_SelectionRect.anchoredPosition = caretAnchor;
            m_SelectionRect.sizeDelta = new(0.0f, m_SelectionRect.sizeDelta.y);
            OnReformatInputText();
        }
        
        protected virtual void OnReformatInputText() { }
        
        protected override void OnContainerSelect()
        {
            CaretIndex = Text.Length == 0 ? -1 : Text.Length - 1;
            SelectAllText();
        }

        protected override void OnContainerDeselect()
        {
            OnSubmit();
            m_Caret.gameObject.SetActive(false);
        }
        
        protected override void OnContainerMouseDown()
        {
            if (Selected)
            {
                CaretIndex = MousePositionToCaretIndex();
                DeselectAll();
                mouseIsDragging = true;
            }
        }

        #region Display
        
        #if UNITY_EDITOR
        
        protected override void OnValidate()
        {
            base.OnValidate();
            UpdateDisplayText();
        }
        
        #endif

        private void UpdateDisplayText()
        {
            m_PlaceholderTextDisplay.gameObject.SetActive(m_Text.Length == 0);
            m_TextDisplay.text = m_Text;
        }

        #endregion

        #region Text Selection Logic

        private void SelectAllText()
        {
            SelectionIndices = new(-1, Text.Length - 1);
        }

        private void UpdateSelectionDisplay()
        {
            float lhsx = SelectionMin == -1 ? 0.0f : GetxAdvance(SelectionMin) + pivotOffset;
            float rhsx = SelectionMax == -1 ? 0.0f : GetxAdvance(SelectionMax) + pivotOffset;

            m_SelectionRect.anchoredPosition = new(lhsx, m_SelectionRect.anchoredPosition.y);
            m_SelectionRect.sizeDelta = new(rhsx - lhsx, m_SelectionRect.sizeDelta.y);
        }

        private void DeleteSelectedText()
        {
            if (SelectionLength == 0)
                return;

            Text = Text.Remove(SelectionMin + 1, SelectionLength);
            CaretIndex = SelectionMin;
            DeselectAll();
        }

        private void DeselectAll()
        {
            SelectionIndices = new(CaretIndex, CaretIndex);
        }

        #endregion

        #region Caret Logic

        private void MoveCaret(int direction, bool shift)
        {
            if (shift)
            {
                if (SelectionLength == 0)
                    SelectionIndices = new(CaretIndex, CaretIndex + direction);
                else
                    SelectionIndices = new(SelectionIndices.x, SelectionIndices.y + direction);
            }
            else
            {
                if (SelectionLength == 0)
                    CaretIndex += direction;
                else
                {
                    CaretIndex = direction > 0 ? SelectionMax : SelectionMin;
                    DeselectAll();
                }
            }
        }

        private void UpdateCaretAnchor()
        {
            if (CaretIndex == -1)
            {
                caretAnchor = Vector3.zero;
                return;
            }
            
            float caretX = GetxAdvance(CaretIndex);
            caretAnchor = new(caretX + pivotOffset, m_Caret.rectTransform.anchoredPosition.y);
        }

        private int MousePositionToCaretIndex()
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(m_MaskRect, Mouse.current.position.ReadValue(), null, out Vector2 point);

            for (int i = 0; i < textInfo.characterCount; i++)
            {
                TMP_CharacterInfo c = textInfo.characterInfo[i];

                float midpoint = (c.origin + c.xAdvance) * 0.5f;

                if (point.x < midpoint)
                    return i - 1;
            }

            return textInfo.characterCount - 1;
        }

        #endregion

        #region Handle User Interactions
        
        private void HandleKeyboardInput(char character)
        {
            if (!Selected || HandleKeyboardCombinations(character))
                return;

            if (char.IsControl(character))
            {
                HandleControlKeys(character);
                return;
            }

            // Extra check to ensure we don't be typing some bullshit
            if (char.IsLetterOrDigit(character) || char.IsWhiteSpace(character))
            {
                if (SelectionLength > 0)
                    DeleteSelectedText();

                Text = Text.Insert(CaretIndex + 1, character.ToString());
                CaretIndex++;
            }
        }

        private bool HandleKeyboardCombinations(char c)
        {
            if (c is (char)1)
            {
                SelectAllText();
                return true;
            }

            return false;
        }

        private void HandleExtraKeys()
        {
            foreach((Key, Action) k in repeatKeys)
            {
                if (Keyboard.current[k.Item1].wasPressedThisFrame)
                {
                    StopAllCoroutines();
                    StartCoroutine(RepeatKey(k));
                }
            }
        }

        private void DeleteKey()
        {
            if (SelectionLength > 0)
                DeleteSelectedText();
            else
            {
                if (CaretIndex < Text.Length - 1)
                    Text = Text.Remove(CaretIndex + 1, 1);
            }
        }

        IEnumerator RepeatKey((Key key, Action action) keyFunc)
        {
            keyFunc.action();

            yield return new WaitForSeconds(KEY_REPEAT_DELAY);
            
            while (Keyboard.current[keyFunc.key].isPressed)
            {
                keyFunc.action();
                yield return new WaitForSeconds(KEY_REPEAT_SPEED);
            }
        }

        private void HandleControlKeys(char c)
        {
            // Backspace
            if (c == '\b')
            {
                if (SelectionLength > 0)
                    DeleteSelectedText();
                else
                {
                    if (CaretIndex > -1)
                    {
                        Text = Text.Remove(CaretIndex, 1);
                        CaretIndex--;
                    }
                }
            }

            // Enter
            if (c is '\r')
            {
                DeselectContainer();
            }
        }

        #endregion

        private float GetxAdvance(int index)
        {
            m_TextDisplay.ForceMeshUpdate();
            return textInfo.characterInfo[index].xAdvance;
        }
    }
}
