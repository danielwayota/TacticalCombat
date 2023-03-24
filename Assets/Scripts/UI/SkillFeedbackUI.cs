using UnityEngine;
using UnityEngine.UI;

public class SkillFeedbackUI : MonoBehaviour
{
    public Text healthModLabel;
    public Text missLabel;

    public Color regularColor = new Color(1, 1, 1);
    public Color healColor = new Color(1, 1, 1);
    public Color criticalColor = new Color(1, 1, 1);

    public bool isHidden { get; protected set; }
    private int healthSum = 0;

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

        this.healthModLabel.gameObject.SetActive(false);
        this.missLabel.gameObject.SetActive(true);

        this.transform.position = this.receiver.transform.position + offset;

        this.Show();
        Invoke("Hide", 2f);
    }

    public void ConfigureForHealthMod(Creature receiver, int healthAmount, bool isCritical)
    {
        this.receiver = receiver;

        this.healthModLabel.gameObject.SetActive(true);
        this.missLabel.gameObject.SetActive(false);

        this.healthSum += healthAmount;

        this.transform.position = this.receiver.transform.position + this.offset;

        this.healthModLabel.text = healthSum.ToString();

        if (this.healthSum == 0)
        {
            this.Hide();
            return;
        }

        if (this.healthSum > 0)
        {
            this.healthModLabel.color = this.healColor;
            this.healthModLabel.text = "+" + this.healthModLabel.text;
        }
        else if (this.healthSum < 0)
        {
            if (isCritical)
                this.healthModLabel.color = this.criticalColor;
            else
                this.healthModLabel.color = this.regularColor;
        }

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
        this.healthSum = 0;
        this.isHidden = true;
        this.gameObject.SetActive(false);
    }
}