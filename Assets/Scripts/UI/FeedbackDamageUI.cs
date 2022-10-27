using UnityEngine;
using UnityEngine.UI;

public class FeedbackDamageUI : MonoBehaviour, IMessageListener
{
    public Text damageLabel;

    public Color regularColor = new Color(1, 1, 1);
    public Color criticalColor = new Color(1, 1, 1);

    public Vector3 offset = new Vector3(.5f, .5f, 0);

    void Start()
    {
        this.Hide();

        MessageManager.current.AddListener(MessageTag.SKILL_DAMAGE, this);
    }

    public void Receive(Message msg)
    {
        SkillDamageMessage sdm = msg as SkillDamageMessage;
        this.Configure(sdm.receiver.transform.position, sdm.damage, sdm.critical);
    }

    public void Configure(Vector3 position, int damageAmount, bool isCritical)
    {
        this.transform.position = position + this.offset;

        this.damageLabel.text = damageAmount.ToString();

        if (isCritical)
            this.damageLabel.color = this.criticalColor;
        else
            this.damageLabel.color = this.regularColor;

        this.Show();
        Invoke("Hide", 2f);
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}