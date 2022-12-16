using UnityEngine;
using UnityEngine.UI;

public class SkillFeedbackUI : MonoBehaviour
{
    public Text damageLabel;
    public Text missLabel;

    public Color regularColor = new Color(1, 1, 1);
    public Color criticalColor = new Color(1, 1, 1);

    public bool isHidden { get; protected set; }
    private int damageSum = 0;

    public float moveSpeed = .5f;
    public Vector3 offset = new Vector3(.5f, .5f, 0);

    public Creature receiver { get; protected set; }

    private void Update()
    {
        this.transform.position = this.transform.position + Vector3.up * this.moveSpeed * Time.deltaTime;
    }

    public void ConfigureForMiss(Creature receiver)
    {
        this.receiver = receiver;

        this.damageLabel.gameObject.SetActive(false);
        this.missLabel.gameObject.SetActive(true);

        this.transform.position = this.receiver.transform.position + offset;

        this.Show();
        Invoke("Hide", 2f);
    }

    public void ConfigureForDamage(Creature receiver, int damageAmount, bool isCritical)
    {
        this.receiver = receiver;

        this.damageLabel.gameObject.SetActive(true);
        this.missLabel.gameObject.SetActive(false);

        this.damageSum += damageAmount;

        this.transform.position = this.receiver.transform.position + this.offset;

        this.damageLabel.text = damageSum.ToString();

        if (isCritical)
            this.damageLabel.color = this.criticalColor;
        else
            this.damageLabel.color = this.regularColor;

        if (this.isHidden)
        {
            this.Show();
            Invoke("Hide", 2f);
        }
    }

    public void Show()
    {
        this.isHidden = false;
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.damageSum = 0;
        this.isHidden = true;
        this.gameObject.SetActive(false);
    }
}