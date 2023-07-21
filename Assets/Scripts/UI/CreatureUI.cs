using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CreatureUI : MonoBehaviour, IMessageListener
{
    public GameObject[] energyBlocks;

    public HealthBarUI healthBar;

    public Text elementalTypeLabel;
    public Text levelLabel;

    public Slider expSlider;

    public DynamicItemUIList dynButtonList;
    public DynamicItemUIList dynStatList;

    public StatusConditionListUI statusConditionListUI;

    protected Creature selectedCreature;

    void Start()
    {
        MessageManager.current.AddListener(MessageTag.CREATURE_SELECTED, this);
        MessageManager.current.AddListener(MessageTag.CREATURE_UPDATED, this);

        this.dynButtonList.ConfigureAndHide();
        this.dynStatList.ConfigureAndHide();

        this.Hide();
    }

    public void DisplayStats(Stats baseStats, Stats currentStats)
    {
        this.elementalTypeLabel.text = baseStats.elementalType.ToString();
        this.levelLabel.text = "Lv. " + currentStats.level;

        this.DisplayEnergy(currentStats.energy);

        this.healthBar.SetHealth(currentStats.hp, currentStats.maxhp);

        this.dynStatList.GetNextItemAndActivate<SingleStatUI>().Configure("Atk", baseStats.attack, currentStats.attack);
        this.dynStatList.GetNextItemAndActivate<SingleStatUI>().Configure("Def", baseStats.defense, currentStats.defense);
        this.dynStatList.GetNextItemAndActivate<SingleStatUI>().Configure("Spd", baseStats.speed, currentStats.speed);
        this.dynStatList.GetNextItemAndActivate<SingleStatUI>().Configure("EAtk", baseStats.elemAttack, currentStats.elemAttack);
        this.dynStatList.GetNextItemAndActivate<SingleStatUI>().Configure("EDef", baseStats.elemDefense, currentStats.elemDefense);
        this.dynStatList.GetNextItemAndActivate<SingleStatUI>().Configure("Acc", baseStats.accuracy, currentStats.accuracy);
        this.dynStatList.GetNextItemAndActivate<SingleStatUI>().Configure("Eva", baseStats.evasion, currentStats.evasion);
    }

    public void DisplayEnergy(int energy)
    {
        foreach (var block in this.energyBlocks)
        {
            block.SetActive(false);
        }

        for (int i = 0; i < energy; i++)
        {
            this.energyBlocks[i].SetActive(true);
        }
    }

    public void AddSkillButtton(string skillName, Skill skill, UnityAction onClick)
    {
        SkillButton btn = this.dynButtonList.GetNextItemAndActivate<SkillButton>();
        btn.Configure(skillName, onClick, skill);
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);

        this.dynButtonList.HideAll();
        this.dynStatList.HideAll();
    }

    public void Receive(Message msg)
    {
        if (msg is CreatureSelectedMessage)
        {
            CreatureSelectedMessage csm = msg as CreatureSelectedMessage;

            this.dynButtonList.HideAll();
            this.dynStatList.HideAll();

            this.selectedCreature = csm.creature;
            if (this.selectedCreature != null)
            {
                Stats baseStats = this.selectedCreature.GetBaseStats();

                this.Show();
                this.DisplayStats(baseStats, this.selectedCreature.GetCurrentStats());
                this.statusConditionListUI.DisplayStatusConditions(this.selectedCreature.GetCurrentStatusConditions());

                this.expSlider.value = baseStats.experiencePercent;

                if (this.selectedCreature.master is HumanMaster)
                {
                    Skill[] skills = this.selectedCreature.GetSkills();

                    this.AddSkillButtton("Move", null, () =>
                    {
                        MessageManager.current.Send(
                            new CreatureActionMoveMessage(this.selectedCreature)
                        );
                    });

                    foreach (var skill in skills)
                    {
                        this.AddSkillButtton(skill.skillName, skill, () =>
                        {
                            MessageManager.current.Send(
                                new CreatureActionSkillMessage(this.selectedCreature, skill)
                            );
                        });
                    }
                }
            }
            else
            {
                this.Hide();
            }
        }


        if (msg is CreatureUpdatedMessage)
        {
            CreatureUpdatedMessage cm = msg as CreatureUpdatedMessage;

            if (this.selectedCreature == cm.creature)
            {
                this.dynStatList.HideAll();
                this.DisplayStats(this.selectedCreature.GetBaseStats(), this.selectedCreature.GetCurrentStats());
                this.statusConditionListUI.DisplayStatusConditions(this.selectedCreature.GetCurrentStatusConditions());
            }
        }
    }
}
