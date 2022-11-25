using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CreatureUI : MonoBehaviour, IMessageListener
{
    public GameObject[] energyBlocks;

    public Slider healthSlider;

    public DynamicItemUIList dynButtonList;
    public DynamicItemUIList dynStatList;

    protected Creature selectedCreature;

    void Start()
    {
        MessageManager.current.AddListener(MessageTag.CREATURE_SELECTED, this);
        MessageManager.current.AddListener(MessageTag.CREATURE_UPDATED, this);

        this.dynButtonList.ConfigureAndHide();
        this.dynStatList.ConfigureAndHide();

        this.Hide();
    }

    public void DisplayStats(Stats stats)
    {
        this.DisplayEnergy(stats.energy);

        this.healthSlider.value = stats.hp / (float)stats.maxhp;

        this.dynStatList.GetNextItemAndActivate<SingleStatUI>().Configure("Atk", stats.attack);
        this.dynStatList.GetNextItemAndActivate<SingleStatUI>().Configure("Def", stats.defense);
        this.dynStatList.GetNextItemAndActivate<SingleStatUI>().Configure("Spd", stats.speed);
        this.dynStatList.GetNextItemAndActivate<SingleStatUI>().Configure("EAtk", stats.elemAttack);
        this.dynStatList.GetNextItemAndActivate<SingleStatUI>().Configure("EAtk", stats.elemDefense);

        this.dynStatList.GetNextItemAndActivate<SingleStatUI>().Configure("Acc", stats.accuracy);
        this.dynStatList.GetNextItemAndActivate<SingleStatUI>().Configure("Eva", stats.evasion);
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

    public void AddSkillButtton(string skillName, UnityAction onClick)
    {
        SkillButton btn = this.dynButtonList.GetNextItemAndActivate<SkillButton>();
        btn.Configure(skillName, onClick);
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
                this.Show();
                this.DisplayStats(this.selectedCreature.GetCurrentStats());

                if (this.selectedCreature.master is HumanMaster)
                {
                    Skill[] skills = this.selectedCreature.GetSkills();

                    this.AddSkillButtton("Move", () =>
                    {
                        MessageManager.current.Send(
                            new CreatureActionMoveMessage(this.selectedCreature)
                        );
                    });

                    foreach (var skill in skills)
                    {
                        this.AddSkillButtton(skill.name, () =>
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
                this.DisplayStats(this.selectedCreature.GetCurrentStats());
            }
        }
    }
}
