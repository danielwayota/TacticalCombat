using System.Globalization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TeamCreatureListItemUI : MonoBehaviour
{
    public Text creatureNameLabel;
    public Text creatureLevelLabel;

    public Slider healthSlider;
    public Slider expSlider;

    public Button showDetalisBtn;

    public void Configure(CreatureData creatureData, UnityAction onClick)
    {
        this.creatureNameLabel.text = creatureData.id;
        this.creatureLevelLabel.text = "Lvl. " + creatureData.level;

        this.healthSlider.value = creatureData.stats.healthPercent;
        this.expSlider.value = creatureData.stats.experiencePercent;

        this.showDetalisBtn.onClick.RemoveAllListeners();
        this.showDetalisBtn.onClick.AddListener(onClick);
    }
}