using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using TMPro;
using Unity.VisualScripting;

public class CustomDropdown : MonoBehaviour, IPointerClickHandler
{
    public TMP_Dropdown dropdown; 
    public List<CustomDropdownOption> customOptions = new List<CustomDropdownOption>();
    public Image captionImage;

    [System.Serializable]
    public class CustomDropdownOption
    {
        public string optionText;
        public Sprite backgroundImage;
    }

    void Start()
    {
        RefreshOptions();

        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);

        
        OnDropdownValueChanged(dropdown.value);
    }

    public void RefreshOptions()
    {
        dropdown.options.Clear();
        foreach (var customOption in customOptions)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData(customOption.optionText));
        }
        dropdown.RefreshShownValue();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        dropdown.OnPointerClick(eventData);

        StartCoroutine(SetDropdownItemBackgrounds());
    }

    private IEnumerator<WaitForEndOfFrame> SetDropdownItemBackgrounds()
    {
        yield return new WaitForEndOfFrame();

        Transform dropdownList = dropdown.transform.Find("Dropdown List");
        if (dropdownList != null)
        {
            var items = dropdownList.GetComponentsInChildren<Toggle>();
            for (int i = 0; i < items.Length && i < customOptions.Count; i++)
            {
                var item = items[i];
                var customOption = customOptions[i];

                Transform itemBackground = item.transform.Find("Item Background");
                if (itemBackground != null)
                {
                    Image backgroundImage = itemBackground.GetComponent<Image>();

                    if (backgroundImage != null && customOption.backgroundImage != null)
                    {
                        backgroundImage.sprite = customOption.backgroundImage;
                    }
                }
            }
        }
    }

    private void OnDropdownValueChanged(int index)
    {
        if (captionImage != null && index >= 0 && index < customOptions.Count)
        {
            Sprite selectedSprite = customOptions[index].backgroundImage;
            if (selectedSprite != null)
            {
                captionImage.sprite = selectedSprite;
            }
        }
    }
}
