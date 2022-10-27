using UnityEngine;

public class FeedbackMissUI : MonoBehaviour, IMessageListener
{
    public Vector3 offset = new Vector3(.5f, .5f, 0);

    private void Start()
    {
        this.Hide();

        MessageManager.current.AddListener(MessageTag.SKILL_MISS, this);
    }

    public void Receive(Message msg)
    {
        SkillMissMessage smm = msg as SkillMissMessage;
        this.Configure(smm.receiver.transform.position);
    }

    public void Configure(Vector3 position)
    {
        this.transform.position = position + offset;

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